using System.Collections.Generic;
using System.Linq;

namespace QNetZ
{
	public static class NetworkPlayers
	{
		public static uint RVCIDCounter = 0xBB98E;

		public static readonly List<PlayerInfo> Players = new List<PlayerInfo>();

		public static PlayerInfo GetPlayerInfoByPID(uint pid)
		{
			return Players.SingleOrDefault(pl => pl.PID == pid);
		}

		public static PlayerInfo GetPlayerInfoByUsername(string userName)
		{
			return Players.SingleOrDefault(pl => pl.Name == userName);
		}

		public static PlayerInfo CreatePlayerInfo(QClient connection)
		{
			var plInfo = new PlayerInfo();

			plInfo.Client = connection;
			plInfo.PID = 0;
			plInfo.RVCID = RVCIDCounter++;

			Players.Add(plInfo);

			return plInfo;
		}

		public static void PurgeAllPlayers()
		{
			Players.Clear();
		}

		public static void DropPlayerInfo(PlayerInfo playerInfo)
		{
			QLog.WriteLine(1, $"dropping player: {playerInfo.Name}");

			if (playerInfo.Client != null)
				playerInfo.Client.PlayerInfo = null;

			playerInfo.OnDropped();
			Players.Remove(playerInfo);
		}

		public static void DropPlayers()
		{
			Players.RemoveAll(playerInfo => {
				if (playerInfo.Client.State != QClient.StateType.Dropped)
					return false;

				if (playerInfo.Client.TimeSinceLastPacket < Constants.ClientTimeoutSeconds)
					return false;

				QLog.WriteLine(1, $"Auto-Dropping player: {playerInfo.Name}");

				if (playerInfo.Client != null)
					playerInfo.Client.PlayerInfo = null;

				playerInfo.OnDropped();
				return true;
			});
		}
	}
}
