using System;
using System.Collections.Generic;
using System.Text;

namespace Alcatraz.DTO.Models
{
	public class OnlineConfig
	{
		public bool Use { get; set; }
		public string ServiceUrl { get; set; }
		public string ConfigKey { get; set; }
		public string AccessKey { get; set; }
	}

	public class ProfileConfig
	{
		public string AccountId { get; set; }
		public string Password { get; set; }
		public string GameKey { get; set; }
	}

	public class AlcatrazClientConfig
	{
		public OnlineConfig OnlineConfig { get; set; }
		public ProfileConfig Profile { get; set; }
	}
}
