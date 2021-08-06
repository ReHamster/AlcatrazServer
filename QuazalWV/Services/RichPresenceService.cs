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
		public RMCResult GetPresence(uint type)
		{
			var list = new List<PresenceElement>();
			return Result(list);
		}
	}
}
