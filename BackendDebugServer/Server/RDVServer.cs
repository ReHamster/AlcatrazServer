using System;
using System.IO;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading;
using QNetZ;

namespace BackendDebugServer
{
    public static class RDVServer
    {
        public static readonly object _sync = new object();
        public static bool _exit = false;
		private static UdpClient listener;
        public static ushort _skipNextNAT = 0xFFFF;
		static QPacketHandlerPRUDP packetHandler;

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
			var listenPort = QConfiguration.Instance.RDVServerPort;

			listener = new UdpClient(listenPort);
            IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0);
			packetHandler = new QPacketHandlerPRUDP(listener, BackendServicesServer.serverPID, QConfiguration.Instance.BackendServiceServerPort /* FIXME: this is not correct!*/, "RendezVous");

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
            Log.WriteLine(priority, "[RendezVous] " + s);
        }
    }
}
