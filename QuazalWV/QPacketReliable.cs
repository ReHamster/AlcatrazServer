using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace QuazalWV
{
	public class QPacketState
	{
		public QPacketState(QPacket p)
		{
			Packet = p;
			GotAck = false;
		}
		public QPacket Packet;
		public bool GotAck;                // if true it won't be sent again
	}

	public class QReliableResponse
	{
		public QReliableResponse(QPacket srcPacket)
		{
			SrcPacket = srcPacket;
			ResponseList = new List<QPacketState>();
			Timeout = DateTime.UtcNow.AddMinutes(1);
		}

		public QPacket SrcPacket;
		public List<QPacketState> ResponseList;
		public DateTime Timeout;
	}

	public class QFragmentedPacket
	{
		public List<QPacket> Fragmets;
}

	//-----------------------------------------------------

	public static class QPacketReliable
	{
		private static List<QPacket> FragmentPackets = new List<QPacket>();
		private static List<QReliableResponse> CachedResponses = new List<QReliableResponse>();

		private static void SendACK(UdpClient udp, QPacket p, ClientInfo client)
		{
			QPacket np = new QPacket(p.toBuffer());
			np.flags = new List<QPacket.PACKETFLAG>() { QPacket.PACKETFLAG.FLAG_ACK, QPacket.PACKETFLAG.FLAG_HAS_SIZE };

			np.m_oSourceVPort = p.m_oDestinationVPort;
			np.m_oDestinationVPort = p.m_oSourceVPort;
			np.m_uiSignature = client.IDsend;
			np.payload = new byte[0];
			np.payloadSize = 0;

			byte[] data = p.toBuffer();

			udp.Send(data, data.Length, client.ep);
		}

		public static bool Defrag(ClientInfo client, QPacket packet)
		{
			if (packet.flags.Contains(QPacket.PACKETFLAG.FLAG_ACK))
				return true;

			if (!packet.flags.Contains(QPacket.PACKETFLAG.FLAG_RELIABLE))
				return true;

			if (!FragmentPackets.Any(x =>
				 x.uiSeqId == packet.uiSeqId &&
				 x.checkSum == packet.checkSum &&
				 x.m_byPartNumber == packet.m_byPartNumber &&
				 x.checkSum == packet.checkSum &&
				 x.m_bySessionID == packet.m_bySessionID &&
				 x.m_uiSignature == packet.m_uiSignature
				))
			{
				FragmentPackets.Add(packet);
			}

			if (packet.m_byPartNumber != 0)
			{
				// add and don't process
				return false;
			}

			// got last fragment, assemble
			var orderedFragments = FragmentPackets
				// .Where(x => 
				// 	x.m_bySessionID == packet.m_bySessionID && 
				// 	x.m_uiConnectionSignature == packet.m_uiConnectionSignature)
				.OrderBy(x => x.uiSeqId);

			int numPackets = 0;
			foreach (var fragPacket in orderedFragments)
			{
				numPackets++;
				if (fragPacket.m_byPartNumber == 0)
					break;
			}

			var fragments = orderedFragments.Take(numPackets).ToArray();

			// remove fragments that we processed
			FragmentPackets.Clear();//RemoveAll(x => fragments.Contains(x));

			if (numPackets > 1)
			{
				// validate algorightm above
				ushort seqId = fragments.First().uiSeqId;
				int nfrag = 1;
				foreach (var fragPacket in fragments)
				{
					// if(fragPacket.uiSeqId != seqId)
					// {
					// 	Log.WriteLine(1, "ERROR : invalid packet sequence for defragmenting - call a programmer!");
					// 	return false;
					// }

					if(fragments.Length == nfrag && fragPacket.m_byPartNumber != 0)
					{
						Log.WriteLine(1, "ERROR : packet sequence does not end with 0 - call a programmer!");
						return false;
					}

					if(!(fragPacket.m_byPartNumber == 0 && fragments.Length == nfrag))
					{
						if (fragPacket.m_byPartNumber != nfrag)
						{
							Log.WriteLine(1, "ERROR : insufficient packet fragments - call a programmer!");
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
						SendACK(client.udp, fragPacket, client);
				}

				var fullPacketData = new MemoryStream();
				foreach (var fragPacket in fragments)
					fullPacketData.Write(fragPacket.payload, 0, fragPacket.payload.Length);

				// replace packet payload with defragmented data
				packet.payload = fullPacketData.ToArray();
				packet.payloadSize = (ushort)fullPacketData.Length;

				Log.WriteLine(10, $"Defragmented sequence of {numPackets} packets !\n");
			}				

			return true;
		}

        // acknowledges packet
        public static void OnGotAck(QPacket ackPacket)
        {
            var (cr, ack) = GetCachedResponseByAckPacket(ackPacket);
            if (cr == null)
                return;

            ack.GotAck = true;

            // can safely remove cache?
            if (cr.ResponseList.All(x => x.GotAck))
			{
                Log.WriteLine(10, "[QPacketReliable] sequence completed!");
                CachedResponses.Remove(cr);
            }
        }

        // returns response cache list by request packet
        public static (QReliableResponse, QPacketState) GetCachedResponseByAckPacket(QPacket packet)
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
        public static QReliableResponse GetCachedResponseByRequestPacket(QPacket packet)
        {
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
        public static void CacheResponse(QPacket requestPacket, QPacket responsePacket)
        {
            // don't cache non-reliable packets
            if (!responsePacket.flags.Contains(QPacket.PACKETFLAG.FLAG_RELIABLE))
                return;

            if (responsePacket.flags.Contains(QPacket.PACKETFLAG.FLAG_ACK))
                return;

            var cache = GetCachedResponseByRequestPacket(requestPacket);
            if (cache == null)
            {
                cache = new QReliableResponse(requestPacket);
                CachedResponses.Add(cache);
            }
            else
			{
                Log.WriteLine(10, "[QPacketReliable] Found cached request");
			}

            cache.ResponseList.Add(new QPacketState(responsePacket));
        }
    }
}
