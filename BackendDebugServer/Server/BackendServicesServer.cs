using QNetZ;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace BackendDebugServer
{
	public static class BackendServicesServer
    {
        public static readonly uint serverPID = 2;
        public static readonly object _sync = new object();
        public static bool _exit = false;
        public static UdpClient listener;
        public static ushort _skipNextNAT = 0xFFFF;
		public static QPacketHandlerPRUDP packetHandler;

		static Task<UdpReceiveResult> CurrentRecvTask = null;

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
        }

        public static void tMainThread(object obj)
        {
			var listenPort = QConfiguration.Instance.BackendServiceServerPort;

			listener = new UdpClient(listenPort);
			packetHandler = new QPacketHandlerPRUDP(listener, serverPID, listenPort, "BackendServices");

			WriteLog(1, $"Listening at port { listenPort }");

			while (true)
            {
                lock (_sync)
                {
                    if (_exit)
                        break;
                }

                try
                {
					// use non-blocking recieve
					if (CurrentRecvTask != null)
					{
						if(CurrentRecvTask.IsCompleted)
						{
							var result = CurrentRecvTask.Result;
							CurrentRecvTask = null;
							packetHandler.ProcessPacket(result.Buffer, result.RemoteEndPoint);
						}
						else if (CurrentRecvTask.IsCanceled || CurrentRecvTask.IsFaulted)
						{
							CurrentRecvTask = null;
						}
					}

					if (CurrentRecvTask == null)
						CurrentRecvTask = listener.ReceiveAsync();

					Thread.Sleep(1);
				}
                catch (Exception ex)
                {
                    WriteLog(1, "error - exception occured! " + ex.Message);
                }
            }
            WriteLog(1, "Server stopped");

			CurrentRecvTask = null;
			listener.Close();
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
