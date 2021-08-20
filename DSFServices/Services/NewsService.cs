using DSFServices.DDL.Models;
using QNetZ;
using QNetZ.Attributes;
using QNetZ.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DSFServices.Services
{
	/// <summary>
	/// Ubi news service
	/// </summary>
	[RMCService(RMCProtocolId.NewsService)]
	public class NewsService : RMCServiceBase
	{
		[RMCMethod(8)]
		public RMCResult GetNewsHeaders(NewsRecipient recipient, uint offset, uint size)
		{
			var plInfo = Context.Client.Info;
			var random = new Random();

			var funNews = new List<string>{
				"Actumcnally",
				$"Hello { plInfo.Name }! Welcome to Driver Madness Alcatraz server!",
				$"Players online: { NetworkPlayers.Players.Count }",
				$"SoapyMan overtakes the DSF and reduces taxes on coffee by {random.Next(10, 400)}%!",
				"Play Driver 2 at opendriver2.github.io!",
				$"Inspiration Byte stocks has raisen by { random.Next(1, 40) }% last trading day!",
				"Subscribe to VortexStory on YouTube!",
			};

			if (NetworkPlayers.GetPlayerInfoByUsername("Olanov") != null)
			{
				funNews.Insert(2, $"Shocking! Olanov started playing DSF again! Stocks has risen by { random.Next(4, 40) }");
			}

			var headers = funNews.Select((x, idx) => new NewsHeader
			{
				m_ID = (uint)idx + 1,
				m_publisherName = "SoapyMan",
				m_title = x,
				m_link = "",
				m_displayTime = DateTime.UtcNow,
				m_expirationTime = DateTime.UtcNow.AddDays(10),
				m_publicationTime = new DateTime(2000, 10, 12, 13, 0, 0),
				m_publisherPID = Context.Client.sPID,
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
