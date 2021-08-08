using QuazalWV.Attributes;
using QuazalWV.Interfaces;
using QuazalWV.DDL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using QuazalWV.DDL;

namespace QuazalWV.Services
{


	[RMCService(RMCProtocolId.PlayerStatsService)]
	public class PlayerStatisticsService : RMCServiceBase
	{
		public static List<StatisticsBoard> StatisticsBoards = new List<StatisticsBoard>();

		[RMCMethod(1)]
		public void PostTwitterMessage(string message)
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(2)]
		public RMCResult WritePlayerStats(IEnumerable<StatisticWriteWithBoard> playerStats)
		{
			var playerId = Context.Client.info.PID;

			foreach(var board in playerStats)
			{
				var playerBoard = StatisticsBoards.Find(x => x.playerId == playerId && x.boardId == board.boardId);

				if (playerBoard == null)
				{
					playerBoard = SeedStatistics.GeneratePlayerBoard(board.boardId, playerId);
					StatisticsBoards.Add(playerBoard);
				}

				foreach (var stat in board.statisticList)
				{
					StatisticsBoardValue variant;

					if(playerBoard.properties.TryGetValue(stat.propertyId, out variant))
					{
						// TODO: utilize stat.friendComparison

						variant.UpdateValueWithPolicy(stat.value, (StatisticPolicy)stat.writePolicy);
					}
					else
						playerBoard.properties[stat.propertyId] = new StatisticsBoardValue(stat.value);
				}
			}

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
#if false
			var statsData = new[] {
				"01 00 00 00 04 45 08 00 0B 00 4A 65 6C 6C 79 73 6F 61 70 79 00 01 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 04 00 00 00 01 72 BE 0B 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 01 02 8F 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 01 03 D3 00 3E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 01 04 37 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 01 CF 4B 0D 96 1F 00 00 00",
				"01 00 00 00 04 45 08 00 0B 00 4A 65 6C 6C 79 73 6F 61 70 79 00 01 00 00 00 0C 00 00 00 00 00 00 00 00 00 00 00 04 00 00 00 01 72 BE 0B 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 01 02 16 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 01 03 AC 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 01 04 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 80 41 5F C0 3F 01 00 00 04 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 04 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 04 73 3A 03 96 1F 00 00 "
			};

			for(var i = 0; i < statsData.Length; i++)
			{
				var m = new MemoryStream(Helper.ParseByteArray(statsData[i]));
				var retModel = DDLSerializer.ReadObject<List<ScoreListRead>>(m);

				if(data.First().boardId == retModel.First().scoresByBoard.First().boardId)
				{
					Log.WriteLine(1, "Statistics request:");
					Log.WriteLine(1, DDLSerializer.ObjectToString(data));
					Log.WriteLine(1, "Statistics result:");
					Log.WriteLine(1, DDLSerializer.ObjectToString(retModel));

					return Result(retModel);
				}
			}

			UNIMPLEMENTED();

			var playerStats = new List<ScoreListRead>();
			return Result(playerStats);
#endif
			var playerStats = new List<ScoreListRead>();

			foreach (var playerId in playerIds)
			{
				var player = DBHelper.GetUserByPID(playerId);

				if (player == null)
					continue;

				var scoreListRead = new ScoreListRead()
				{
					pid = playerId,
					pname = player.name
				};
				playerStats.Add(scoreListRead);

				foreach (var statReqData in data)
				{
					var playerBoard = StatisticsBoards.Find(x => x.playerId == playerId && x.boardId == statReqData.boardId);

					// create empty one
					if(playerBoard == null )
					{
						playerBoard = SeedStatistics.GeneratePlayerBoard(statReqData.boardId, playerId);
						StatisticsBoards.Add(playerBoard);
					}

					var readBoardValue = new StatisticReadValueByBoard()
					{
						boardId = playerBoard.boardId,
						lastUpdate = playerBoard.lastUpdate,
						rank = playerBoard.rank,
						score = playerBoard.score
					};

					readBoardValue.scores = playerBoard.properties
						.Where(kv => statReqData.propertyIds.Contains(kv.Key))
						.Select(kv => new StatisticReadValue((byte)kv.Key, kv.Value)).ToArray();

					scoreListRead.scoresByBoard.Add(readBoardValue);
				}
			}

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
