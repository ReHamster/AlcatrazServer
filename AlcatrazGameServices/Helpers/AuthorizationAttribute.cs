using Alcatraz.DTO.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Security.Claims;

namespace Alcatraz.GameServices.Helpers
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class AuthorizeAttribute : Attribute, IAuthorizationFilter
	{
		bool RequireAdmin { get; set; }

		public AuthorizeAttribute(bool RequireAdmin = false)
		{
			this.RequireAdmin = RequireAdmin;
		}

		public void OnAuthorization(AuthorizationFilterContext context)
		{
			if (context.HttpContext.User == null || !context.HttpContext.User.Identities.FirstOrDefault().IsAuthenticated)
			{
				// not logged in
				context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
				return;
			}

			if (RequireAdmin && context.HttpContext.User.Claims.Any(x => x.Type == ClaimTypes.Role && x.Value == "admin"))
			{
				context.Result = new JsonResult(new { message = "Forbidden" }) { StatusCode = StatusCodes.Status403Forbidden };
			}
		}
	}
}
