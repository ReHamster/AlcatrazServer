using QuazalWV.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuazalWV
{
	class RMCPResponseDDL<T> : RMCPResponse where T: class
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
			DDLHelper.WriteObject(objectData, m);

			return m.ToArray();
		}

		public override string ToString()
		{
			return $"[RMCPResponseDDL<{typeof(T).Name}>";
		}
	}
}
