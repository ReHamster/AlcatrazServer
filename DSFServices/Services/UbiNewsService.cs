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
			var newsDataBytes = "02 00 00 00 02 00 00 00 05 00 4E 61 6D 65 00 0C 00 44 65 73 63 72 69 70 74 69 6F 6E 00 A2 F9 FE 6C 1F 00 00 00 00 00 00 00 00 00 00 00 08 00 55 62 69 4E 65 77 73 00 06 00 65 6E 2D 55 53 00 00";

			var stream = new MemoryStream(Helper.ParseByteArray(newsDataBytes));
			var newsData = DDLSerializer.ReadObject<NewsChannel>(stream);

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
