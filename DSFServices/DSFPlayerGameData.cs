using DSFServices.DDL.Models;
using QNetZ;
using System.Linq;

namespace DSFServices
{
	public class DSFPlayerGameData : IPlayerDataStore
	{
		public DSFPlayerGameData(PlayerInfo owner)
		{
			Owner = owner;

			CurrentGatheringId = uint.MaxValue;
			CurrentSessionTypeID = uint.MaxValue;
			CurrentSessionID = uint.MaxValue;
		}

		// when player dropped, game data will be destroyed
		// so we need to remove player from game session and gatherings

		public void OnDropped()
		{
			var gathering = PartySessions.GatheringList.FirstOrDefault(x => x.Session.m_idMyself == CurrentGatheringId);

			if (gathering != null)
			{
				gathering.Participants.Remove(Owner.PID);

				if (gathering.Participants.Count == 0)
				{
					QLog.WriteLine(1, $"Auto-deleted gathering {CurrentGatheringId}");
					PartySessions.GatheringList.Remove(gathering);
				}
			}
			var session = GameSessions.SessionList
				.FirstOrDefault(x => x.Id == CurrentSessionID &&
									 x.TypeID == CurrentSessionTypeID);

			if (session != null)
			{
				session.PublicParticipants.Remove(Owner.PID);
				session.Participants.Remove(Owner.PID);

				if (session.PublicParticipants.Count == 0 && session.Participants.Count == 0)
				{
					QLog.WriteLine(1, $"Auto-deleted session {CurrentSessionID}");
					GameSessions.SessionList.Remove(session);
				}
			}
		}

		public readonly PlayerInfo Owner;

		public uint CurrentGatheringId { get; set; }
		public uint CurrentSessionTypeID { get; set; }
		public uint CurrentSessionID { get; set; }
	}

	public static class DSFPlayerInfoExtensions
	{
		public static DSFPlayerGameData GameData(this PlayerInfo plInfo)
		{
			return plInfo.GetData<DSFPlayerGameData>();
		}
	}
}
