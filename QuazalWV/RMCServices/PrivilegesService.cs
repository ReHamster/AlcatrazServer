using QuazalWV.Attributes;
using QuazalWV.Interfaces;

namespace QuazalWV.RMCServices
{
	/// <summary>
	/// Secure connection service protocol
	/// </summary>
	[RMCService(RMCP.PROTOCOL.PrivilegesService)]
	public class PrivilegesService : RMCServiceBase
	{
		[RMCMethod(1)] 	public void GetPrivileges(RMCPacketGetPrivileges request)
		{
			var response = new RMCPacketResponseGetPrivileges();

			// TODO: populate

			SendResponseWithACK(response);
		}

		[RMCMethod(2)] 	public void ActivateKey()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(3)] 	public void ActivateKeyWithExpectedPrivileges()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(4)] 	public void GetPrivilegeRemainDuration()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(5)] 	public void GetExpiredPrivileges()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(6)]  public void GetPrivilegesEx()
		{
			UNIMPLEMENTED();
		}

	}
}
