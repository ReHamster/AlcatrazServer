using Alcatraz.DTO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlcatrazLauncher.Helpers
{
	/// <summary>
	/// Информация о сессии пользователя
	/// </summary>
	class SessionInfo
	{
		private static SessionInfo _session = new SessionInfo();
		public static SessionInfo Get() { return _session; }

		// global properties
		public AuthenticateResponse LoginData { get; set; }
	}
}
