using Newtonsoft.Json;
using System;

namespace Alcatraz.Context.Entities
{
	public class User
	{
		public uint Id { get; set; }
		
		public string Username { get; set; }
		public string PlayerNickName { get; set; }
		[JsonIgnore]
		public string Password { get; set; }
		public int RewardFlags { get; set; }
		public DateTime CreatedTime { get; set; }
		public DateTime LastUpdateTime { get; set; }
		public DateTime LastPlayTime { get; set; }
		public bool IsAdmin { get; set; }
	}
}
