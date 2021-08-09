using QNetZ;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace RDVServices
{
	public static class DBHelper
	{
		public static SQLiteConnection connection = new SQLiteConnection();

		public static void Init()
		{
			connection.ConnectionString = "Data Source=database.sqlite";
			connection.Open();
			Log.WriteLine(1, "DB loaded...");
		}

		public static void Close()
		{
			connection.Close();
		}

		public static List<List<string>> GetQueryResults(string query)
		{
			List<List<string>> result = new List<List<string>>();
			SQLiteCommand command = new SQLiteCommand(query, connection);
			SQLiteDataReader reader = command.ExecuteReader();
			while (reader.Read())
			{
				List<string> entry = new List<string>();
				for (int i = 0; i < reader.FieldCount; i++)
					entry.Add(reader[i].ToString());
				result.Add(entry);
			}
			reader.Close();
			reader.Dispose();
			command.Dispose();
			return result;
		}

		public static ClientInfo GetUserByName(string name)
		{
			ClientInfo result = null;
			var results = GetQueryResults("SELECT * FROM users WHERE name='" + name + "'");
			foreach (var entry in results)
			{
				result = new ClientInfo();
				result.PID = Convert.ToUInt32(entry[1]);
				result.name = entry[2];
				result.pass = entry[3];
			}
			return result;
		}

		public static ClientInfo GetUserByPID(uint PID)
		{
			ClientInfo result = null;
			var results = GetQueryResults("SELECT * FROM users WHERE PID='" + PID + "'");
			foreach (var entry in results)
			{
				result = new ClientInfo();
				result.PID = Convert.ToUInt32(entry[1]);
				result.name = entry[2];
				result.pass = entry[3];
			}
			return result;
		}
	}
}
