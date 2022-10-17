using Alcatraz.GameServices.Helpers;
using Alcatraz.GameServices.Services;
using Alcatraz.DTO.Models;

using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Extensions.Options;
using QNetZ;
using Alcatraz.DTO;
using Constants = Alcatraz.DTO.Constants;
using Microsoft.Extensions.Configuration;

namespace Alcatraz.GameServices.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class UsersController : ControllerBase
	{
		private IUserService _userService;
		private readonly IOptions<QConfiguration> _configuration;

		public UsersController(IUserService userService, IOptions<QConfiguration> serverConfig)
		{
			_userService = userService;
			_configuration = serverConfig;
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

		[HttpGet("GetAlcatrazConfig")]
		public IActionResult GetAlcatrazConfig()
		{
			var claims = HttpContext.User.Claims;
			var uidClaim = claims.FirstOrDefault(x => x.Type == "uid");

			UserModel currentUser = null;

			uint uid = 0;
			if (uint.TryParse(uidClaim.Value, out uid))
				currentUser = _userService.GetById(uid);

			if (currentUser == null)
				return Unauthorized();

			var settings = new JsonSerializerSettings
			{
				Formatting = Formatting.Indented,
				ContractResolver = new DefaultContractResolver(),
				DefaultValueHandling = DefaultValueHandling.Include,
				TypeNameHandling = TypeNameHandling.None,
				NullValueHandling = NullValueHandling.Ignore,
				ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
			};

			var serviceUrl = _configuration.Value.ServiceURLHostName;
			var accessKey = _configuration.Value.SandboxAccessKey;
			var configKey = _configuration.Value.OnlineConfigKey;

			var passwordHash = DTO.Helpers.SecurePasswordHasher.Hash($"{currentUser.Id}-{currentUser.PlayerNickName}");

			var profileConfig = new ProfileConfig()
			{
				Username = currentUser.Username,
				AccountId = currentUser.PlayerNickName,
				Password = passwordHash,

				ServiceUrl = serviceUrl,
				AccessKey = accessKey,
				ConfigKey = configKey,
			};

			var config = new AlcatrazClientConfig();
			config.BorderlessWindow = true;
			config.DiscordRichPresence = true;
			config.UseProfile = Constants.AlcatrazProfileKey;
			config.Profiles.Add(Constants.AlcatrazProfileKey, profileConfig);

			var alcatrazConfig = JsonConvert.SerializeObject(config, settings);
			var bytes = Encoding.ASCII.GetBytes(alcatrazConfig);

			return File(bytes, "application/json", Constants.ConfigFilename);
		}
	}
}
