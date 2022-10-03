using Alcatraz.DTO.Models;
using Alcatraz.GameServices.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Alcatraz.GameServices.Pages.Account
{
    public class SignInModel : PageModel
    {
		[BindProperty]
		public AuthenticateRequest AuthModel { get; set; }

		public string ErrorMessage;

		IUserService _userService;

		[FromQuery]
		public string ReturnUrl { get; set; }

		public SignInModel(IUserService userService)
		{
			_userService = userService;
		}

		public void OnGet()
		{
			AuthModel = new AuthenticateRequest();
		}

		public async Task<IActionResult> OnPost()
		{
			if (!ModelState.IsValid)
			{
				ErrorMessage = "Username or Password is invalid";
				return Page();
			}

			var response = _userService.Authenticate(AuthModel);
			if (response != null)
			{
				var user = _userService.GetById(response.Id);

				if (user != null)
				{
					var claims = _userService.GetUserClaims(user);
					var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
					var principal = new ClaimsPrincipal(identity);

					await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
				}
				else
					ErrorMessage = "Username or Password is invalid";

				return RedirectToPage(ReturnUrl ?? "/Account/Manage");
			}

			ErrorMessage = "Username or Password is invalid";

			return Page();
		}
	}
}
