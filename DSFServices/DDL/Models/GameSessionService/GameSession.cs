using QNetZ.DDL;
using System;
using System.Collections.Generic;

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

	//-----------------------------------------------------
	// TODO: entities

	public class GameSessionData
	{
		public GameSessionData()
		{
			Session = new GameSession();
			HostURLs = new List<StationURL>();
			Participants = new HashSet<uint>();
			PublicParticipants = new HashSet<uint>();
		}

		public uint Id { get; set; }

		public GameSession Session { get; set; }
		public uint HostPID { get; set; }
		public ICollection<StationURL> HostURLs { get; set; }
		public HashSet<uint> Participants { get; set; }     // ID, Private
		public HashSet<uint> PublicParticipants { get; set; }     // ID, Public
	}
}
