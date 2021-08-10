using Alcatraz.Context.Entities;

namespace Alcatraz.GameServices.Models
{
	public class AuthenticateResponse
	{
		public uint Id { get; set; }
		public string GameNickName { get; set; }
		public string Username { get; set; }
		public string Token { get; set; }


		public AuthenticateResponse(User user, string token)
		{
			Id = user.Id;
			GameNickName = user.GameNickName;

			Username = user.Username;
			Token = token;
		}
	}
}
