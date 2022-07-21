using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace QNetZ
{
	class QPacketState
	{
		public QPacketState(QPacket p)
		{
			Packet = p;
			GotAck = false;
			ReSendCount = 0;
		}
		public QPacket Packet;
		public int ReSendCount;
		public bool GotAck;                // if true it won't be sent again
	}

	class QReliableResponse
	{
		public QReliableResponse(QPacket srcPacket, IPEndPoint endpoint)
		{
			SrcPacket = srcPacket;
			ResponseList = new List<QPacketState>();
			DropTime = DateTime.UtcNow.AddSeconds(18);
			ResendTime = DateTime.UtcNow.AddSeconds(Constants.PacketResendTimeSeconds);
			Endpoint = endpoint;
		}

		public QPacket SrcPacket;
		public List<QPacketState> ResponseList;

		public DateTime DropTime;		// if ACKs not recieved, in this time it will be dropped
		public DateTime ResendTime;
		public IPEndPoint Endpoint;     // client endpoint
	}

	//-----------------------------------------------------

	public partial class QPacketHandlerPRUDP
	{
		bool Defrag(QClient client, QPacket packet)
		{
			if (packet.flags.Contains(QPacket.PACKETFLAG.FLAG_ACK))
				return true;

			if (!packet.flags.Contains(QPacket.PACKETFLAG.FLAG_RELIABLE))
				return true;

			if (!AccumulatedPackets.Any(x =>
				 x.uiSeqId == packet.uiSeqId &&
				 x.checkSum == packet.checkSum &&
				 x.m_byPartNumber == packet.m_byPartNumber &&
				 x.checkSum == packet.checkSum &&
				 x.m_bySessionID == packet.m_bySessionID &&
				 x.m_uiSignature == packet.m_uiSignature
				))
			{
				AccumulatedPackets.Add(packet);
			}

			if (packet.m_byPartNumber != 0)
			{
				// add and don't process
				return false;
			}

			// got last fragment, assemble
			var orderedFragments = AccumulatedPackets.OrderBy(x => x.uiSeqId);
			int numPackets = 0;
			foreach (var fragPacket in orderedFragments)
			{
				numPackets++;
				if (fragPacket.m_byPartNumber == 0)
					break;
			}

			var fragments = orderedFragments.Take(numPackets).ToArray();

			// remove fragments that we processed
			AccumulatedPackets.Clear();//RemoveAll(x => fragments.Contains(x));

			if (numPackets > 1)
			{
				// validate algorightm above
				ushort seqId = fragments.First().uiSeqId;
				int nfrag = 1;
				foreach (var fragPacket in fragments)
				{
					//if(fragPacket.uiSeqId != seqId)
					//{
					//	QLog.WriteLine(1, "ERROR : packet sequence mismatch!");
					//	//return false;
					//}

					if (fragments.Length == nfrag && fragPacket.m_byPartNumber != 0)
					{
						QLog.WriteLine(1, "ERROR : packet sequence does not end with 0 - call a programmer!");
						return false;
					}

					if (!(fragPacket.m_byPartNumber == 0 && fragments.Length == nfrag))
					{
						if (fragPacket.m_byPartNumber != nfrag)
						{
							QLog.WriteLine(1, "ERROR : insufficient packet fragments - call a programmer!");
							return false;
						}
					}

					seqId++;
					nfrag++;
				}

				// acks are required for each packet
				foreach (var fragPacket in fragments)
				{
					if (fragPacket.flags.Contains(QPacket.PACKETFLAG.FLAG_NEED_ACK))
						SendACK(fragPacket, client);
				}

				var fullPacketData = new MemoryStream();
				foreach (var fragPacket in fragments)
					fullPacketData.Write(fragPacket.payload, 0, fragPacket.payload.Length);

				// replace packet payload with defragmented data
				packet.payload = fullPacketData.ToArray();
				packet.payloadSize = (ushort)fullPacketData.Length;

				QLog.WriteLine(10, $"Defragmented sequence of {numPackets} packets !\n");
			}

			return true;
		}

		// acknowledges packet
		void OnGotAck(QPacket ackPacket)
		{
			var (cr, ack) = GetCachedResponseByAckPacket(ackPacket);
			if (cr == null)
				return;

			ack.GotAck = true;

			// can safely remove cache?
			if (cr.ResponseList.All(x => x.GotAck))
			{
				QLog.WriteLine(10, "[QPacketReliable] sequence completed!");
				CachedResponses.Remove(cr);
			}
		}

		// returns response cache list by request packet
		(QReliableResponse, QPacketState) GetCachedResponseByAckPacket(QPacket packet)
		{
			foreach (var cr in CachedResponses)
			{
				var st = cr.ResponseList.FirstOrDefault(x =>
					 x.Packet.m_bySessionID == packet.m_bySessionID &&
					 x.Packet.uiSeqId == packet.uiSeqId);

				if (st != null)
					return (cr, st);
			}
			return (null, null);
		}

		// returns response cache list by request packet
		QReliableResponse GetCachedResponseByRequestPacket(QPacket packet)
		{
			if (packet == null)
				return null;

			if (packet.m_oSourceVPort == null)
			{
				QLog.WriteLine(1, "GetCachedResponseByRequestPacket - invalid packet SRC VPORT!\n");
				return null;
			}

			if (packet.m_oDestinationVPort == null)
			{
				QLog.WriteLine(1, "GetCachedResponseByRequestPacket - invalid packet DEST VPORT!\n");
				return null;
			}

			// delete all invalid messages
			CachedResponses.RemoveAll(x => 
				x.SrcPacket == null || 
				x.SrcPacket.m_oSourceVPort == null || 
				x.SrcPacket.m_oDestinationVPort == null);

			// FIXME: check packet type?
			return CachedResponses.FirstOrDefault(cr =>
					cr.SrcPacket.type == packet.type &&
					cr.SrcPacket.m_uiSignature == packet.m_uiSignature &&
					cr.SrcPacket.m_oSourceVPort.type == packet.m_oSourceVPort.type &&
					cr.SrcPacket.m_oSourceVPort.port == packet.m_oSourceVPort.port &&
					cr.SrcPacket.m_oDestinationVPort.type == packet.m_oDestinationVPort.type &&
					cr.SrcPacket.m_oDestinationVPort.port == packet.m_oDestinationVPort.port &&
					cr.SrcPacket.uiSeqId == packet.uiSeqId &&
					cr.SrcPacket.checkSum == packet.checkSum);
		}

		// Caches the response which is going to be sent
		bool CacheResponse(QPacket requestPacket, QPacket responsePacket, IPEndPoint ep)
		{
			// only DATA can be reliable
			if (responsePacket.type != QPacket.PACKETTYPE.DATA)
				return false;

			// don't cache non-reliable packets
			if (!responsePacket.flags.Contains(QPacket.PACKETFLAG.FLAG_RELIABLE))
				return false;

			if (responsePacket.flags.Contains(QPacket.PACKETFLAG.FLAG_ACK))
				return false;

			var cache = GetCachedResponseByRequestPacket(requestPacket);
			if (cache == null)
			{
				cache = new QReliableResponse(requestPacket, ep);
				CachedResponses.Add(cache);
			}
			else
			{
				QLog.WriteLine(10, "[QPacketReliable] Found cached request");
			}

			cache.ResponseList.Add(new QPacketState(responsePacket));
			return true;
		}

		private void RetrySend(QReliableResponse cache)
		{
			QLog.WriteLine(5, "Re-sending reliable packets...");

			foreach (var crp in cache.ResponseList.Where(x => x.GotAck == false))
			{
				var data = crp.Packet.toBuffer();
				crp.ReSendCount++;
				UDP.Send(data, data.Length, cache.Endpoint);
			}
		}

		private void CheckResendPackets()
		{
			CachedResponses.RemoveAll(x => x.SrcPacket == null);

			foreach(var crp in CachedResponses)
			{
				if (DateTime.UtcNow >= crp.ResendTime)
				{
					RetrySend(crp);
					crp.ResendTime = DateTime.UtcNow.AddSeconds(Constants.PacketResendTimeSeconds);
				}
			}

			// drop packets
			CachedResponses.RemoveAll(x => DateTime.UtcNow >= x.DropTime);
		}
	}
}
