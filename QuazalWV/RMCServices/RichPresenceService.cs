using QuazalWV.Attributes;
using QuazalWV.Interfaces;
using System.Collections.Generic;

namespace QuazalWV.RMCServices
{
	[RMCService(RMCP.PROTOCOL.RichPresenceService)]
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
