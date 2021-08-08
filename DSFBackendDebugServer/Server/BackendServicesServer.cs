using System;
using System.IO;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading;
using QuazalWV;

namespace GROBackendWV
{
    public static class BackendServicesServer
    {
        public static readonly uint serverPID = 2;
        public static readonly object _sync = new object();
        public static bool _exit = false;
        public static ushort listenPort = Global.BackendServiceServerPort;
        public static UdpClient listener;
        public static ushort _skipNextNAT = 0xFFFF;
		public static QPacketHandlerPRUDP packetHandler;


		public static void Start()
        {
            _exit = false;
            new Thread(tMainThread).Start();
        }

        public static void Stop()
        {
            lock (_sync)
            {
                _exit = true;
            }
            if (listener != null)
                listener.Close();
        }

        public static void tMainThread(object obj)
        {
            listener = new UdpClient(listenPort);
            IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0);

			packetHandler = new QPacketHandlerPRUDP(listener, serverPID, listenPort, "BackendServices");

			WriteLog(1, $"Server started at port { listenPort }");

			while (true)
            {
                lock (_sync)
                {
                    if (_exit)
                        break;
                }
                try
                {
                    byte[] bytes = listener.Receive(ref ep);
                    ProcessPacket(bytes, ep);
                }
                catch (Exception ex)
                {
                    WriteLog(1, "error - exception occured! " + ex.Message);
                }
            }
            WriteLog(1, "Server stopped");
        }

        public static void ProcessPacket(byte[] data, IPEndPoint ep)
        {
			packetHandler.ProcessPacket(data, ep);
        }

        private static void WriteLog(int priority, string s)
        {
            Log.WriteLine(priority, "[BackendServices] " + s);
        }
    }
}
