using Alcatraz.Context;
using Alcatraz.Context.Entities;
using Alcatraz.DTO.Models;
using Alcatraz.GameServices.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Alcatraz.GameServices.Pages.Account
{
	public class UserRegisterExtModel : UserRegisterModel
	{
		public string PasswordRetype { get; set; }
	}

	[BindProperties(SupportsGet = true)]
	public class SignUpModel : PageModel
    {
		[BindProperty]
		public UserRegisterExtModel RegisterModel { get; set; }

		public string ErrorMessage;

		IUserService _userService;

		public SignUpModel(IUserService userService)
		{
            _userService = userService;
		}

		public void OnGet()
		{
			RegisterModel = new UserRegisterExtModel();
		}

        public async Task<IActionResult> OnPost()
        {
			if (!ModelState.IsValid)
			{
				ErrorMessage = "Username or Password is invalid";
				return Page();
			}

			if(RegisterModel.PasswordRetype != RegisterModel.Password)
			{
				ErrorMessage = "Password mismatch";
				return Page();
			}

			var result = _userService.Register(RegisterModel);
            if(result.Success)
			{
				var user = _userService.GetById(result.Id);

				if(user != null)
				{
					var claims = _userService.GetUserClaims(user);
					var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
					var principal = new ClaimsPrincipal(identity);

					await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

					return RedirectToPage("/Account/Manage");
				}
			}

            ErrorMessage = result.ErrorMessage;

			return Page();
		}
    }
}
