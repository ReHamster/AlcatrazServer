﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRPBackendWV
{
    public class RMCPacketRequestTelemetry_Method1 : RMCPRequest
    {
        public string unk1;
        public string unk2;
        public string unk3;
        public uint unk4;

        public RMCPacketRequestTelemetry_Method1(Stream s)
        {
            unk1 = Helper.ReadString(s);
            unk2 = Helper.ReadString(s);
            unk3 = Helper.ReadString(s);
            unk4 = Helper.ReadU32(s);
        }

        public override byte[] ToBuffer()
        {
            MemoryStream result = new MemoryStream();
            Helper.WriteString(result, unk1);
            Helper.WriteString(result, unk2);
            Helper.WriteString(result, unk3);
            Helper.WriteU32(result, unk4);
            return result.ToArray();
        }

        public override string ToString()
        {
            return "[RMC Packet Request Telemetry_Method1]";
        }

        public override string PayloadToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("\t[Unknown 1 : " + unk1 + "]");
            sb.AppendLine("\t[Unknown 2 : " + unk2 + "]");
            sb.AppendLine("\t[Unknown 3 : " + unk3 + "]");
            sb.AppendLine("\t[Unknown 4 : 0x" + unk4.ToString("X8") + "]");
            return sb.ToString();
        }
    }
}
