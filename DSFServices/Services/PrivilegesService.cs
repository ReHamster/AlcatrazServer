using DSFServices.DDL.Models;
using QNetZ;
using QNetZ.Attributes;
using QNetZ.Interfaces;
using System.Collections.Generic;

namespace DSFServices.Services
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
			var result = new Dictionary<uint, Privilege>();

			result.Add(1, new Privilege()
			{
				m_ID = 1,
				m_description = "Allow to play online"
			});

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
