using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace QNetZ
{
	public partial class QPacketHandlerPRUDP
	{
		public QPacketHandlerPRUDP(UdpClient udp, uint pid, ushort port, string sourceName = "PRUDP Handler")
		{
			UDP = udp;
			SourceName = sourceName;
			PID = pid;
			Port = port;
			NATPingTimeToIgnore = new List<ulong>();
			AccumulatedPackets = new List<QPacket>();
			CachedResponses = new List<QReliableResponse>();
			Clients = new List<QClient>();

			if(QConfiguration.Instance == null)
			{
				throw new Exception("QConfiguration.Instance is null! You need to configure it first!");
			}
		}

		private readonly UdpClient UDP;
		public string SourceName;
		public readonly uint PID;
		public readonly ushort Port;
		private List<QPacket> AccumulatedPackets = new List<QPacket>();
		private List<QReliableResponse> CachedResponses = new List<QReliableResponse>();
		private readonly List<ulong> NATPingTimeToIgnore;

		public List<QClient> Clients;

		public uint ClientIdCounter = 0x12345678;	// or client signature

		private QPacket ProcessSYN(QPacket p, IPEndPoint from)
		{
			// create protocol client
			var qclient = GetQClientByEndPoint(from);
			if(qclient == null)
			{
				qclient = NewQClient(from);
			}

			QLog.WriteLine(2, $"[{ SourceName }] Got SYN packet");
			qclient.seqCounterOut = 0;

			p.m_uiConnectionSignature = qclient.IDrecv;

			return MakeACK(p, qclient);
		}

		private QPacket ProcessCONNECT(QClient client, QPacket p)
		{
			client.IDsend = p.m_uiConnectionSignature;

			QLog.WriteLine(2, $"[{ SourceName }] Got CONNECT packet");

			var reply = MakeACK(p, client);

			if (p.payload != null && p.payload.Length > 0)
				reply.payload = MakeConnectPayload(client, p);

			return reply;
		}

		private byte[] MakeConnectPayload(QClient client, QPacket p)
		{
			var m = new MemoryStream(p.payload);

			// read kerberos ticket
			// TODO: decrypt it and read session key instead of using Constants.SessionKey
			uint size = Helper.ReadU32(m);
			byte[] kerberosTicket = new byte[size];
			m.Read(kerberosTicket, 0, (int)size);

			// read encrypted data
			size = Helper.ReadU32(m) - 16;
			byte[] buff = new byte[size];
			m.Read(buff, 0, (int)size);

			buff = Helper.Decrypt(Constants.SessionKey, buff);

			m = new MemoryStream(buff);
			uint userPrincipalID = Helper.ReadU32(m);
			uint connectionId = Helper.ReadU32(m);		// TODO: utilize

			client.info = NetworkPlayers.GetPlayerInfoByPID(userPrincipalID);

			uint responseCode = Helper.ReadU32(m);

			// Buffer<uint>
			m = new MemoryStream();
			Helper.WriteU32(m, 4);
			Helper.WriteU32(m, responseCode + 1);

			return m.ToArray();
		}

		private QPacket ProcessDISCONNECT(QClient client, QPacket p)
		{
			QPacket reply = MakeACK(p, client);
			reply.m_uiSignature = client.IDsend;

			QLog.WriteLine(2, $"[{ SourceName }] Got DISCONNECT packet");

			return reply;
		}

		private QPacket ProcessPING(QClient client, QPacket p)
		{
			return MakeACK(p, client);
		}

		public void Send(QPacket reqPacket, QPacket sendPacket, IPEndPoint ep)
		{
			byte[] data = sendPacket.toBuffer();
			StringBuilder sb = new StringBuilder();

			CacheResponse(reqPacket, new QPacket(data));

			foreach (byte b in data)
				sb.Append(b.ToString("X2") + " ");

			QLog.WriteLine(5,  () => $"[{ SourceName }] send : { sendPacket.ToStringShort()}");
			QLog.WriteLine(10, () => $"[{ SourceName }] send : { sb.ToString()}");
			QLog.WriteLine(10, () => $"[{ SourceName }] send : { sendPacket.ToStringDetailed() }");

			// TODO: bufferize in queue then send, that's how Quazal does it
			UDP.Send(data, data.Length, ep);

			QLog.LogPacket(true, data);
		}

		public QPacket MakeACK(QPacket p, QClient client)
		{
			QPacket np = new QPacket(p.toBuffer());
			np.flags = new List<QPacket.PACKETFLAG>() { QPacket.PACKETFLAG.FLAG_ACK, QPacket.PACKETFLAG.FLAG_HAS_SIZE };

			np.m_oSourceVPort = p.m_oDestinationVPort;
			np.m_oDestinationVPort = p.m_oSourceVPort;
			np.m_uiSignature = client.IDsend;
			np.payload = new byte[0];
			np.payloadSize = 0;
			return np;
		}

		public void SendACK(QPacket p, QClient client)
		{
			var np = MakeACK(p, client);
			var data = np.toBuffer();

			UDP.Send(data, data.Length, client.endpoint);
		}

		public void MakeAndSend(QClient client, QPacket reqPacket, QPacket newPacket, byte[] data)
		{
			var stream = new MemoryStream(data);

			int numFragments = 0;

			if (stream.Length > Constants.PacketFragmentMaxSize)
				newPacket.flags.AddRange(new[] { QPacket.PACKETFLAG.FLAG_HAS_SIZE });

			newPacket.uiSeqId = client.seqCounterOut;
			newPacket.m_byPartNumber = 1;
			while (stream.Position < stream.Length)
			{
				int payloadSize = (int)(stream.Length - stream.Position);

				if (payloadSize <= Constants.PacketFragmentMaxSize)
				{
					newPacket.m_byPartNumber = 0;  // indicate last packet
				}
				else
					payloadSize = Constants.PacketFragmentMaxSize;

				byte[] buff = new byte[payloadSize];
				stream.Read(buff, 0, payloadSize);

				newPacket.uiSeqId++;
				newPacket.payload = buff;
				newPacket.payloadSize = (ushort)newPacket.payload.Length;

				Send(reqPacket, newPacket, client.endpoint);

				newPacket.m_byPartNumber++;
				numFragments++;
			}

			client.seqCounterOut = newPacket.uiSeqId;

			QLog.WriteLine(10, $"[{ SourceName }] sent { numFragments } packets");
		}

		//-------------------------------------------------------------------------------------------

		public void ProcessPacket(byte[] data, IPEndPoint from, bool removeConnectPayload = false)
		{
			// first we drop old clients to proceed
			DropClients();

			while (true)
			{
				var packetIn = new QPacket(data);

				{
					var m = new MemoryStream(data);

					byte[] buff = new byte[(int)packetIn.realSize];
					m.Read(buff, 0, buff.Length);

					var sb = new StringBuilder();

					foreach (byte b in data)
						sb.Append(b.ToString("X2") + " ");

					QLog.LogPacket(false, buff);
					QLog.WriteLine(5,  ()=> $"[{ SourceName }] received : { packetIn.ToStringShort() }" );
					QLog.WriteLine(10, ()=> $"[{ SourceName }] received : { sb }" );
					QLog.WriteLine(10, ()=> $"[{ SourceName }] received : { packetIn.ToStringDetailed() }");
				}

				QPacket reply = null;
				QClient client = null;

				if (packetIn.type != QPacket.PACKETTYPE.SYN && packetIn.type != QPacket.PACKETTYPE.NATPING)
					client = GetQClientByIDrecv(packetIn.m_uiSignature);

				// update client to not time out him
				if (client != null && client.info != null)
				{
					client.LastPacketTime = DateTime.UtcNow;
				}

				switch (packetIn.type)
				{
					case QPacket.PACKETTYPE.SYN:
						reply = ProcessSYN(packetIn, from);
						break;
					case QPacket.PACKETTYPE.CONNECT:
						if (client != null && !packetIn.flags.Contains(QPacket.PACKETFLAG.FLAG_ACK))
						{
							client.sPID = PID;
							client.sPort = Port;

							if (removeConnectPayload)
							{
								packetIn.payload = new byte[0];
								packetIn.payloadSize = 0;
							}

							reply = ProcessCONNECT(client, packetIn);
						}
						break;
					case QPacket.PACKETTYPE.DATA:
						{
							// NOT VALID
							if (client == null)
								break;

							if (Defrag(client, packetIn) == false)
								break;

							// ack for reliable packets
							if (packetIn.flags.Contains(QPacket.PACKETFLAG.FLAG_ACK))
							{
								OnGotAck(packetIn);
								break;
							}

							// resend?
							var cache = GetCachedResponseByRequestPacket(packetIn);
							if (cache != null)
							{
								SendACK(packetIn, client);
								RetrySend(cache, client);
								break;
							}

							if (packetIn.m_oSourceVPort.type == QPacket.STREAMTYPE.RVSecure)
								RMC.HandlePacket(this, packetIn, client);

							if (packetIn.m_oSourceVPort.type == QPacket.STREAMTYPE.DO)
								DO.HandlePacket(this, packetIn, client);
						}
						break;
					case QPacket.PACKETTYPE.DISCONNECT:
						if (client != null)
							reply = ProcessDISCONNECT(client, packetIn);
						break;
					case QPacket.PACKETTYPE.PING:
						if (client != null)
							reply = ProcessPING(client, packetIn);
						break;
					case QPacket.PACKETTYPE.NATPING:

						ulong time = BitConverter.ToUInt64(packetIn.payload, 5);

						if (NATPingTimeToIgnore.Contains(time))
						{
							NATPingTimeToIgnore.Remove(time);
						}
						else
						{
							reply = packetIn;
							var m = new MemoryStream();
							byte b = (byte)(reply.payload[0] == 1 ? 0 : 1);

							m.WriteByte(b);

							Helper.WriteU32(m, 0x1234); //RVCID
							Helper.WriteU64(m, time);

							reply.payload = m.ToArray();

							Send(packetIn, reply, from);

							m = new MemoryStream();
							b = (byte)(b == 1 ? 0 : 1);

							m.WriteByte(b);
							Helper.WriteU32(m, 0x1234); //RVCID

							time = Helper.MakeTimestamp();

							NATPingTimeToIgnore.Add(time);

							Helper.WriteU64(m, Helper.MakeTimestamp());
							reply.payload = m.ToArray();
						}
						break;
				}

				if (reply != null)
					Send(packetIn, reply, from);

				// retry sending reliable packets
				CheckResendPackets(client);

				// more packets in data stream?
				if (packetIn.realSize != data.Length)
				{
					var m = new MemoryStream(data);

					int left = (int)(data.Length - packetIn.realSize);
					byte[] newData = new byte[left];

					m.Seek(packetIn.realSize, 0);
					m.Read(newData, 0, left);

					data = newData;
				}
				else
					break;
			}
		}
		private void DropClients()
		{
			foreach (var c in Clients)
			{
				if((DateTime.UtcNow - c.LastPacketTime).TotalSeconds > Constants.ClientTimeoutSeconds)
				{
					// also drop network players
					NetworkPlayers.DropPlayerInfo(c.info);
					Clients.Remove(c);
					return;
				}
			}
		}

		public QClient GetQClientByIDrecv(uint id)
		{
			foreach (var c in Clients)
			{
				if (c.IDrecv == id)
					return c;
			}

			QLog.WriteLine(1, $"[{ SourceName }] Error : Cant find client for id : 0x" + id.ToString("X8"));
			return null;
		}

		public QClient NewQClient(IPEndPoint from)
		{
			QLog.WriteLine(2, $"[{ SourceName }] [QUAZAL] New client { from.Address }:{ from.Port } registered at server PID={PID}");

			var qclient = new QClient(++ClientIdCounter, from);

			Clients.Add(qclient);
			return qclient;
		}

		public QClient GetQClientByEndPoint(IPEndPoint ep)
		{
			foreach (var c in Clients)
			{
				if (c.endpoint.Address.ToString() == ep.Address.ToString() && c.endpoint.Port == ep.Port)
					return c;
			}

			return null;
		}

		public QClient GetQClientByClientPID(uint userPID)
		{
			foreach (var c in Clients)
			{
				if (c.info == null)
					continue;

				// also check if timed out
				if ((DateTime.UtcNow - c.LastPacketTime).TotalSeconds > Constants.ClientTimeoutSeconds)
					continue;

				if (c.info.PID == userPID)
					return c;
			}

			return null;
		}
	}
}
