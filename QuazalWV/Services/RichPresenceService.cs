using QuazalWV.Attributes;
using QuazalWV.Interfaces;
using QuazalWV.DDL.Models;
using System.Collections.Generic;

namespace QuazalWV.Services
{
	[RMCService(RMCProtocolId.RichPresenceService)]
	public class RichPresenceService : RMCServiceBase
	{
		[RMCMethod(1)]
		public RMCResult SetPresence(uint type)
		{
			UNIMPLEMENTED();
			return Error(0);
		}

		[RMCMethod(2)]
		public RMCResult GetPresence(uint type)
		{
			// ret: 0A 00 00 00 6D 01 1A 00 00 00 01 80 00 00 
			UNIMPLEMENTED();
			var list = new List<PresenceElement>();
			return Result(list);
		}
	}
}
