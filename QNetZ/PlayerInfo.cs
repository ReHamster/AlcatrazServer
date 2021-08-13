using System.Net;

namespace QNetZ
{
	public class PlayerInfo
	{
		public QClient client;	// connection info
		public uint PID { get; set; }		// printcipal ID
		public uint RVCID { get; set; }     // rendez-vous connection ID

		public uint StationID;
		public string AccountId;
		public string Name { get; set; }
	}
}
