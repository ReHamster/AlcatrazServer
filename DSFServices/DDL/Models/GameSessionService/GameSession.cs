using QNetZ.DDL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DSFServices.DDL.Models
{

	public enum GameSessionAttributeType
	{
		PublicSlots = 3,
		PrivateSlots = 4,
		FilledPublicSlots = 5,
		FilledPrivateSlots = 6,
		FreePublicSlots = 50,	// used internally by game
		FreePrivateSlots = 51,  // used internally by game

		// TODO: other parameters

		/*
		XBOX:
			MM_SEARCH_QUERY_ID = 0,
			MM_HOST_PARAM_MAX_PUBLIC_SLOTS = 2,
			MM_HOST_PARAM_MAX_PRIVATE_SLOTS = 3,
			MM_SESSION_FILLED_PUB_SLOTS = 4,
			MM_SESSION_FREE_PUB_SLOTS = 5,
			MM_SESSION_FILLED_PRIV_SLOTS = 8,
			MM_SESSION_FREE_PRIV_SLOTS = 9,
			MM_SESSION_GAME_MODE = 10,
			MM_SESSION_GAME_TYPE = 11, // in some place it were 25
		*/
	}

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
