using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuazalWV
{
    public class Gathering
    {
        public uint m_idMyself { get; set; }
		public uint m_pidOwner { get; set; }
		public uint m_pidHost { get; set; }
		public ushort m_uiMinParticipants { get; set; }
		public ushort m_uiMaxParticipants { get; set; }
		public uint m_uiParticipationPolicy { get; set; }
		public uint m_uiPolicyArgument { get; set; }
		public uint m_uiFlags { get; set; }
		public uint m_uiState { get; set; }
		public string m_strDescription { get; set; }
    }

	public class HermesPartySession : Gathering
	{
		public short m_freePublicSlots { get; set; }
		public short m_freePrivateSlots { get; set; }
		public short m_maxPrivateSlots { get; set; }
		public uint m_privacySettings { get; set; }
		public string m_name { get; set; }
		public string m_buffurizedOwnerId { get; set; }
	}
}
