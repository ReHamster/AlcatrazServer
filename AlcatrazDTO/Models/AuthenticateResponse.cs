namespace Alcatraz.DTO.Models
{
	public class AuthenticateResponse
	{
		public uint Id { get; set; }
		public string PlayerNickName { get; set; }
		public string Username { get; set; }
		public string Token { get; set; }

		public AuthenticateResponse()
		{

		}

		public AuthenticateResponse(UserModel user, string token)
		{
			Id = user.Id;
			PlayerNickName = user.PlayerNickName;

			Username = user.Username;
			Token = token;
		}
	}
}
