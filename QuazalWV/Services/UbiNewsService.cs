using QuazalWV.Attributes;
using QuazalWV.Interfaces;
using QuazalWV.DDL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuazalWV.Services
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
				m_locale = "en-US",
				m_name = "Test",
				m_description = "Test news channel",
				m_ownerPID = Context.Client.sPID,
				m_subscribable = false,
				m_type = "Title",
				m_creationTime = DateTime.UtcNow,
				m_expirationTime = DateTime.UtcNow.AddDays(100),
			};

			return Result(result);
		}
	}
}
