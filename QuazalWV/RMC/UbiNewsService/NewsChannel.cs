using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuazalWV
{
	class NewsChannel
	{
		public NewsChannel()
		{

		}

		public uint m_ID { get; set; }
		public uint m_ownerPID { get; set; }
		public string m_name { get; set; }
		public string m_description { get; set; }
		public DateTime m_creationTime { get; set; }
		public DateTime m_expirationTime { get; set; }
		public string m_type { get; set; }
		public string m_locale { get; set; }
		public bool m_subscribable { get; set; }
	}
}
