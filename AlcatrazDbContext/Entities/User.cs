using Newtonsoft.Json;

namespace Alcatraz.Context.Entities
{
	public class User
	{
		public uint Id { get; set; }
		public string GameNickName { get; set; }
		public string Username { get; set; }

		[JsonIgnore]
		public string Password { get; set; }
	}
}
