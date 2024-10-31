using QNetZ.DDL;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;

namespace QNetZ
{
	public class PlayerInfo
	{
		public PlayerInfo()
		{
			DataStore = new Dictionary<Type, IPlayerDataStore>();
		}

		public void OnDropped()
		{
			if (Client != null)
				Client.PlayerInfo = null;

			foreach (var ds in DataStore.Values)
				ds.OnDropped();
		}

		public QClient Client;	// connection info
		public uint PID { get; set; }		// printcipal ID
		public uint RVCID { get; set; }     // rendez-vous connection ID

		public uint StationID;
		public string AccountId;
		public string Name { get; set; }

		public StationURL Url
		{
			get
			{
				if (Client == null)
					return null;

				return new StationURL(
					"prudp",
					Client.Endpoint.Address.ToString(),
					new Dictionary<string, int>() {
						{ "port", Client.Endpoint.Port },
						{ "RVCID", (int)RVCID },
						//{ "type", 3 }	// TODO: IsPublic and Behind NAT flags
					});
			}
		}
		// game - specific stuff comes here
		public T GetData<T>() where T: class
		{
			IPlayerDataStore value;

			if(DataStore.TryGetValue(typeof(T), out value))
				return (T)value;

			var createFunc = Expression.Lambda<Func<T>>(
				Expression.New(typeof(T).GetConstructor(new[] { typeof(PlayerInfo) }), new [] { Expression.Constant(this) })
			).Compile();

			DataStore[typeof(T)] = value = (IPlayerDataStore)createFunc();

			return (T)value;
		}

		private Dictionary<Type, IPlayerDataStore> DataStore;
	}

	public interface IPlayerDataStore
	{
		void OnDropped();
	}
}
