using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuazalWV.DDL.Models
{
	public class RegisterResult
	{
		public uint retval { get; set; }
		public uint pidConnectionID { get; set; }
		public string urlPublic { get; set; }
	}
}