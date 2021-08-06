using System.IO;
using System.Text;

namespace QuazalWV
{
	public class RMCPacket
    {
        public RMCProtocolId proto;
        public bool isRequest;
        public bool success;
        public uint error;
        public uint callID;
        public uint methodID;
        public RMCPRequest request;
        public int _afterProtocolOffset;

        public RMCPacket()
        {
        }

        public RMCPacket(QPacket p)
        {
            MemoryStream m = new MemoryStream(p.payload);
            Helper.ReadU32(m);
            ushort b = Helper.ReadU8(m);
            isRequest = (b >> 7) == 1;
            try
            {
                if ((b & 0x7F) != 0x7F)
                    proto = (RMCProtocolId)(b & 0x7F);
                else
                {
                    b = Helper.ReadU16(m);
                    proto = (RMCProtocolId)(b);
                }
            }
            catch
            {
                Log.WriteLine(1, "[RMC Packet] Error: Unknown RMC packet protocol 0x" + b.ToString("X2"));
                return;
            }
            _afterProtocolOffset = (int)m.Position;
        }
        

        public override string ToString()
        {
            return "[RMC Packet : Proto = " + proto + " CallID=" + callID + " MethodID=" + methodID + "]";
        }

        public string PayLoadToString()
        {
            StringBuilder sb = new StringBuilder();
            if (request != null)
                sb.Append(request);
            return sb.ToString();
        }

        public byte[] ToBuffer()
        {
            MemoryStream result = new MemoryStream();
            byte[] buff = request.ToBuffer();
            Helper.WriteU32(result, (uint)(buff.Length + 9));
            byte b = (byte)proto;
            if (isRequest)
                b |= 0x80;
            Helper.WriteU8(result, b);
            Helper.WriteU32(result, callID);
            Helper.WriteU32(result, methodID);
            result.Write(buff, 0, buff.Length);
            return result.ToArray();
        }
    }
}
