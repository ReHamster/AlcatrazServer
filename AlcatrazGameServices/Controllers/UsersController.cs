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
				return Unauthorized(new ErrorModel { Message = "Username or password is incorrect" });
			}

			return Ok(response);
		}

		[HttpPost("Register")]
		public IActionResult Register([FromBody] UserRegisterModel model)
		{
			var response = _userService.Register(model);

			if (response == 0)
				return BadRequest(new ErrorModel { Message = "Unable to register user" });

			return Ok(response);
		}

		[Authorize]
		[HttpPost("UpdateUser")]
		public IActionResult UpdateUser([FromBody] UserModel model)
		{
			var user = (UserModel)HttpContext.Items["User"];

			if (user == null)
				return Unauthorized(new ErrorModel { Message = "Unable to update user" });

			model.Id = user.Id;

			var response = _userService.Update(model);

			if (!response)
				return BadRequest(new ErrorModel { Message = "Unable to update user" });

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
				return Unauthorized(new ErrorModel { Message = "Unable to change user password" });

			var response = _userService.ChangePassword(user.Id, model.NewPassword);

			if (!response)
				return BadRequest(new ErrorModel { Message = "Unable to change user password" });

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
