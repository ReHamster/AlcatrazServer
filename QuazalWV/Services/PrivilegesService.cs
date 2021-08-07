using QuazalWV.Attributes;
using QuazalWV.Interfaces;
using QuazalWV.DDL.Models;
using System.Collections.Generic;

namespace QuazalWV.Services
{
	/// <summary>
	/// Secure connection service protocol
	/// </summary>
	[RMCService(RMCProtocolId.PrivilegesService)]
	public class PrivilegesService : RMCServiceBase
	{
		[RMCMethod(1)]
		public RMCResult GetPrivileges(string localeCode)
		{
			// return bytes:
			//2D 00 00 00 23 01 16 00 00 00 01 80 00 00 01 00 00 00 01 00 00 00 01 00 00 00 15 00 41 6C 6C 6F 77 20 74 6F 20 70 6C 61 79 20 6F 6E 6C 69 6E 65 00

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
