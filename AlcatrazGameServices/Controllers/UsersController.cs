using Alcatraz.GameServices.Helpers;
using Alcatraz.GameServices.Services;
using Alcatraz.DTO.Models;

using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Alcatraz.GameServices.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class UsersController : ControllerBase
	{
		private IUserService _userService;

		public UsersController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpPost("Authenticate")]
		public IActionResult Authenticate(AuthenticateRequest model)
		{
			var response = _userService.Authenticate(model);

			if (response == null)
				return BadRequest(new ErrorModel { Message = "Username or password is incorrect" });

			return Ok(response);
		}

		[HttpPost("Register")]
		public IActionResult Register(UserRegisterModel model)
		{
			var response = _userService.Register(model);

			if (response == 0)
				return BadRequest(new ErrorModel { Message = "Unable to register user" });

			return Ok(response);
		}

		[Authorize]
		[HttpGet]
		public IActionResult GetAll()
		{
			var users = _userService.GetAll().Select(x => new UserModel
			{
				Id = x.Id,
				PlayerNickName = x.PlayerNickName,
				Username = x.Username
			});

			return Ok(users);
		}
	}
}
