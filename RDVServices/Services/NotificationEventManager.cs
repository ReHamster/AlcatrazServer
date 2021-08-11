using DSFServices.DDL.Models;
using QNetZ;
using QNetZ.Attributes;
using QNetZ.Interfaces;

namespace RDVServices.Services
{
	[RMCService(RMCProtocolId.NotificationEventManager)]
	public class NotificationEventManager : RMCServiceBase
	{
		[RMCMethod(1)]
		public void Notify(NotificationEvent notification)
		{
			// Dummy event
		}
	}
}
