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


	[RMCService(RMCProtocolId.PlayerStatsService)]
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
			// return 63 01 00 00 6C 01 44 00 00 00 04 80 00 00 01 00 00 00 04 45 08 00 0B 00 4A 65 6C 6C 79 73 6F 61 70 79 00 01 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 04 00 00 00 01 72 BE 0B 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 01 02 8F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 01 03 D3 00 3E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 01 04 37 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 01 CF 4B 0D 96 1F 00 00 00 
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
