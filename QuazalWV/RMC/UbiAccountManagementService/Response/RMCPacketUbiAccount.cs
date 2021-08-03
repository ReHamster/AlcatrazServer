using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuazalWV
{
	public class UbiAccountStatus
	{
		public UbiAccountStatus()
		{
			m_basicStatus = 0;
			m_missingRequiredInformations = false;
			m_recoveringPassword = false;
			m_pendingDeactivation = false;
		}

		public uint m_basicStatus;
		public bool m_missingRequiredInformations;
		public bool m_recoveringPassword;
		public bool m_pendingDeactivation;

		public void toBuffer(Stream s)
		{
			Helper.WriteU32(s, m_basicStatus);
			Helper.WriteBool(s, m_missingRequiredInformations);
			Helper.WriteBool(s, m_recoveringPassword);
			Helper.WriteBool(s, m_pendingDeactivation);
		}
	}

	public class ExternalAccount
	{
		public ExternalAccount()
		{
			m_accountType = 0;
		}

		uint m_accountType;
		string m_id;
		string m_username;

		public void toBuffer(Stream s)
		{
			Helper.WriteU32(s, m_accountType);
			Helper.WriteString(s, m_id);
			Helper.WriteString(s, m_username);
		}
	}

	public class RMCPacketUbiAccount : RMCPResponse
	{
		public RMCPacketUbiAccount()
		{
			m_externalAccounts = new List<ExternalAccount>();
			m_status = new UbiAccountStatus();
			m_dateOfBirth = new DateTime(2000, 10, 12);
		}

		public string m_ubiAccountId;
		public string m_username;
		public string m_password;
		public UbiAccountStatus m_status;
		public string m_email;
		public DateTime m_dateOfBirth;
		public uint m_gender;
		public string m_countryCode;
		public bool m_optIn;
		public bool m_thirdPartyOptIn;
		public string m_firstName;
		public string m_lastName;
		public string m_preferredLanguage;
		public List<ExternalAccount> m_externalAccounts;

		public override string PayloadToString()
		{
			return "";
		}

		public override byte[] ToBuffer()
		{
			MemoryStream m = new MemoryStream();

			Helper.WriteString(m, m_ubiAccountId);
			Helper.WriteString(m, m_username);
			Helper.WriteString(m, m_password);
			m_status.toBuffer(m);

			Helper.WriteString(m, m_email);
			Helper.WriteDateTime(m, m_dateOfBirth);

			Helper.WriteU32(m, m_gender);
			Helper.WriteString(m, m_countryCode);
			Helper.WriteBool(m, m_optIn);
			Helper.WriteBool(m, m_thirdPartyOptIn);
			Helper.WriteString(m, m_firstName);
			Helper.WriteString(m, m_lastName);
			Helper.WriteString(m, m_preferredLanguage);

			Helper.WriteU32(m, (uint)m_externalAccounts.Count);
			foreach (var acc in m_externalAccounts)
				acc.toBuffer(m);

			return m.ToArray();
		}

		public override string ToString()
		{
			return "[RMCPacketUbiAccount]";
		}
	}
}
