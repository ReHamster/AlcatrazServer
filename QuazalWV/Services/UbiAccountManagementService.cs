using QuazalWV.Attributes;
using QuazalWV.Interfaces;
using QuazalWV.DDL.Models;


namespace QuazalWV.Services
{
	/// <summary>
	/// Ubi account management service
	/// </summary>
	[RMCService(RMCProtocol.UbiAccountManagementService)]
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
			var reply = new UbiAccount();
			reply.m_ubiAccountId = Context.Client.accountId;
			reply.m_username = Context.Client.name;
			reply.m_password = Context.Client.pass;
			reply.m_firstName = "Soapy";
			reply.m_lastName = "Man";
			reply.m_countryCode = "kz";
			reply.m_email = "dumb@ass.com";

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
		public void LookupPrincipalIds()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(12)]
		public void LookupUbiAccountIDsByPids()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(13)]
		public void LookupUbiAccountIDsByUsernames()
		{
			UNIMPLEMENTED();
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
