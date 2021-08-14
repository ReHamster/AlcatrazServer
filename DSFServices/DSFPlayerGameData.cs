using DSFServices.DDL.Models;
using QNetZ;

namespace DSFServices
{
	public class DSFPlayerGameData
	{
		public DSFPlayerGameData()
		{
			CurrentGatheringId = uint.MaxValue;
			CurrentSessionTypeID = uint.MaxValue;
			CurrentSessionID = uint.MaxValue;
		}

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
