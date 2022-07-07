using DSFServices.DDL.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using QNetZ;
using QNetZ.Attributes;
using QNetZ.Interfaces;
using RDVServices;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DSFServices.Services
{


	[RMCService(RMCProtocolId.PlayerStatsService)]
	public class PlayerStatisticsService : RMCServiceBase
	{
		//public static List<StatisticsBoard> StatisticsBoards = new List<StatisticsBoard>();

		[RMCMethod(1)]
		public void PostTwitterMessage(string message)
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(2)]
		public RMCResult WritePlayerStats(IEnumerable<StatisticWriteWithBoard> playerStats)
		{
			var playerId = Context.Client.Info.PID;

			using (var db = DBHelper.GetDbContext())
			{
				foreach (var writeBoard in playerStats)
				{
					var playerBoard = db.PlayerStatisticBoards
						.FirstOrDefault(x => x.PlayerId == playerId && x.BoardId == writeBoard.boardId);

					if (playerBoard == null)
					{
						playerBoard = SeedStatistics.GeneratePlayerBoard(writeBoard.boardId, playerId);
						db.Add(playerBoard);
						db.SaveChanges();
					}

					// avoid Client evaluation simply by converting it into int array
					var propertyIdsOnly = writeBoard.statisticList.Select(ws => (int)ws.propertyId).ToArray();

					var properties = db.PlayerStatisticBoardValues
						.Where(x => propertyIdsOnly.Contains(x.PropertyId) && x.PlayerBoardId == playerBoard.Id)
						.ToArray();

					foreach (var writeStat in writeBoard.statisticList)
					{
						var variantJSON = properties.FirstOrDefault(x => x.PropertyId == writeStat.propertyId);

						var variant = new StatisticsBoardValue()
						{
							value = JsonConvert.DeserializeObject<StatisticValueVariant>(variantJSON.ValueJSON),
							rankingCriterionIndex = (byte)variantJSON.RankingCriterionIndex,
							scoreLostForNextSlice = JsonConvert.DeserializeObject<StatisticValueVariant>(variantJSON.ScoreLostForNextSliceJSON),
							sliceScore = JsonConvert.DeserializeObject<StatisticValueVariant>(variantJSON.SliceScoreJSON)
						};

						// Update variant value
						variant.UpdateValueWithPolicy(writeStat.value, (StatisticPolicy)writeStat.writePolicy);

						// put back the values
						variantJSON.RankingCriterionIndex = variant.rankingCriterionIndex;
						variantJSON.ValueJSON = JsonConvert.SerializeObject(variant.value);
						variantJSON.ScoreLostForNextSliceJSON = JsonConvert.SerializeObject(variant.scoreLostForNextSlice);
						variantJSON.SliceScoreJSON = JsonConvert.SerializeObject(variant.sliceScore);
					}
				}

				db.SaveChanges();
			}

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
					pname = player.Username
				};
				playerStats.Add(scoreListRead);

				using (var db = DBHelper.GetDbContext())
				{
					foreach (var statReqData in data)
					{
						var playerBoard = db.PlayerStatisticBoards
							.Include(x => x.Values)
							.FirstOrDefault(x => x.PlayerId == playerId && x.BoardId == statReqData.boardId);

						// create empty one
						if (playerBoard == null)
						{
							playerBoard = SeedStatistics.GeneratePlayerBoard(statReqData.boardId, playerId);
							db.Add(playerBoard);
							db.SaveChanges();
						}

						var readBoardValue = new StatisticReadValueByBoard()
						{
							boardId = playerBoard.BoardId,
							lastUpdate = playerBoard.LastUpdate,
							rank = playerBoard.Rank,
							score = playerBoard.Score
						};

						readBoardValue.scores = playerBoard.Values.Select(x => new StatisticReadValue()
						{
							propertyId = (byte)x.PropertyId,
							value = JsonConvert.DeserializeObject<StatisticValueVariant>(x.ValueJSON),
							rankingCriterionIndex = (byte)x.RankingCriterionIndex,
							scoreLostForNextSlice = JsonConvert.DeserializeObject<StatisticValueVariant>(x.ScoreLostForNextSliceJSON),
							sliceScore = JsonConvert.DeserializeObject<StatisticValueVariant>(x.SliceScoreJSON)
						}).ToArray();

						scoreListRead.scoresByBoard.Add(readBoardValue);
					}
				}
			}

			return Result(playerStats);
		}

		[RMCMethod(5)]
		public RMCResult ReadStatsLeaderboardByRange(int boardId, int columnId, int rankStart, int numRows, int filterId, int filterValue)
		{
			var playerStats = new List<ScoreListRead>();
			uint playersTotal = 0;

			using (var db = DBHelper.GetDbContext())
            {
				var playerBoards = db.PlayerStatisticBoards
					.Where(x => x.BoardId == boardId);

				playersTotal = (uint)playerBoards.Count();
			}

			return Result(new { a = playerStats, b = playersTotal });
		}

		[RMCMethod(8)]
		public RMCResult ReadStatsLeaderboardByPIDs(IEnumerable<LeaderboardData> dataList, List<uint> playerPIDs)
		{
			var playerStats = new List<ScoreListRead>();
			uint playersTotal = 0;

			using (var db = DBHelper.GetDbContext())
			{
				foreach(var data in dataList)
                {
					var playerBoards = db.PlayerStatisticBoards
						.Where(x => x.BoardId == data.boardId)
						.Where(x => playerPIDs.Contains(x.PlayerId));

					playersTotal += (uint)playerBoards.Count();
				}
			}

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
