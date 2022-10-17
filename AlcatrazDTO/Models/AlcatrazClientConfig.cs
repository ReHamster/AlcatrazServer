using System;
using System.Collections.Generic;
using System.Text;

namespace Alcatraz.DTO.Models
{
	public class ProfileConfig
	{
		public string Username { get; set; }		// empty when Ubi
		public string AccountId { get; set; }
		public string Password { get; set; }
		public string GameKey { get; set; }			// empty when alcatraz
		public string ServiceUrl { get; set; }      // empty when Ubi
		public string ConfigKey { get; set; }       // empty when Ubi
		public string AccessKey { get; set; }       // empty when Ubi
	}

	public class AlcatrazClientConfig
	{
		public AlcatrazClientConfig()
		{
			Profiles = new Dictionary<string, ProfileConfig>();
			BorderlessWindow = true;
			DiscordRichPresence = true;
		}

		public bool BorderlessWindow { get; set; }
		public bool DiscordRichPresence { get; set; }

		public string UseProfile { get; set; }
		public Dictionary<string,ProfileConfig> Profiles { get; set; }

		//-------------------------

		public static AlcatrazClientConfig Instance { get; set; }
	}
}
