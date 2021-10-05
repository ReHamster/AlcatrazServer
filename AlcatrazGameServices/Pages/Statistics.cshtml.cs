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
		}


		MainDbContext _dbContext;
		public StatisticsModel(MainDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public IEnumerable<PlayerStat> PlayerStats { get; set; }
		public int NumRegisteredUsers { get; set; }
		public int NumPlayersOnline { get; set; }
		public int NumPages { get; set; }

		private StatisticValueVariant GetStatisticsValue(IEnumerable<PlayerStatisticsBoardValue> boardValues, StatisticDesc desc)
		{
			var boardValue = boardValues.FirstOrDefault(brd => brd.PropertyId == desc.statID && brd.PlayerBoard.BoardId == desc.statBoard);
			if(boardValue == null)
			{
				return new StatisticValueVariant();
			}
			return JsonConvert.DeserializeObject<StatisticValueVariant>(boardValue.ValueJSON);
		}

		public void OnGet(int p = 1)
        {
			int pageSize = 50;
			NumRegisteredUsers = _dbContext.Users.Count();
			NumPlayersOnline = QNetZ.NetworkPlayers.Players.Count;
			NumPages = (NumRegisteredUsers / pageSize)+1;

			var selectedStatistics = new string[]
			{
				"XP", 
			};

			var statistics = SeedStatistics.AllStatisticDescriptions.Where(x => selectedStatistics.Contains(x.statName));
			var statIds = statistics.Select(x => x.statID).ToArray();

			var usersPage = _dbContext.Users.Skip((p-1) * pageSize).Take(pageSize).ToArray();

			var userIds = usersPage.Select(x => x.Id).ToArray();

			var boardValues = _dbContext.PlayerStatisticBoardValues
				.Include(x => x.PlayerBoard)
				.Where(x => userIds.Contains(x.PlayerBoard.PlayerId) && statIds.Contains(x.PropertyId))
				.ToArray();

			var statXP = statistics.FirstOrDefault(x => x.statName == selectedStatistics[0]);

			PlayerStats = usersPage.Select(x => {
				var values = boardValues.Where(val => val.PlayerBoard.PlayerId == x.Id);

				var statXPVal = GetStatisticsValue(values, statXP);

				return new PlayerStat
				{
					PlayerNickname = x.PlayerNickName,
					XP = statXPVal.valueInt32,
				};
			});
		}
    }
}
