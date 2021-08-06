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
			var random = new Random();

			string[] funNews = {
				"Actumcnally",
				$"Welcome to Driver Madness server free from U-beey-soft!",
				$"SoapyMan overtakes the DSF and reduces taxes on coffee by {random.Next(10, 400)}%!",
				"Play latest REDRIVER2 at opendriver2.github.io!",
				$"Inspiration Byte stocks has raisen by { random.Next(1, 40) }% last trading day!",
				"Subscribe to VortexStory on YouTube!",
				"Play online matches with Driver Madness discord members at fridays!",
				$"Shocking! Olanov started playing DSF again! Stocks has risen by { random.Next(4, 40) }"
			};

			var headers = funNews.Select((x, idx) => new NewsHeader
			{
				m_ID = (uint)idx + 1,
				m_publisherName = "SoapyMan",
				m_title = x,
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
