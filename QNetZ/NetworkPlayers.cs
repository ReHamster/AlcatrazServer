using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;

namespace QNetZ
{
	public static class NetworkPlayers
	{
		public static uint RVCIDCounter = 0xBB98E;

		public static readonly List<PlayerInfo> Players = new List<PlayerInfo>();

		public static PlayerInfo GetPlayerInfoByPID(uint pid)
		{
			foreach (PlayerInfo pl in Players)
			{
				if (pl.PID == pid)
					return pl;
			}
			return null;
		}

		public static PlayerInfo GetPlayerInfoByUsername(string userName)
		{
			foreach (PlayerInfo pl in Players)
			{
				if (pl.Name == userName)
					return pl;
			}
			return null;
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

		public static void DropPlayerInfo(PlayerInfo plInfo)
		{
			QLog.WriteLine(1, $"dropping player: {plInfo.Name}");
			Players.Remove(plInfo);
		}

		public static void DropPlayers()
		{
			for (var i = 0; i < Players.Count; i++)
			{
				var plInfo = Players[i];
				if (plInfo.Client.State == QClient.StateType.Dropped &&
					(DateTime.UtcNow - plInfo.Client.LastPacketTime).TotalSeconds > Constants.ClientTimeoutSeconds)
				{
					QLog.WriteLine(1, $"dropping player: {plInfo.Name}");

					// also drop network players
					Players.RemoveAt(i);
					i--;
				}
			}
		}
	}
}
