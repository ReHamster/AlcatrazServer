using QuazalWV.DDL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuazalWV
{
    public abstract class RMCPResponse
    {
        public abstract override string ToString();
        public abstract string PayloadToString();
        public abstract byte[] ToBuffer();
    }

	public class RMCPResponseEmpty : RMCPResponse
	{
		public override byte[] ToBuffer()
		{
			return new byte[0];
		}

		public override string ToString()
		{
			return "[RMCPacketResponseEmpty]";
		}

		public override string PayloadToString()
		{
			return "";
		}
	}

	// Wrapper class for DDL (or ANY object)
	public class RMCPResponseDDL<T> : RMCPResponse where T : class
	{
		public RMCPResponseDDL(T data)
		{
			objectData = data;
		}
		T objectData;

		public override string PayloadToString()
		{
			return "";
		}

		public override byte[] ToBuffer()
		{
			var m = new MemoryStream();
			DDLSerializer.WriteObject(objectData, m);

			return m.ToArray();
		}

		public override string ToString()
		{
			return $"[ResponseDDL<{typeof(T).Name}>]";
		}
	}
}
