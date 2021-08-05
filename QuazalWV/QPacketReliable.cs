using System;
using System.Collections.Generic;
using System.Linq;
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

    //-----------------------------------------------------

    public static class QPacketReliable
	{
        private static List<QReliableResponse> CachedResponses = new List<QReliableResponse>();

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
                     x.Packet.m_uiSignature == packet.m_uiSignature &&
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
                    cr.SrcPacket.m_uiSignature == packet.m_uiSignature &&
                    cr.SrcPacket.m_oSourceVPort == packet.m_oSourceVPort &&
                    cr.SrcPacket.m_oDestinationVPort == packet.m_oDestinationVPort &&
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
