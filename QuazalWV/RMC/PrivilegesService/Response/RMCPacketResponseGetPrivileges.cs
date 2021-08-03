using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuazalWV
{
	public class Map_U32_Privilege
	{
		public uint key;
		public Privilege privilege;
		public void toBuffer(Stream s)
		{
			Helper.WriteU32(s, key);
			privilege.toBuffer(s);
		}
	}

	public class Privilege
	{
		uint m_ID;
		string m_description;

		public void toBuffer(Stream s)
		{
			Helper.WriteU32(s, m_ID);
			Helper.WriteString(s, m_description);
		}
	}

	public class PrivilegeEx
	{
		uint m_ID;
		string m_description;
		ushort m_duration;

		public void toBuffer(Stream s)
		{
			Helper.WriteU32(s, m_ID);
			Helper.WriteString(s, m_description);
			Helper.WriteU16(s, m_duration);
		}
	}

	public class RMCPacketResponseGetPrivileges : RMCPResponse
	{
		List<Map_U32_Privilege> privileges;

		public RMCPacketResponseGetPrivileges()
		{
			privileges = new List<Map_U32_Privilege>();
		}

		public override string PayloadToString()
		{
			return "";
		}

		public override byte[] ToBuffer()
		{
			MemoryStream m = new MemoryStream();

			Helper.WriteU32(m, (uint)privileges.Count);
			foreach (var priv in privileges)
				priv.toBuffer(m);

			return m.ToArray();
		}

		public override string ToString()
		{
			return "[RMCPacketResponseGetPrivileges]";
		}
	}
}
