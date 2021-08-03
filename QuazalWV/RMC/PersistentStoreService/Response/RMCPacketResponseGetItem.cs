using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuazalWV
{
	public class RMCPacketResponseGetItem : RMCPResponse
	{
		public byte[] buffer; // QBuffer
		public bool result;

		public RMCPacketResponseGetItem()
		{
			buffer = new byte []{ };
			result = true;
		}

		public override string PayloadToString()
		{
			return "";
		}

		public override byte[] ToBuffer()
		{
			var m = new MemoryStream();

			Helper.WriteU32(m, (uint)buffer.Length);
			m.Write(buffer, 0, buffer.Length);

			Helper.WriteBool(m, result);

			return m.ToArray();
		}

		public override string ToString()
		{
			return "[RMCPacketResponseGetItem]";
		}
	}
}
