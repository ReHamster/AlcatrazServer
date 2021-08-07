using QuazalWV.Attributes;
using QuazalWV.Interfaces;
using System.Collections.Generic;

namespace QuazalWV.Services
{
	/// <summary>
	/// User friends service
	/// </summary>
	[RMCService(RMCProtocolId.FriendsService)]
	public class FriendsService : RMCServiceBase
	{
		[RMCMethod(1)]
		public void AddFriend()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(2)]
		public void AddFriendByName()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(3)]
		public void AddFriendWithDetails()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(4)]
		public void AddFriendByNameWithDetails()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(5)]
		public void AcceptFriendship()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(6)]
		public void DeclineFriendship()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(7)]
		public void BlackList()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(8)]
		public void BlackListByName()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(9)]
		public void ClearRelationship()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(10)]
		public void UpdateDetails()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(11)]
		public void GetList()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(12)]
		public RMCResult GetDetailedList(byte byRelationship, bool bReversed)
		{
			var result = new List<FriendData>();

			return Result(result);
		}

		[RMCMethod(13)]
		public RMCResult GetRelationships(int offset, int size)
		{
			// 20 01 00 00 14 01 17 00 00 00 0D 80 00 00 0B 00 00 00 0B 00 00 00 47 BD 05 00 0E 00 56 6F 72 74 65 78 4C 65 42 65 6C 67 65 00 01 00 00 00 00 00 AB D6 05 00 09 00 6D 63 6E 61 6C 6C 79 6F 00 01 00 00 00 00 00 E9 3B 08 00 0C 00 53 6E 6F 6F 70 79 42 6C 61 6E 6B 00 01 00 00 00 00 00 22 3D 08 00 0B 00 7A 75 63 6B 69 6C 6F 61 6B 73 00 01 00 00 00 00 00 B5 43 08 00 0F 00 67 6F 6C 64 65 6E 5F 73 6C 65 6E 64 65 72 00 01 00 00 00 00 00 23 44 08 00 0C 00 4E 69 6B 6B 69 43 68 61 6E 39 32 00 01 00 00 00 00 00 D0 45 08 00 0C 00 61 64 72 69 61 61 6E 39 31 30 30 00 01 00 00 00 00 00 2B 47 08 00 0E 00 52 61 63 69 6E 67 46 72 65 61 6B 39 35 00 01 00 00 00 00 00 8C 4D 08 00 0E 00 56 65 72 79 48 6F 74 50 65 72 73 6F 6E 00 01 00 00 00 00 00 F3 4D 08 00 0C 00 56 6F 72 74 65 78 53 74 6F 72 65 00 01 00 00 00 00 00 97 4F 08 00 0D 00 53 49 44 45 53 57 49 50 45 31 32 37 00 01 00 00 00 00 00 
			var result = new RelationshipsResult();

			return Result(result);
		}
	}
}
