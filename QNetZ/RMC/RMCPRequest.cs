﻿using QNetZ.DDL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QNetZ
{
    public abstract class RMCPRequest
    {
        public abstract override string ToString();
        public abstract string PayloadToString();
        public abstract byte[] ToBuffer();
    }

	// Wrapper class for DDL (or ANY object)
	public class RMCPRequestDDL<T> : RMCPRequest where T : class
	{
		public RMCPRequestDDL(T data)
		{
			objectData = data;
		}
		T objectData;

		public override string PayloadToString()
		{
			return DDLSerializer.ObjectToString(objectData);
		}

		public override byte[] ToBuffer()
		{
			var m = new MemoryStream();
			DDLSerializer.WriteObject(objectData, m);

			return m.ToArray();
		}

		public override string ToString()
		{
			return $"[RMCPRequestDDL<{typeof(T).Name}>]";
		}
	}
}
