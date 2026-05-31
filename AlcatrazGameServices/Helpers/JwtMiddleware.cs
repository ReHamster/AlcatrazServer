using Alcatraz.GameServices.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;

namespace Alcatraz.GameServices.Helpers
{
	public class JwtMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly AppSettings _appSettings;

		public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
		{
			_next = next;
			_appSettings = appSettings.Value;
		}

		public async Task Invoke(HttpContext context, IUserService userService)
		{
			if (context.User == null || !context.User.Identities.FirstOrDefault().IsAuthenticated)
			{
                await _next(context);
				return;
            }

            var userIdClaim = context.User.Claims.FirstOrDefault(x => x.Type == "uid");
			if (userIdClaim == null)
			{
				await _next(context);
				return;
			}

            context.Items["User"] = userService.GetById(uint.Parse(userIdClaim.Value));
			await _next(context);
		}
	}
}
