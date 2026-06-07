using Alcatraz.Context;
using Alcatraz.DTO.Models;
using Alcatraz.GameServices.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alcatraz.GameServices.Pages.Admin
{
	public class UsersModel : PageModel
	{
		public int NumRegisteredUsers { get; set; }
		public int NumPages { get; set; }
		public int CurPage { get; set; }
		public UserModel CurrentUser { get; set; }

		public string SearchTerm { get; set; }

		public IEnumerable<UserModel> UsersOnPage { get; set; }

		MainDbContext _dbContext;
		private IUserService _userService;

		public UsersModel(MainDbContext dbContext, IUserService userService)
		{
			_dbContext = dbContext;
			_userService = userService;
			CurPage = 0;
		}

		public IActionResult OnGet(int p = 1, string search = "")
		{
			SearchTerm = search;
			CurPage = p;

			var claims = HttpContext.User.Claims;
			var uidClaim = claims.FirstOrDefault(x => x.Type == "uid");

			uint uid = 0;
			if (uidClaim != null && uint.TryParse(uidClaim.Value, out uid))
				CurrentUser = _userService.GetById(uid);

			if (CurrentUser == null)
				return RedirectToPage(PageConstants.SignInUrl);

			var usersRequest = _dbContext.Users
				.Where(x => string.IsNullOrWhiteSpace(SearchTerm) ? true : EF.Functions.Like(x.PlayerNickName, $"%{SearchTerm}%"));

			int pageSize = 10;
			NumRegisteredUsers = _dbContext.Users.Count();
			NumPages = (usersRequest.Count() / pageSize) + 1;

			var usersPage = usersRequest.OrderBy(x => x.Id).Skip((p-1) * pageSize).Take(pageSize).ToArray();

			UsersOnPage = usersPage.Select(x => new UserModel
			{
				Id = x.Id,
				PlayerNickName = x.PlayerNickName,
				Username = x.Username,
				CreatedTime = x.CreatedTime,
				LastUpdateTime = x.LastUpdateTime,
				LastPlayTime = x.LastPlayTime,
				IsAdmin = x.IsAdmin
			});
			return Page();
		}

		public async Task<IActionResult> OnPostResetPassword(uint id)
		{
			_userService.ResetPassword(id);

            return RedirectToPage(new { p = CurPage, search = SearchTerm });
		}

        public async Task<IActionResult> OnPostDelete(uint id)
        {
			_userService.Delete(id);

            return RedirectToPage(new { p = CurPage, search = SearchTerm });
        }
    }
}
