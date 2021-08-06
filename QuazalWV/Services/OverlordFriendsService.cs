using QuazalWV.Attributes;
using QuazalWV.Interfaces;
using System.Collections.Generic;

namespace QuazalWV.Services
{
	[RMCService(RMCProtocolId.OverlordFriendsService)]
	public class OverlordFriendsService : RMCServiceBase
	{
		[RMCMethod(1)]
		public RMCResult SyncFriends(uint friendType)
		{
			var list = new List<string>();
			return Result(list);
		}
	}
}
