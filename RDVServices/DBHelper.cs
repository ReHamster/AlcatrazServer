using Alcatraz.Context;
using Alcatraz.Context.Entities;
using Microsoft.EntityFrameworkCore;
using QNetZ;
using System.Linq;

namespace RDVServices
{
	public static class DBHelper
	{
		public static MainDbContext GetDbContext()
		{
			var contextOptions = new DbContextOptionsBuilder<MainDbContext>();
			var opts = MainDbContext.OnContextBuilding(contextOptions, (DBType)QConfiguration.Instance.DbType, QConfiguration.Instance.DbConnectionString);

			var retCtx = new MainDbContext(opts.Options);

			retCtx.Database.Migrate();

			return retCtx;
		}

		public static User GetUserByName(string name)
		{
			using(var context = GetDbContext())
			{
				return context.Users
					.AsNoTracking()
					.SingleOrDefault(x => x.PlayerNickName == name);
			}
		}

		public static User GetUserByPID(uint PID)
		{
			using (var context = GetDbContext())
			{
				return context.Users
					.AsNoTracking()
					.SingleOrDefault(x => x.Id == PID);
			}
		}
	}
}
