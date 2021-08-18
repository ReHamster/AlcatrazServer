using DSFServices.DDL.Models;
using QNetZ;
using QNetZ.DDL;
using System.Collections.Generic;
using System.Linq;

namespace DSFServices
{
	public static class PartySessions
	{
		public static List<PartySessionGathering> GatheringList = new List<PartySessionGathering>();

		public static void UpdateGatheringParticipation(PlayerInfo player, uint newGatheringId)
		{
			var oldGatheringId = player.GameData().CurrentGatheringId;

			if (oldGatheringId == newGatheringId)
				return;

			// remove participation from old gathering
			var oldGathering = GatheringList.FirstOrDefault(x => x.Session.m_idMyself == oldGatheringId);
			if (oldGathering != null)
			{
				oldGathering.Participants.Remove(player.PID);

				if (oldGathering.Participants.Count == 0)
				{
					QLog.WriteLine(1, $"Auto-deleted gathering {oldGatheringId}");
					GatheringList.Remove(oldGathering);
				}
			}

			// set new participation
			player.GameData().CurrentGatheringId = newGatheringId;

			var newGathering = GatheringList.FirstOrDefault(x => x.Session.m_idMyself == newGatheringId);
			if (newGathering != null)
			{
				newGathering.Participants.Add(player.PID);
			}
		}
	}

	public class PartySessionGathering
	{
		public PartySessionGathering()
		{
			Session = new HermesPartySession();
		}

		public PartySessionGathering(HermesPartySession session)
		{
			Session = session;
			Urls = new List<StationURL>();
			Participants = new HashSet<uint>();
		}

		public HermesPartySession Session { get; set; }
		public List<StationURL> Urls { get; set; } // host and player URLs
		public HashSet<uint> Participants { get; set; }
	}
}
