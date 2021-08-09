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
	class Program
	{
		static readonly string ConfigFileName = "./DSFServer.json";
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

			// load config
			var configContents = File.ReadAllText("./DSFServer.json");
			QConfiguration.Instance = JsonConvert.DeserializeObject<QConfiguration>(configContents);

			// register service
			ServiceFactoryDSF.RegisterDSFServices();

			// start DB
			DBHelper.Init();

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
			DBHelper.Close();
		}

		protected static void cancelHandler(object sender, ConsoleCancelEventArgs args)
		{
			cancelPressed = true;
		}
	}
}
