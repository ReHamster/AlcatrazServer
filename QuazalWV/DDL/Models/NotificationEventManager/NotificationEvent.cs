using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuazalWV.DDL.Models
{
	public enum NotificationType : int
	{
		PartyEvent = 1004000,

		// 3001 	New participant
		// 3002 	Participation cancelled
		// 3007 	Participant disconnected
		// 3008 	Participation ended
		// 4000 	Ownership change
		// 109000 	Gathering unregistered
		// 116000 	Matchmake referee round started
	}

	public class NotificationEvent
	{
		public NotificationEvent()
		{

		}
		public NotificationEvent(NotificationType type, uint subType)
		{
			m_uiType = (uint)type | subType;
		}

		public uint m_pidSource { get; set; }

		// NotificationType
		public uint m_uiType { get; set; }
		public uint m_uiParam1 { get; set; }
		public uint m_uiParam2 { get; set; }
		public string m_strParam { get; set; }
}
}
