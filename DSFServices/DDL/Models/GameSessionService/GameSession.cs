using QNetZ.DDL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DSFServices.DDL.Models
{
	public class GameSessionProperty
	{
		public uint ID { get; set; }
		public uint Value { get; set; }
	}

	public class GameSession
	{
		public GameSession()
		{
			m_attributes = new List<GameSessionProperty>();
		}
		public uint m_typeID { get; set; }
		public ICollection<GameSessionProperty> m_attributes { get; set; }
	}

	public class GameSessionKey
	{
		public uint m_typeID { get; set; }
		public uint m_sessionID { get; set; }
	}

	public class GameSessionUpdate
	{
		public GameSessionKey m_sessionKey { get; set; }
		public ICollection<GameSessionProperty> m_attributes { get; set; }
	}

	public class GameSessionSearchResult
	{
		public GameSessionSearchResult()
		{
			m_sessionKey = new GameSessionKey();
			m_hostURLs = new List<StationURL>();
			m_attributes = new List<GameSessionProperty>();
		}

		public GameSessionKey m_sessionKey { get; set; }
		public uint m_hostPID { get; set; }
		public ICollection<StationURL> m_hostURLs { get; set; }
		public ICollection<GameSessionProperty> m_attributes { get; set; }
	}
}
