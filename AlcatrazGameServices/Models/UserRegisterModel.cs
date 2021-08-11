using Newtonsoft.Json;

namespace Alcatraz.GameServices.Models
{
	public class UserRegisterModel
	{
		public string Username { get; set; }
		[JsonIgnore]
		public string Password { get; set; }

		public string PlayerNickName { get; set; }
	}
}
