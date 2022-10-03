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

		IUserService _userService;

		private readonly IOptions<QConfiguration> _configuration;

		public ManageModel(IUserService userService, IOptions<QConfiguration> serverConfig)
        {
            _userService = userService;
			_configuration = serverConfig;
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

			if(getConfig)
			{
				var settings = new JsonSerializerSettings
				{
					Formatting = Formatting.Indented,
					ContractResolver = new DefaultContractResolver(),
					DefaultValueHandling = DefaultValueHandling.Include,
					TypeNameHandling = TypeNameHandling.None,
					NullValueHandling = NullValueHandling.Ignore,
					ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
				};

				var serviceUrl = "alcatraz.drivermadness.net";
				var accessKey = _configuration.Value.SandboxAccessKey;
				var configKey = "23ad683803a0457cabce83f905811dbc";

				var profileConfig = new ProfileConfig
				{
					Username = CurrentUser.Username,
					AccountId = CurrentUser.PlayerNickName,
					Password = uidClaim.Value,

					ServiceUrl = serviceUrl,
					AccessKey = accessKey,
					ConfigKey = configKey
				};

				var config = new AlcatrazClientConfig();
				config.UseProfile = "Alcatraz";
				config.Profiles.Add("Alcatraz", profileConfig);

				string alcatrazConfig = JsonConvert.SerializeObject(config, settings);
				byte[] bytes = Encoding.ASCII.GetBytes(alcatrazConfig);

				return File(bytes, "application/json", "Alcatraz.json");
			}

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
