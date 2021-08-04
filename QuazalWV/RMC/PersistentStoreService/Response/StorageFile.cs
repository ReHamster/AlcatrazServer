using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuazalWV
{
	public class StorageFile
	{
		public byte[] m_buffer { get; set; }
		public uint retcode { get; set; }
		public StorageFile()
		{
			m_buffer = new byte []{ };
			retcode = 0;
		}
	}
}
