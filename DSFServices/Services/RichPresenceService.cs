using QNetZ.Attributes;
using QNetZ.Interfaces;
using System.Collections.Generic;
using DSFServices.DDL.Models;
using QNetZ.DDL;
using QNetZ;
using System.Linq;
using System.IO;
using System;

namespace DSFServices.Services
{
	[RMCService(RMCProtocolId.RichPresenceService)]
	public class RichPresenceService : RMCServiceBase
	{
		[RMCMethod(1)]
		public RMCResult SetPresence(int phraseId, qBuffer argument)
		{
			var plInfo = Context.Client.Info;
			var existingPresence = plInfo.GameData().Presence.FirstOrDefault(x => x.phraseId == phraseId && x.principalId == plInfo.PID);

			if(existingPresence != null)
			{
				existingPresence.argument = argument;
				existingPresence.isConnected = true;
			}
			else
			{
				plInfo.GameData().Presence.Add(new PresenceElement()
				{
					isConnected = true,
					phraseId = phraseId,
					principalId = plInfo.PID,
					argument = argument,
				});
			}

			return Error(0);
		}

		[RMCMethod(2)]
		public RMCResult GetPresence(IEnumerable<uint> pids)
		{
			var players = pids.Select(x => new Tuple<uint, PlayerInfo>(x, NetworkPlayers.GetPlayerInfoByPID(x)));

			var presenceResult = new List<PresenceElement>();

			foreach(var tuple in players)
			{
				if(tuple.Item2 != null)
				{
					presenceResult.AddRange(tuple.Item2.GameData().Presence);
				}
				else
				{
					presenceResult.Add(new PresenceElement()
					{
						phraseId = 11,
						isConnected = false,
						principalId = tuple.Item1,
						argument = new qBuffer()
					});
				}
			}

			return Result(presenceResult);
		}
	}
}
