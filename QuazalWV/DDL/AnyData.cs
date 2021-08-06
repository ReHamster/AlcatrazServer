using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuazalWV.DDL
{
	public interface IAnyData
	{
		void Read(Stream s);
		void Write(Stream s);
	}

	public class AnyData<T> : IAnyData where T: class
	{
		public AnyData()
		{
			className = typeof(T).Name; // that's for writing
		}

		public string className;
		public T data;

		// FIXME: maybe instead we construct object from buffer? But it's pain in ass. This AnyData is pain in ass

		public void Read(Stream s)
		{
			className = Helper.ReadString(s);
			uint thisSize = Helper.ReadU32(s);

			// not this data - skip
			if (className != typeof(T).Name)
			{
				s.Seek(thisSize, SeekOrigin.Current);
				return;
			}

			thisSize = Helper.ReadU32(s);
			long curPos = s.Position;
			data = DDLSerializer.ReadObject<T>(s);

			if ((s.Position - curPos) != thisSize)
			{
				throw new Exception($"AnyData<{typeof(T).Name}> reading error - data size mismatch");
			}
		}

		public void Write(Stream s)
		{
			Helper.WriteString(s, className);

			var m = new MemoryStream();
			DDLSerializer.WriteObject(data, m);

			uint size = (uint)m.Position;

			// write size into memory buffer and data
			Helper.WriteU32(s, size + sizeof(int));
			Helper.WriteU32(s, size);
			s.Write(m.GetBuffer(), 0, (int)size);
		}
	}
}
