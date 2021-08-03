using System.Collections.Generic;
using System.IO;

namespace QuazalWV
{
	public class PersistentInfo // StorageFile
	{
		public string m_name;
		public uint m_size;

		public void toBuffer(Stream s)
		{
			Helper.WriteString(s, m_name);
			Helper.WriteU32(s, m_size);
		}
	}

	public class RMCPacketResponseGetFileList : RMCPResponse
	{
		public List<PersistentInfo> fileList;

		public RMCPacketResponseGetFileList()
		{
			fileList = new List<PersistentInfo>();
		}

		public override string PayloadToString()
		{
			return "";
		}

		public override byte[] ToBuffer()
		{
			MemoryStream s = new MemoryStream();

			Helper.WriteU32(s, (uint)fileList.Count);
			foreach (var file in fileList)
				file.toBuffer(s);

			return s.ToArray();
		}

		public override string ToString()
		{
			return "[RMCPacketResponseGetFileList]";
		}
	}
}
