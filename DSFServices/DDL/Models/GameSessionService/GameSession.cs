using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSFServices.DDL.Models
{
	public class GameSessionProperty
	{
		public uint ID { get; set; }
		public uint Value { get; set; }
	}

	public class GameSession
	{
		public uint m_typeID { get; set; }
		public IEnumerable<GameSessionProperty> m_attributes { get; set; }
	}

	public class GameSessionKey
	{
		public uint m_typeID { get; set; }
		public uint m_sessionID { get; set; }
	}

	public class GameSessionUpdate
	{
		public GameSessionKey m_sessionKey { get; set; }
		public IEnumerable<GameSessionProperty> m_attributes { get; set; }
	}
}
