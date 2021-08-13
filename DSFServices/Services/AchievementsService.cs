using DSFServices.DDL.Models;
using QNetZ;
using QNetZ.Attributes;
using QNetZ.DDL;
using QNetZ.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace DSFServices.Services
{
	[RMCService(RMCProtocolId.AchievementsService)]
	public class AchievementsService : RMCServiceBase
	{
		[RMCMethod(3)]
		public RMCResult UnlockAchievements(IEnumerable<int> achievementIds)
		{
			UNIMPLEMENTED();
			return Error(0);
		}
	}
}
