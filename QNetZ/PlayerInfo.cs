﻿using System;
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
			foreach (var ds in DataStore.Values)
				ds.OnDropped();
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
