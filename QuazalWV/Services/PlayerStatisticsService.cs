using QuazalWV.Attributes;
using QuazalWV.Interfaces;
using QuazalWV.DDL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuazalWV.Services
{


	[RMCService(RMCProtocol.PlayerStatsService)]
	public class PlayerStatisticsService : RMCServiceBase
	{
		[RMCMethod(1)]
		public void PostTwitterMessage(string message)
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(2)]
		public RMCResult WritePlayerStats(IEnumerable<StatisticWriteWithBoard> playerStats)
		{
			UNIMPLEMENTED();
			return Error(0);
		}

		[RMCMethod(3)]
		public void WritePlayerStatsWithFriendsComparison(IEnumerable<StatisticWriteWithBoard> playerStats, List<uint> playerPIDs)
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(4)]
		public RMCResult ReadPlayerStats(IEnumerable<StatisticData> data, List<uint> playerIds)
		{
			var playerStats = new List<ScoreListRead>();
			return Result(playerStats);
		}

		[RMCMethod(5)]
		public RMCResult ReadStatsLeaderboardByRange(int boardId, int columnId, int rankStart, int numRows, int filterId, int filterValue)
		{
			var playerStats = new List<ScoreListRead>();
			uint playersTotal = 0;

			return Result(new { a = playerStats, b = playersTotal });
		}

		[RMCMethod(8)]
		public RMCResult ReadStatsLeaderboardByPIDs(IEnumerable<LeaderboardData> dataList, List<uint> playerPIDs)
		{
			var playerStats = new List<ScoreListRead>();
			uint playersTotal = 0;

			return Result(new { a = playerStats, b = playersTotal });
		}

		[RMCMethod(10)]
		public RMCResult ReadStatsLeaderboardKeyRanks(int boardId, int columnId)
		{
			var playerStats = new List<ScoreListRead>();
			return Result(playerStats);
		}


	}
}
