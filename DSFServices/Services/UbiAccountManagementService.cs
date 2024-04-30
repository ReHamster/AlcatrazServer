using DSFServices.DDL.Models;
using QNetZ;
using QNetZ.Attributes;
using QNetZ.DDL;
using QNetZ.Interfaces;
using RDVServices;
using System.Collections.Generic;
using System.IO;
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
			var playerInfo = Context.Client.PlayerInfo;
			var account = new UbiAccount()
			{
				m_ubiAccountId = playerInfo.AccountId,
				m_username = playerInfo.Name,
				m_password = "",
				m_firstName = "",
				m_lastName = "",
				m_countryCode = "KZ",
				m_email = "whatever@dontcare.com",
				m_preferredLanguage = "en",
				m_gender = 0,
				m_optIn = true,
				m_thirdPartyOptIn = true,
				m_status = new UbiAccountStatus()
                {
					m_basicStatus = 2,
					m_missingRequiredInformations = false,
					m_pendingDeactivation = false,
					m_recoveringPassword = true
                },
				m_externalAccounts = new List<ExternalAccount>()
                {
					new ExternalAccount()
                    {
						m_accountType = 11,
						m_id = "loh",
						m_username = "aabb0"
                    },
					new ExternalAccount()
					{
						m_accountType = 25,
						m_id = "pidr",
						m_username = "aabb1"
					},
					new ExternalAccount()
					{
						m_accountType = 31,
						m_id = "whatev",
						m_username = "aabb2"
					}

				},
				m_dateOfBirth = new System.DateTime(1990, 11, 1)
			};
			
			return Result(new { account = account, exist = true});
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
		public RMCResult LookupUbiAccountIDsByPids(IEnumerable<uint> pids)
		{
			var ubiaccountIDs = new Dictionary<uint, string>();

			using (var db = DBHelper.GetDbContext())
			{
				var usersList = db.Users.Where(x => pids.Contains(x.Id)).ToArray();

				foreach (var usr in usersList)
				{
					ubiaccountIDs[usr.Id] = usr.PlayerNickName;
				}
			}

			return Result(ubiaccountIDs);
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
		public RMCResult LookupUsernamesByUbiAccountIDs(IEnumerable<string> UbiAccountIds)
		{
			var Usernames = new Dictionary<string, string>();

			using (var db = DBHelper.GetDbContext())
			{
				var usersList = db.Users.Where(x => UbiAccountIds.Contains(x.Username)).ToArray();

				foreach (var usr in usersList)
				{
					Usernames[usr.Username] = usr.Username;
				}
			}

			return Result(Usernames);
		}

		[RMCMethod(15)]
		public RMCResult LookupUbiAccountIDsByUsernameSubString(string UsernameSubString)
		{
			var UbiAccountIDs = new Dictionary<string, string>();
			
			UNIMPLEMENTED();
			return Result(UbiAccountIDs);
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
