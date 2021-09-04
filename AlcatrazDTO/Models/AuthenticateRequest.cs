using System.ComponentModel.DataAnnotations;

namespace Alcatraz.DTO.Models
{
	public class AuthenticateRequest
	{
		[Required]
		public string Username { get; set; }

		[Required]
		public string Password { get; set; }
	}
}
