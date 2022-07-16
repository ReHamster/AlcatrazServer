using DSFServices.DDL.Models;
using QNetZ;
using QNetZ.Attributes;
using QNetZ.DDL;
using QNetZ.Interfaces;
using System;
using System.IO;

namespace DSFServices.Services
{
	/// <summary>
	/// Ubi news service
	/// </summary>
	[RMCService(RMCProtocolId.UbiNewsService)]
	public class UbiNewsService : RMCServiceBase
	{
		[RMCMethod(1)]
		public RMCResult GetChannel()
		{
			var result = new NewsChannel
			{
				m_ID = 1,
				m_ownerPID = Context.Client.sPID,
				m_locale = "en-US",
				m_name = "Name",
				m_description = "Description",
				
				m_subscribable = false,
				m_type = "UbiNews",
				m_creationTime = DateTime.MinValue,
				m_expirationTime = DateTime.MinValue,
			};

			return Result(result);
		}
	}
}
