using Alcatraz.Context;
using Alcatraz.GameServices.Helpers;
using Alcatraz.GameServices.Services;
using DSFServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using QNetZ;

namespace Alcatraz.GameServices
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			// configure strongly typed settings object
			services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
			services.Configure<QConfiguration>(Configuration.GetSection("Services"));

			var secOpts = Configuration.GetSection("Services").Get<QConfiguration>();

			services.AddCors();
			services.AddControllers();

			services.AddRazorPages();

			services.AddSwaggerGen();

			// register user service
			services.AddScoped<IUserService, UserService>();

			// add servers
			services.AddSingleton<IHostedService, BackendServicesServer>();
			services.AddSingleton<IHostedService, RendezVousServer>();

			services.AddDbContext<MainDbContext>(opt =>
			{
				MainDbContext.OnContextBuilding(opt, (DBType)secOpts.DbType, secOpts.DbConnectionString);
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, MainDbContext dbContext)
		{
			// update database if haven't
			dbContext.Database.Migrate();

			// global cors policy
			app.UseCors(x => x
				.AllowAnyOrigin()
				.AllowAnyMethod()
				.AllowAnyHeader());

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			// Enable middleware to serve generated Swagger as a JSON endpoint.
			app.UseSwagger();

			// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
			// specifying the Swagger JSON endpoint.
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
			});


			app.UseRouting();

			app.UseAuthentication();

			app.UseAuthorization();

			// custom jwt auth middleware
			app.UseMiddleware<JwtMiddleware>();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
				endpoints.MapRazorPages();
			});
		}
	}
}
