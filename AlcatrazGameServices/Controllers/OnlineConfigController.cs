using Alcatraz.DTO.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using QNetZ;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Alcatraz.GameServices.Controllers
{
	[ApiController]
	[Route("OnlineConfigService.svc")]
	public class OnlineConfigController : ControllerBase
	{
		private readonly ILogger<OnlineConfigController> _logger;
		private readonly IOptions<QConfiguration> _configuration;

		public OnlineConfigController(ILogger<OnlineConfigController> logger, IOptions<QConfiguration> serverConfig)
		{
			_configuration = serverConfig;
			_logger = logger;
		}

		private static Dictionary<string, string> ResponseTemplates = new Dictionary<string, string>()
		{
			{ "SandboxUrl",						@"prudp:/address=#ADDRESS#;port=#PORT#"},
			{ "SandboxUrlWS",                   @"#ADDRESS#:#PORT#"},
			{ "uplay_DownloadServiceUrl",       @"http://#ADDRESS#/UplayServices/UplayFacade/DownloadServicesRESTXML.svc/REST/XML/?url="},
			{ "uplay_DynContentBaseUrl",        @"http://#ADDRESS#/u/Uplay/"},
			{ "uplay_DynContentSecureBaseUrl",  @"http://#ADDRESS#/"},
			{ "uplay_LinkappBaseUrl",           @"http://#ADDRESS#/u/Uplay/Packages/linkapp/1.1/"},
			{ "uplay_PackageBaseUrl",           @"http://#ADDRESS#/u/Uplay/Packages/1.0.1/"},
			{ "uplay_WebServiceBaseUrl",        @"http://#ADDRESS#/UplayServices/UplayFacade/ProfileServicesFacadeRESTXML.svc/REST/"},
		};

		[HttpGet("GetOnlineConfig")]
		public IEnumerable<OnlineConfigEntry> GetOnlineConfig(string onlineConfigID, string target)
		{
			_logger.LogInformation($"Requested game config '{ onlineConfigID }'");

			// TODO: database access for 'onlineConfigID'

			var hostAddress = string.IsNullOrWhiteSpace(_configuration.Value.ServerBindAddress) ? Dns.GetHostName() : _configuration.Value.ServerBindAddress;
			var targetPort = _configuration.Value.RDVServerPort;

			var list = new List<OnlineConfigEntry>();

			foreach(var v in ResponseTemplates)
			{
				var value = v.Value
					.Replace("#ADDRESS#", hostAddress)
					.Replace("#PORT#", targetPort.ToString());

				list.Add(new OnlineConfigEntry
				{
					Name = v.Key,
					Values = new[] { value }
				});
			}

			return list;
		}
	}

	[ApiController]
	[Route("MatchMakingConfig.aspx")]
	public class MatchMakingConfigController : ControllerBase
	{
		private readonly ILogger<MatchMakingConfigController> _logger;
		private readonly IOptions<QConfiguration> _configuration;

		public MatchMakingConfigController(ILogger<MatchMakingConfigController> logger, IOptions<QConfiguration> serverConfig)
		{
			_configuration = serverConfig;
			_logger = logger;
		}

		[HttpGet]
		public IEnumerable<OnlineConfigEntry> Get(string action, string gid, string locale, string format)
		{
			var list = new List<OnlineConfigEntry>();
			return list;
		}
	}
}
