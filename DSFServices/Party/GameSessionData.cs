using DSFServices.DDL.Models;
using QNetZ.DDL;
using System.Collections.Generic;
using System.Linq;

namespace DSFServices
{
	public static class GameSessions
	{
		public static readonly List<GameSessionData> SessionList = new List<GameSessionData>();
	}

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
		public List<StationURL> HostURLs { get; set; }
		public HashSet<uint> Participants { get; set; }     // ID, Private
		public HashSet<uint> PublicParticipants { get; set; }     // ID, Public

		public uint[] AllParticipants
		{
			get
			{
				return Participants.Concat(PublicParticipants).ToArray();
			}
		}
	}
}
