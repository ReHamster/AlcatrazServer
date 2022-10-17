using Alcatraz.DTO.Models;
using Alcatraz.GameServices.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.IO;
using System;
using DSFServices.DDL.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;
using Microsoft.Extensions.Configuration;
using QNetZ;
using Alcatraz.GameServices.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;

namespace Alcatraz.GameServices.Pages.Account
{
	[BindProperties(SupportsGet = true)]
	public class ManageModel : PageModel
    {
        public string CurrentUserName { get; set; }
        public UserModel EditModel { get; set; }

        UserModel CurrentUser { get; set; }
        public string ErrorMessage { get; set; }
		public string PasswordErrorMessage { get; set; }

		public string NewPassword { get; set; }
		public string NewPasswordRetype { get; set; }

		private IUserService _userService;

		public ManageModel(IUserService userService)
        {
            _userService = userService;
		}

		public IActionResult OnGet(bool getConfig = false)
        {
			var claims = HttpContext.User.Claims;
			var uidClaim = claims.FirstOrDefault(x => x.Type == "uid");

			uint uid = 0;
			if (uint.TryParse(uidClaim.Value, out uid))
				CurrentUser = _userService.GetById(uid);

			EditModel = new UserModel()
            {
                Id = CurrentUser.Id,
                PlayerNickName = CurrentUser.PlayerNickName,
                Username = CurrentUser.Username
			};

			CurrentUserName = HttpContext.User.Identity.Name;

			return Page();
		}

        public async Task<IActionResult> OnPost()
        {
			{
				var claims = HttpContext.User.Claims;
				var uidClaim = claims.FirstOrDefault(x => x.Type == "uid");

				uint uid = 0;
				if (uint.TryParse(uidClaim.Value, out uid))
					CurrentUser = _userService.GetById(uid);
			}

			if(CurrentUser == null)
			{
				ErrorMessage = "Session outdated?";
				CurrentUserName = HttpContext.User.Identity.Name;
				return Page();
			}

			if (!ModelState.IsValid)
			{
				ErrorMessage = "Username or Password is invalid";
				CurrentUserName = HttpContext.User.Identity.Name;
				return Page();
			}

			EditModel.Id = CurrentUser.Id;
			var result = _userService.Update(EditModel);
			if (result.Success)
			{
				if (!string.IsNullOrEmpty(NewPassword))
				{
					if (NewPasswordRetype != NewPassword)
					{
						PasswordErrorMessage = "Password mismatch";
						return Page();
					}

					_userService.ChangePassword(CurrentUser.Id, NewPassword);
				}
				PasswordErrorMessage = "";

				// authenticate again
				var user = _userService.GetById(CurrentUser.Id);

				if (user != null)
				{
					var claims = _userService.GetUserClaims(user);
					var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
					var principal = new ClaimsPrincipal(identity);

					await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
				}

				CurrentUserName = HttpContext.User.Identity.Name;

				return Page();
			}

			ErrorMessage = result.ErrorMessage;
			CurrentUserName = HttpContext.User.Identity.Name;

			return Page();
		}
	}
}
