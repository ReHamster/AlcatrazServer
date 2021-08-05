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
	[RMCService(RMCProtocol.NewsService)]
	public class NewsService : RMCServiceBase
	{
		[RMCMethod(8)]
		public RMCResult GetNewsHeaders(NewsRecipient recipient, uint offset, uint size)
		{
			var headers = new List<NewsHeader>();
			headers.Add(new NewsHeader
			{
				m_ID = 0,
				m_publisherName = "SoapyMan",
				m_title = "SoapyMan overtakes the world and raises taxes by 400% on coffee!",
				m_link = "",
				m_displayTime = DateTime.UtcNow,
				m_expirationTime = DateTime.UtcNow.AddDays(10),
				m_publicationTime = new DateTime(2000, 10, 12, 13, 0, 0),
				m_publisherPID = Context.Client.PID,
				m_recipientID = Context.Client.IDsend,
				m_recipientType = 0,
			});

			headers.Add(new NewsHeader
			{
				m_ID = 0,
				m_publisherName = "SoapyMan",
				m_title = "Inspiration Byte stocks has raisen by 5.6% last trading day",
				m_link = "",
				m_displayTime = DateTime.UtcNow,
				m_expirationTime = DateTime.UtcNow.AddDays(10),
				m_publicationTime = new DateTime(2000, 10, 12, 13, 0, 0),
				m_publisherPID = Context.Client.PID,
				m_recipientID = Context.Client.IDsend,
				m_recipientType = 0,
			});

			return Result(headers);
		}

		[RMCMethod(9)]
		public RMCResult GetNewsMessages(IEnumerable<uint> messageIds)
		{
			var messages = new List<string>();
			messages.Add("This text apparently doesn't work.");

			return Result(messages);
		}
	}
}
