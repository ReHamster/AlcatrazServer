using QNetZ;
using QNetZ.Attributes;
using QNetZ.Interfaces;
using System.Collections.Generic;

namespace DSFServices.Services
{
	[RMCService(RMCProtocolId.OverlordFriendsService)]
	public class OverlordFriendsService : RMCServiceBase
	{
		[RMCMethod(1)]
		public RMCResult SyncFriends(uint friendType, IEnumerable<string> friends)
		{
			var list = new List<string>();
			return Result(list);
		}
	}
}
