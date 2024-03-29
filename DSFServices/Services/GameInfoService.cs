﻿using DSFServices.DDL.Models;
using QNetZ;
using QNetZ.Attributes;
using QNetZ.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DSFServices.Services
{
	/// <summary>
	/// Hermes Game Info protocol
	/// </summary>
	[RMCService(RMCProtocolId.GameInfoService)]
	class GameInfoService : RMCServiceBase
	{
		// files which server can renturn
		private static string[] FileList = {
			"OnlineConfig.ini"
		};

		[RMCMethod(5)]
		public RMCResult GetFileInfoList(int indexStart, int numElements, string stringSearch)
		{
			var fileList = new List<PersistentInfo>();

			foreach (var name in FileList.Skip(indexStart).Take(numElements))
			{
				var path = Path.Combine(QConfiguration.Instance.ServerFilesPath, name);

				if (!File.Exists(path))
					continue;

				var fi = new FileInfo(path);

				fileList.Add(new PersistentInfo
				{
					m_name = name,
					m_size = (uint)fi.Length
				});
			}

			return Result(fileList);
		}
	}
}
