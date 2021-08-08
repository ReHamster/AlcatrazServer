using QuazalWV.Attributes;
using QuazalWV.Interfaces;
using QuazalWV.DDL.Models;
using System.Collections.Generic;
using QuazalWV.DDL;

namespace QuazalWV.Services
{
	[RMCService(RMCProtocolId.RichPresenceService)]
	public class RichPresenceService : RMCServiceBase
	{
		private static List<PresenceElement> presences;

		[RMCMethod(1)]
		public RMCResult SetPresence(int phraseId, qBuffer argument)
		{
			/*
			presences.Add(new PresenceElement()
			{
				isConnected = true,
				phraseId = phraseId,
				principalId = Context.Client.info.PID,
				argument = argument.data,
			});*/
			return Error(0);
		}

		[RMCMethod(2)]
		public RMCResult GetPresence(IEnumerable<uint> pids)
		{
			// ret: 0A 00 00 00 6D 01 1A 00 00 00 01 80 00 00 
			UNIMPLEMENTED();
			var list = new List<PresenceElement>();
			return Result(list);
		}
	}
}
