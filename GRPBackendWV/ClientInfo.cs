﻿using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRPBackendWV
{
    public class ClientInfo
    {
        public uint PID;
        public uint IDrecv;
        public uint IDsend;
        public byte sessionID;
        public byte[] sessionKey;
        public ushort seqCounter;
        public uint callCounter;
        public string name;
        public string pass;
        public IPEndPoint ep;
        public UdpClient udp;
    }
}
