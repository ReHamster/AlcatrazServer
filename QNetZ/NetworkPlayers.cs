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

		static readonly List<PlayerInfo> Players = new List<PlayerInfo>();

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

			plInfo.client = connection;
			plInfo.PID = 0;
			plInfo.RVCID = RVCIDCounter++;

			Players.Add(plInfo);

			return plInfo;
		}

		public static void PurgeAllPlayers()
		{
			Players.Clear();
		}

		public static void DropPlayerInfo(PlayerInfo client)
		{
			Players.Remove(client);
		}
	}
}
