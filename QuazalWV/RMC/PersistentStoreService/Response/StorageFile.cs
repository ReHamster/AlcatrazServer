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
		public byte[] buffer { get; set; }
		public bool result { get; set; }

		public StorageFile()
		{
			buffer = new byte []{ };
			result = true;
		}
	}
}
