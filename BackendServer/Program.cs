using DSFServices;
using QNetZ;
using RDVServices;
using System;
using System.Drawing;
using System.Threading;

namespace BackendServer
{
	class Program
	{
		static bool cancelPressed = false;

		static void Main(string[] args)
		{
			Console.Title = "Alcatraz";
			Console.Clear();

			Console.WriteLine("Alcatraz backend starting...");

			Console.CancelKeyPress += new ConsoleCancelEventHandler(cancelHandler);

			Log.LogFunction = (int priority, string s, Color color) =>
			{
				if (priority > 2)
					return;

				Console.ForegroundColor = (color.R == 255 && color.G == 0 && color.B == 0) ? ConsoleColor.Red : ConsoleColor.White;
				Console.WriteLine(s);
			};

			ServiceFactoryDSF.RegisterDSFServices();
			QConfiguration.Instance = new QConfiguration()
			{
				ServerBindAddress = "37.77.104.232",
				RDVServerPort = 21005,
				BackendServiceServerPort = 21006,
				ServerFilesPath = "/root/BackendServer/serverFiles/",
				SandboxAccessKey = "8dtRv2oj"            // Server access key. Affects packet checksum;
			};

			DBHelper.Init();

			TCPServer.Start();
			BackendServicesServer.Start();
			RDVServer.Start();

			while (!cancelPressed)
			{
				Thread.Sleep(10);
			}

			Console.WriteLine("Stopping server...");

			TCPServer.Stop();
			BackendServicesServer.Stop();
			RDVServer.Stop();
			DBHelper.Close();
		}

		protected static void cancelHandler(object sender, ConsoleCancelEventArgs args)
		{
			cancelPressed = true;
		}
	}
}
