using QuazalWV.Attributes;
using QuazalWV.Interfaces;
using System.Collections.Generic;

namespace QuazalWV.RMCServices
{
	/// <summary>
	/// Secure connection service protocol
	/// </summary>
	[RMCService(RMCP.PROTOCOL.PrivilegesService)]
	public class PrivilegesService : RMCServiceBase
	{
		[RMCMethod(1)] 	
		public RMCResult GetPrivileges(string localeCode)
		{
			var result = new Dictionary<uint, Privilege>();

			// TODO: populate
			//var response = new RMCPacketResponseGetPrivileges();
			//SendResponseWithACK(response);

			return Result(result);
		}

		[RMCMethod(2)] 	
		public void ActivateKey()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(3)] 	
		public void ActivateKeyWithExpectedPrivileges()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(4)] 	
		public void GetPrivilegeRemainDuration()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(5)] 	
		public void GetExpiredPrivileges()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(6)] 
		public void GetPrivilegesEx()
		{
			UNIMPLEMENTED();
		}

	}
}
