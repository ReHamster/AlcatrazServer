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
			var plInfo = Context.Client.PlayerInfo;
			var presence = plInfo.GameData().CurrentPresence;

			QLog.WriteLine(2, $"Presence set to {phraseId}, {Convert.ToHexString(argument.data)}");

			if(presence == null)
			{
				presence = new PresenceElement();
				plInfo.GameData().CurrentPresence = presence;
			}

			presence.principalId = plInfo.PID;
			presence.phraseId = phraseId;
			presence.argument = argument;
			presence.isConnected = true;

			return Error(0);
		}

		[RMCMethod(2)]
		public RMCResult GetPresence(IEnumerable<uint> pids)
		{
			var presenceResult = new List<PresenceElement>();

			foreach(var principalId in pids)
			{
				var playerInfo = NetworkPlayers.GetPlayerInfoByPID(principalId);
				if (playerInfo != null && playerInfo.GameData().CurrentPresence != null)
				{
					presenceResult.Add(playerInfo.GameData().CurrentPresence);
				}
				else
				{
					presenceResult.Add(new PresenceElement()
					{
						phraseId = 2,
						isConnected = false,
						principalId = principalId,
						argument = new qBuffer()
					});
				}
			}

			return Result(presenceResult);
		}
	}
}
