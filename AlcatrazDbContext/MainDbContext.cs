using Alcatraz.Context.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Alcatraz.Context
{
	public enum DBType
	{
		SQLite = 0,
		MySQL = 1,
	}

	// TO run migrations:
	// Add-Migration NAME -Project AlcatrazDbContext -StartupProject AlcatrazGameServices -Context MainDbContext

	public class MainDbContext : DbContext
	{
		public static DbContextOptionsBuilder OnContextBuilding(DbContextOptionsBuilder opt, DBType type, string connectionString)
		{
			if(type == DBType.SQLite)
			{
				return opt.UseSqlite(connectionString);
			}

			if (type == DBType.MySQL)
			{
				var serverVersion = new MySqlServerVersion(new Version(8, 0, 25));
				return opt.UseMySql(connectionString, serverVersion, conf => conf.CommandTimeout(60));
			}

			return opt;
		}
		public MainDbContext()
			: base()
		{
		}

		public MainDbContext(DbContextOptions options)
			: base(options)
		{
		}

		public async Task EnsureSeedData()
		{
		
		}

		//------------------------------------------------------------------------------------------
		// Model relations comes here

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.Entity<Relationship>()
					.HasKey(t => new { t.User1Id, t.User2Id });

			base.OnModelCreating(builder);
		}

		//------------------------------------------------------------------------------------------
		// Database tables itself

		// USERS
		public DbSet<User> Users { get; set; }
		public DbSet<Relationship> UserRelationships { get; set; }
	}
}
