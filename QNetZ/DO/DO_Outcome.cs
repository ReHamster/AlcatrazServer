﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QNetZ
{
    public static class DO_Outcome
    {
        public static byte[] HandleMessage(QClient client, byte[] data)
        {
            QLog.WriteLine(2, "[DO] Received Called Outcome 0x" + BitConverter.ToUInt32(data, 3).ToString("X") + " for call ID 0x" + BitConverter.ToUInt16(data, 1).ToString("X"));
            return null;
        }
        public static byte[] Create(ushort callID, uint outcome)
        {
            QLog.WriteLine(2, "[DO] Creating DO_OutcomeMessage");
            MemoryStream m = new MemoryStream();
            m.WriteByte(0x8);
            Helper.WriteU16(m, callID);
            Helper.WriteU32(m, outcome);
            return m.ToArray();
        }
    }
}
