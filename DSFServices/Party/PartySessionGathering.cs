using DSFServices.DDL.Models;
using QNetZ.DDL;
using System.Collections.Generic;

namespace DSFServices
{
	public static class PartySessions
	{
		public static List<PartySessionGathering> GatheringList = new List<PartySessionGathering>();
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
