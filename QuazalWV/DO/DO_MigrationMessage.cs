﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuazalWV
{
    public static class DO_MigrationMessage
    {
        public static byte[] HandlePacket(ClientInfo client, byte[] data)
        {
            Log.WriteLine(1, "[DO] Handling MigrationMessage...");
            return new byte[] { 0x11, 0x03, 0x00, 0x01, 0x00, 0xC0, 0x05, 0x04, 0x00, 0xC0, 0x05, 0x04, 0x00, 0xC0, 0x05, 0x03, 0x00, 0x00, 0x00, 0x00 };
        }
    }
}