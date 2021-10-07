using DSFServices.DDL.Models;
using QNetZ;
using QNetZ.Attributes;
using QNetZ.Interfaces;
using RDVServices;
using System.Collections.Generic;
using System.Linq;

namespace DSFServices.Services
{
	/// <summary>
	/// Ubi account management service
	/// </summary>
	[RMCService(RMCProtocolId.UbiAccountManagementService)]
	public class UbiAccountManagementService : RMCServiceBase
	{
		[RMCMethod(1)]
		public void CreateAccount()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(2)]
		public void UpdateAccount()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(3)]
		public RMCResult GetAccount()
		{
			var playerInfo = Context.Client.Info;
			var reply = new UbiAccount()
			{
				m_ubiAccountId = playerInfo.AccountId,
				m_username = playerInfo.Name,
				m_password = "[REDACTED]",
				m_firstName = "Soapy",
				m_lastName = "Man",
				m_countryCode = "kz",
				m_email = "dumb@ass.com",
				m_preferredLanguage = "en",
				m_gender = 0,
			};
			
			return Result(reply);
		}

		[RMCMethod(4)]
		public void LinkAccount()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(5)]
		public void GetTOS()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(6)]
		public void ValidateUsername()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(7)]
		public void ValidatePassword()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(8)]
		public void ValidateEmail()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(9)]
		public void GetCountryList()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(10)]
		public void ForgetPassword()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(11)]
		public RMCResult LookupPrincipalIds(IEnumerable<string> ubiAccountIds)
		{
			var pids = new Dictionary<string, uint>();
			using (var db = DBHelper.GetDbContext())
			{
				var usersList = db.Users.Where(x => ubiAccountIds.Contains(x.Username)).ToArray();

				foreach (var usr in usersList)
				{
					pids[usr.Username] = usr.Id;
				}
			}
			return Result(pids);
		}

		[RMCMethod(12)]
		public void LookupUbiAccountIDsByPids()
		{
			// TODO: implement RMC method
			UNIMPLEMENTED();
		}

		[RMCMethod(13)]
		public RMCResult LookupUbiAccountIDsByUsernames(IEnumerable<string> Usernames)
		{
			var UbiAccountIDs = new Dictionary<string, string>();

			using(var db = DBHelper.GetDbContext())
			{
				var usersList = db.Users.Where(x => Usernames.Contains(x.PlayerNickName)).ToArray();

				foreach(var usr in usersList)
				{
					UbiAccountIDs[usr.Username] = usr.PlayerNickName;
				}
			}

			return Result(UbiAccountIDs);
		}

		[RMCMethod(14)]
		public void LookupUsernamesByUbiAccountIDs()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(15)]
		public void LookupUbiAccountIDsByUsernameSubString()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(16)]
		public void UserHasPlayed()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(17)]
		public void IsUserPlaying()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(18)]
		public void LookupUbiAccountIDsByUsernamesGlobal()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(19)]
		public void LookupUbiAccountIDsByEmailsGlobal()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(20)]
		public void LookupUsernamesByUbiAccountIDsGlobal()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(21)]
		public void GetTOSEx()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(22)]
		public void HasAcceptedLatestTOS()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(23)]
		public void AcceptLatestTOS()
		{
			UNIMPLEMENTED();
		}
	}
}
