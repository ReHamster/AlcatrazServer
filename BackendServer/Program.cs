using DSFServices;
using Newtonsoft.Json;
using QNetZ;
using RDVServices;
using System;
using System.Drawing;
using System.IO;
using System.Threading;

namespace BackendServer
{
	class Configuration
	{
		public QConfiguration Services { get; set; }
		public bool PacketLogging { get; set; }
		public int LogLevel { get; set; }
	}

	class Program
	{
		static readonly string ConfigFileName = "./DSFServer.json";
		static bool cancelPressed = false;

		static void Main(string[] args)
		{
			Console.Title = "Alcatraz";
			Console.Clear();

			Console.WriteLine("Alcatraz backend starting...");

			// load config
			var configContents = File.ReadAllText(ConfigFileName);
			var config = JsonConvert.DeserializeObject<Configuration>(configContents);
			QConfiguration.Instance = config.Services;

			Console.CancelKeyPress += new ConsoleCancelEventHandler(cancelHandler);

			Log.enablePacketLogging = config.PacketLogging;
			Log.LogFunction = (int priority, string s, Color color) =>
			{
				if (priority > config.LogLevel)
					return;

				Console.ForegroundColor = (color.R == 255 && color.G == 0 && color.B == 0) ? ConsoleColor.Red : ConsoleColor.White;
				Console.WriteLine(s);
			};

			// register service
			ServiceFactoryDSF.RegisterDSFServices();

			// start UDP & HTTP listeners
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
		}

		protected static void cancelHandler(object sender, ConsoleCancelEventArgs args)
		{
			cancelPressed = true;
		}
	}
}
