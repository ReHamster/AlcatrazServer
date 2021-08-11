using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSFServices.DDL.Models
{
	public class Invitation
	{
		public uint m_idGathering { get; set; }
		public uint m_idGuest { get; set; }
		public string m_strMessage { get; set; }
	}
}
