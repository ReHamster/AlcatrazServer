﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRPBackendWV
{
    public class RMCPacketResponseInboxMessageService_Method2 : RMCPacketReply
    {
        public class InboxMessage
        {
            public uint unk1;
            public uint unk2;
            public uint unk3;
            public uint unk4;
            public string unk5;
            public ulong unk6;
            public uint unk7;
            public void toBuffer(Stream s)
            {
                Helper.WriteU32(s, unk1);
                Helper.WriteU32(s, unk2);
                Helper.WriteU32(s, unk3);
                Helper.WriteU32(s, unk4);
                Helper.WriteString(s, unk5);
                Helper.WriteU64(s, unk6);
                Helper.WriteU32(s, unk7);
            }
        }

        public List<InboxMessage> msgs = new List<InboxMessage>();

        public RMCPacketResponseInboxMessageService_Method2()
        {
            msgs.Add(new InboxMessage());
        }

        public override byte[] ToBuffer()
        {
            MemoryStream m = new MemoryStream();
            Helper.WriteU32(m, (uint)msgs.Count);
            foreach (InboxMessage msg in msgs)
                msg.toBuffer(m);
            return m.ToArray();
        }

        public override string ToString()
        {
            return "[RMCPacketResponseInboxMessageService_Method2]";
        }
    }
}
