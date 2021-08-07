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
	}

	public class NotificationEvent
	{
		public NotificationEvent()
		{

		}
		public NotificationEvent(NotificationType type, uint subType)
		{
			m_uiType = (uint)type + subType;
		}

		public uint m_pidSource { get; set; }

		// NotificationType
		public uint m_uiType { get; set; }
		public uint m_uiParam1 { get; set; }
		public uint m_uiParam2 { get; set; }
		public string m_strParam { get; set; }
		public uint m_uiParam3 { get; set; }

		public override string ToString()
		{
			var stringBuilder = new StringBuilder();

			stringBuilder.AppendLine("NotificationEvent {");
			stringBuilder.AppendLine($"    m_pidSource = { m_pidSource }");
			stringBuilder.AppendLine($"    m_uiType = ({ (NotificationType)(m_uiType - (m_uiType % 1000)) }, { m_uiType % 1000 })");
			stringBuilder.AppendLine($"    m_uiParam1 = { m_uiParam1 }");
			stringBuilder.AppendLine($"    m_uiParam2 = { m_uiParam2 }");
			stringBuilder.AppendLine($"    m_strParam = { m_strParam }");
			stringBuilder.AppendLine($"    m_uiParam3 = { m_uiParam3 }");
			stringBuilder.AppendLine("}");

			return stringBuilder.ToString();
		}
}
}
