using System;
using System.IO;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading;
using QNetZ;
using System.Threading.Tasks;

namespace BackendServer
{
	public static class RDVServer
	{
		public static readonly object _sync = new object();
		public static bool _exit = false;
		private static UdpClient listener;
		public static ushort _skipNextNAT = 0xFFFF;
		static QPacketHandlerPRUDP packetHandler;

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
			var listenPort = QConfiguration.Instance.RDVServerPort;

			listener = new UdpClient(listenPort);
			packetHandler = new QPacketHandlerPRUDP(listener, BackendServicesServer.serverPID, listenPort, "RendezVous");

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
						if (CurrentRecvTask.IsCompleted)
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

		private static void WriteLog(int priority, string s)
		{
			Log.WriteLine(priority, "[RendezVous] " + s);
		}
	}
}
