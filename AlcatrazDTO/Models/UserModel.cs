using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Alcatraz.DTO.Models
{
	public class UserModel
	{
		public uint Id { get; set; }
		public string Username { get; set; }

		[MaxLength(14, ErrorMessage = "Nickname can't be longer than 14 characters (sorry)")]
		public string PlayerNickName { get; set; }

		public DateTime CreatedTime { get; set; }
		public DateTime LastUpdateTime { get; set; }
		public DateTime LastPlayTime { get; set; }
		public bool IsAdmin { get; set; }
	}
}
