using System.IO;
using System.Text;

namespace QNetZ
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
		public RMCPResponse response;
		public uint _afterProtocolOffset;
		public uint requestSize;

		public RMCPacket()
		{
		}

		public RMCPacket(QPacket p)
		{
			var m = new MemoryStream(p.payload);

			// the request info size + data size
			requestSize = Helper.ReadU32(m);

			ushort b = Helper.ReadU8(m);
			isRequest = (b >> 7) == 1;

			try
			{
				if ((b & 0x7F) != 0x7F)
				{
					proto = (RMCProtocolId)(b & 0x7F);
				}
				else
				{
					b = Helper.ReadU16(m);
					proto = (RMCProtocolId)(b);
				}
			}
			catch
			{
				QLog.WriteLine(1, "[RMC Packet] Error: Unknown RMC packet protocol 0x" + b.ToString("X2"));
				return;
			}

			if (isRequest)
			{
				callID = Helper.ReadU32(m);
				methodID = Helper.ReadU32(m);
			}
			else
			{
				// response
				success = m.ReadByte() == 1;

				if (success)
				{
					callID = Helper.ReadU32(m);
					methodID = Helper.ReadU32(m) & (~0x8000u);
				}
				else
				{
					methodID = Helper.ReadU32(m) & (~0x8000u);
					error = Helper.ReadU32(m);
				}
			}

			_afterProtocolOffset = (uint)m.Position;
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
			var packetData = new MemoryStream();

			if ((ushort)proto < 0x7F)
			{
				// request has 0x80 flag
				uint protoIdent = (uint)proto | (isRequest ? 0x80u : 0x0u);
				Helper.WriteU8(packetData, (byte)protoIdent);
			}
			else
			{
				uint protoIdent = 0x7Fu | (isRequest ? 0x80u : 0x0u);
				Helper.WriteU8(packetData, (byte)protoIdent);
				Helper.WriteU16(packetData, (ushort)proto);
			}

			byte[] buff;

			if (isRequest)
			{
				Helper.WriteU32(packetData, callID);
				Helper.WriteU32(packetData, methodID);

				// write request payload
				buff = request.ToBuffer();
				packetData.Write(buff, 0, buff.Length);
			}
			else
			{
				// response
				success = error == 0;
				packetData.WriteByte((byte)(success ? 1 : 0));

				if (success)
				{
					Helper.WriteU32(packetData, callID);
					Helper.WriteU32(packetData, methodID | 0x8000);

					// write response payload
					buff = response.ToBuffer();
					packetData.Write(buff, 0, buff.Length);
				}
				else
				{
					Helper.WriteU32(packetData, methodID | 0x8000);
					Helper.WriteU32(packetData, error);
				}
			}

			buff = packetData.ToArray();
			packetData = new MemoryStream();
			
			Helper.WriteU32(packetData, (uint)buff.Length);
			packetData.Write(buff, 0, buff.Length);

			return packetData.ToArray();
		}
	}
}
