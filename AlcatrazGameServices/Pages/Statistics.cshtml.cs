using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alcatraz.Context;
using Alcatraz.Context.Entities;
using DSFServices.DDL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Alcatraz.GameServices.Pages
{
    public class StatisticsModel : PageModel
    {
		public class PlayerStat
		{
			public string PlayerNickname { get; set; }
			public int XP { get; set; }
			public int MP_LEVEL { get; set; }
			public int OverallWins { get; set; }
		}


		MainDbContext _dbContext;
		public StatisticsModel(MainDbContext dbContext)
		{
			_dbContext = dbContext;
			CurPage = 0;
		}

		public IEnumerable<PlayerStat> PlayerStats { get; set; }
		public int NumRegisteredUsers { get; set; }
		public int NumPlayersOnline { get; set; }
		public int NumPages { get; set; }
		public int CurPage { get; set; }

		public string SearchStats { get; set; }

		private StatisticValueVariant GetStatisticsValue(IEnumerable<PlayerStatisticsBoardValue> boardValues, StatisticDesc desc)
		{
			var boardValue = boardValues.FirstOrDefault(brd => brd.PropertyId == desc.statInBoardId && brd.PlayerBoard.BoardId == desc.statBoard);
			if(boardValue == null)
			{
				return new StatisticValueVariant();
			}
			return JsonConvert.DeserializeObject<StatisticValueVariant>(boardValue.ValueJSON);
		}

		public void OnGet(int p = 1, string search = "")
        {
			SearchStats = search;
			CurPage = p;

			var usersRequest = _dbContext.Users.Where(x => string.IsNullOrWhiteSpace(SearchStats) ? true : x.PlayerNickName.Contains(SearchStats));

			int pageSize = 10;
			NumRegisteredUsers = _dbContext.Users.Count();
			NumPlayersOnline = QNetZ.NetworkPlayers.Players.Count;
			NumPages = (usersRequest.Count() / pageSize) + 1;

			var selectedStatistics = new string[]
			{
				"XP",
				"MP_LEVEL",
				"Overall Wins"
			};

			var statistics = SeedStatistics.AllStatisticDescriptions.Where(x => selectedStatistics.Contains(x.statName));
			var statXP = statistics.FirstOrDefault(x => x.statName == selectedStatistics[0]);
			var statMP_LEVEL = statistics.FirstOrDefault(x => x.statName == selectedStatistics[1]);
			var statOverall_Wins = statistics.FirstOrDefault(x => x.statName == selectedStatistics[2]);

			var usersPage = usersRequest.Skip((p-1) * pageSize).Take(pageSize).ToArray();

			var userIds = usersPage.Select(x => x.Id).ToArray();
			var statIds = statistics.Select(x => x.statInBoardId).ToArray();
			var boardIds = statistics.Select(x => x.statBoard).ToArray();

			var boardValues = _dbContext.PlayerStatisticBoardValues
				.Include(x => x.PlayerBoard)
				.Where(x => userIds.Contains(x.PlayerBoard.PlayerId) && statIds.Contains(x.PropertyId))
				.Where(x => boardIds.Contains(x.PlayerBoard.BoardId))
				.ToArray();

			PlayerStats = usersPage.Select(x => {
				var values = boardValues.Where(val => val.PlayerBoard.PlayerId == x.Id);

				var statXPVal = GetStatisticsValue(values, statXP);
				var statMP_LEVELVal = GetStatisticsValue(values, statMP_LEVEL);
				var statOverall_WinsVal = GetStatisticsValue(values, statOverall_Wins);

				return new PlayerStat
				{
					PlayerNickname = x.PlayerNickName,
					MP_LEVEL = statMP_LEVELVal.valueInt32,
					XP = statXPVal.valueInt32,
					OverallWins = statOverall_WinsVal.valueInt32,
				};
			});
		}
    }
}
