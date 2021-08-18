using DSFServices.DDL.Models;
using QNetZ;
using QNetZ.DDL;
using System.Collections.Generic;
using System.Linq;

namespace DSFServices
{
	public static class GameSessions
	{
		public static readonly List<GameSessionData> SessionList = new List<GameSessionData>();

		public static void UpdateSessionParticipation(PlayerInfo player, uint newSessionId, uint newSessionTypeId, bool isPrivate)
		{
			var oldSessionId = player.GameData().CurrentSessionID;
			var oldSessionTypeId = player.GameData().CurrentSessionTypeID;

			if (oldSessionId == newSessionId)
				return;

			// remove participation from old session
			var oldSession = SessionList.FirstOrDefault(x => x.Id == oldSessionId && x.TypeID == oldSessionTypeId);
			if (oldSession != null)
			{
				oldSession.PublicParticipants.Remove(player.PID);
				oldSession.Participants.Remove(player.PID);

				oldSession.Attributes[(uint)GameSessionAttributeType.FilledPublicSlots] = (uint)oldSession.PublicParticipants.Count;
				oldSession.Attributes[(uint)GameSessionAttributeType.FilledPrivateSlots] = (uint)oldSession.Participants.Count;

				if (oldSession.PublicParticipants.Count == 0 && oldSession.Participants.Count == 0)
				{
					QLog.WriteLine(1, $"Auto-deleted session {oldSessionId}");
					SessionList.Remove(oldSession);
				}
			}

			// set new participation
			player.GameData().CurrentSessionID = newSessionId;
			player.GameData().CurrentSessionTypeID = newSessionTypeId;

			var newSession = SessionList.FirstOrDefault(x => x.Id == newSessionId && x.TypeID == newSessionTypeId);
			if (newSession != null)
			{
				if(isPrivate)
					newSession.Participants.Add(player.PID);
				else
					newSession.PublicParticipants.Add(player.PID);

				newSession.Attributes[(uint)GameSessionAttributeType.FilledPublicSlots] = (uint)newSession.PublicParticipants.Count;
				newSession.Attributes[(uint)GameSessionAttributeType.FilledPrivateSlots] = (uint)newSession.Participants.Count;
			}
		}
	}

	public class GameSessionData
	{
		public GameSessionData()
		{
			Attributes = new Dictionary<uint, uint>();
			HostURLs = new List<StationURL>();
			Participants = new HashSet<uint>();
			PublicParticipants = new HashSet<uint>();
		}

		public uint Id { get; set; }

		public uint TypeID { get; set; }
		public Dictionary<uint, uint> Attributes { get; set; }
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
