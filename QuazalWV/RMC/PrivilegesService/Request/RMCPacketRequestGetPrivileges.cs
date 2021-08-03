using System.IO;

namespace QuazalWV
{
	public class RMCPacketGetPrivileges : RMCPRequest
	{
		public string localeCode;

		public RMCPacketGetPrivileges(Stream s)
		{
			localeCode = Helper.ReadString(s);
		}

		public override string PayloadToString()
		{
			return "";
		}

		public override byte[] ToBuffer()
		{
			MemoryStream m = new MemoryStream();
			Helper.WriteString(m, localeCode);
			return m.ToArray();
		}

		public override string ToString()
		{
			return "[RMCPacketRequestGetPrivileges]";
		}
	}
}
