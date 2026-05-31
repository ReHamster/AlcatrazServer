using Alcatraz.DTO.Models;
using Alcatraz.GameServices.Helpers;
using Alcatraz.GameServices.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using QNetZ;
using System.Linq;
using System.Text;
using Constants = Alcatraz.DTO.Constants;

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

		[Authorize]
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
