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
			DataStore = new Dictionary<Type, object>();
		}

		public QClient Client;	// connection info
		public uint PID { get; set; }		// printcipal ID
		public uint RVCID { get; set; }     // rendez-vous connection ID

		public uint StationID;
		public string AccountId;
		public string Name { get; set; }

		// game - specific stuff comes here
		public T GetData<T>() where T: class
		{
			object value;

			if(DataStore.TryGetValue(typeof(T), out value))
				return (T)value;

			var createFunc = Expression.Lambda<Func<T>>(
				Expression.New(typeof(T).GetConstructor(Type.EmptyTypes))
			).Compile();

			DataStore[typeof(T)] = value = createFunc();

			return (T)value;
		}

		private Dictionary<Type, object> DataStore;
	}
}
