using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSFServices.DDL.Models
{
	public class UplayActionPlatform
	{
		public string m_platformCode { get; set; }
		public bool m_completed { get; set; }
		public string m_specificKey { get; set; }
	}

	public class UplayAction
	{
		public UplayAction()
		{
			m_platforms = new List<UplayActionPlatform>();
		}

		public string m_code { get; set; }
		public string m_name { get; set; }
		public string m_description { get; set; }
		public ushort m_value { get; set; }
		public string m_gameCode { get; set; }

		public IEnumerable<UplayActionPlatform> m_platforms { get; set; }
	}
}
