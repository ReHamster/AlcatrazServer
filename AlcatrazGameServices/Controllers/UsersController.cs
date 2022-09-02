using Alcatraz.GameServices.Helpers;
using Alcatraz.GameServices.Services;
using Alcatraz.DTO.Models;

using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

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
		public IActionResult Authenticate([FromBody] AuthenticateRequest model)
		{
			var response = _userService.Authenticate(model);

			if (response == null)
			{
				return Unauthorized(new ResultModel("Username or password is incorrect"));
			}

			return Ok(response);
		}

		[HttpPost("Register")]
		public IActionResult Register([FromBody] UserRegisterModel model)
		{
			var result = _userService.Register(model);

			if (!result.Success)
				return BadRequest(result);

			return Ok(result.Id);
		}

		[Authorize]
		[HttpPost("UpdateUser")]
		public IActionResult UpdateUser([FromBody] UserModel model)
		{
			var user = (UserModel)HttpContext.Items["User"];

			if (user == null)
				return Unauthorized(new ResultModel("Unable to update user"));

			model.Id = user.Id;

			var result = _userService.Update(model);

			if (!result.Success)
				return BadRequest(result);

			// pasword change successful so generate jwt token
			var token = _userService.GenerateJwtToken(user);

			return Ok(new AuthenticateResponse(user, token));
		}

		[Authorize]
		[HttpPost("ChangePassword")]
		public IActionResult ChangePassword(ChangePasswordRequest model)
		{
			var user = (UserModel)HttpContext.Items["User"];

			if (user == null)
				return Unauthorized(new ResultModel("Unable to change user password"));

			var result = _userService.ChangePassword(user.Id, model.NewPassword);

			if (!result.Success)
				return BadRequest(result);

			// pasword change successful so generate jwt token
			var token = _userService.GenerateJwtToken(user);

			return Ok(new AuthenticateResponse(user, token));
		}

		[Authorize]
		[HttpGet]
		public IActionResult GetAll()
		{
			var users = _userService.GetAll();

			return Ok(users);
		}
	}
}
