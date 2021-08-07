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
	[RMCService(RMCProtocolId.NewsService)]
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

			// returns E9 03 00 00 1F 01 27 00 00 00 08 80 00 00 07 00 00 00 5B 47 06 00 02 00 00 00 02 00 00 00 02 00 00 00 13 00 51 75 61 7A 61 6C 20 52 65 6E 64 65 7A 2D 56 6F 75 73 00 00 90 C6 71 1F 00 00 00 B2 8E 0E 96 1F 00 00 00 B2 8E 0E 9A 1F 00 00 00 1D 00 57 65 6C 63 6F 6D 65 20 74 6F 20 74 68 65 20 6E 65 77 20 55 70 6C 61 79 20 50 43 21 00 01 00 00 5C 47 06 00 02 00 00 00 02 00 00 00 02 00 00 00 13 00 51 75 61 7A 61 6C 20 52 65 6E 64 65 7A 2D 56 6F 75 73 00 00 90 82 71 1F 00 00 00 B2 8E 0E 96 1F 00 00 00 B2 8E 0E 9A 1F 00 00 00 18 00 55 62 69 73 6F 66 74 20 43 6F 6E 66 65 72 65 6E 63 65 20 4C 69 76 65 00 01 00 00 5D 47 06 00 02 00 00 00 02 00 00 00 02 00 00 00 13 00 51 75 61 7A 61 6C 20 52 65 6E 64 65 7A 2D 56 6F 75 73 00 00 90 82 71 1F 00 00 00 B2 8E 0E 96 1F 00 00 00 B2 8E 0E 9A 1F 00 00 00 06 00 47 69 66 74 73 00 01 00 00 5E 47 06 00 02 00 00 00 02 00 00 00 02 00 00 00 13 00 51 75 61 7A 61 6C 20 52 65 6E 64 65 7A 2D 56 6F 75 73 00 00 90 1E 6F 1F 00 00 00 B2 8E 0E 96 1F 00 00 00 B2 8E 0E 9A 1F 00 00 00 77 00 4E 65 77 20 6D 75 6C 74 69 70 6C 61 79 65 72 20 63 6F 6E 74 65 6E 74 20 68 61 73 20 6A 75 73 74 20 62 65 65 6E 20 75 6E 6C 6F 63 6B 65 64 21 20 4E 65 77 20 72 6F 75 74 65 73 20 61 6E 64 20 73 74 61 72 74 69 6E 67 20 70 6F 69 6E 74 73 20 61 72 65 20 6E 6F 77 20 61 76 61 69 6C 61 62 6C 65 20 77 68 65 6E 20 79 6F 75 20 70 6C 61 79 20 6F 6E 6C 69 6E 65 2E 00 01 00 00 60 47 06 00 02 00 00 00 02 00 00 00 02 00 00 00 13 00 51 75 61 7A 61 6C 20 52 65 6E 64 65 7A 2D 56 6F 75 73 00 00 90 18 6F 1F 00 00 00 B2 8E 0E 96 1F 00 00 00 B2 8E 0E 9A 1F 00 00 00 81 00 4A 6F 69 6E 20 74 68 65 20 44 72 69 76 65 72 20 43 6C 75 62 20 6F 6E 20 64 72 69 76 65 72 2D 63 6C 75 62 2E 75 62 69 2E 63 6F 6D 21 20 53 65 65 20 79 6F 75 72 20 73 74 61 74 73 2C 20 66 6F 6C 6C 6F 77 20 79 6F 75 72 20 66 72 69 65 6E 64 27 73 20 61 63 74 69 76 69 74 79 2C 20 62 72 6F 77 73 65 20 46 69 6C 6D 20 44 69 72 65 63 74 6F 72 20 63 6C 69 70 73 20 61 6E 64 20 6D 6F 72 65 21 00 01 00 00 5F 47 06 00 02 00 00 00 02 00 00 00 02 00 00 00 13 00 51 75 61 7A 61 6C 20 52 65 6E 64 65 7A 2D 56 6F 75 73 00 00 90 AA 6E 1F 00 00 00 B2 8E 0E 96 1F 00 00 00 B2 8E 0E 9A 1F 00 00 00 5F 00 43 6F 6E 67 72 61 74 75 6C 61 74 69 6F 6E 73 2C 20 64 72 69 76 65 72 73 21 20 54 6F 67 65 74 68 65 72 2C 20 79 6F 75 27 76 65 20 75 6E 6C 6F 63 6B 65 64 20 61 20 6E 65 77 20 76 65 72 73 69 6F 6E 20 6F 66 20 74 68 65 20 6F 6E 6C 69 6E 65 20 54 61 6B 65 64 6F 77 6E 20 6D 6F 64 65 21 00 01 00 00 61 47 06 00 02 00 00 00 02 00 00 00 02 00 00 00 13 00 51 75 61 7A 61 6C 20 52 65 6E 64 65 7A 2D 56 6F 75 73 00 00 90 3E 6E 1F 00 00 00 B2 8E 0E 96 1F 00 00 00 B2 8E 0E 9A 1F 00 00 00 7B 00 44 69 64 20 79 6F 75 20 6B 6E 6F 77 3F 20 54 61 6E 6E 65 72 27 73 20 79 65 6C 6C 6F 77 20 61 6E 64 20 62 6C 61 63 6B 20 43 68 61 6C 6C 65 6E 67 65 72 20 69 73 20 61 20 6E 6F 64 20 74 6F 20 61 20 6D 79 74 68 69 63 61 6C 20 63 61 72 20 66 72 6F 6D 20 74 
			// 68 65 20 62 65 74 61 20 6F 66 20 74 68 65 20 76 65 72 79 20 66 69 72 73 74 20 44 72 69 76 65 72 20 67 61 6D 65 21 00 01 00 00 
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
