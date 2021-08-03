using QuazalWV.Attributes;
using QuazalWV.Interfaces;

namespace QuazalWV.RMCServices
{
	/// <summary>
	/// User friends service
	/// </summary>
	[RMCService(RMCP.PROTOCOL.FriendsService)]
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
        public void GetDetailedList()
        {
            UNIMPLEMENTED();
        }

        [RMCMethod(13)]
        public RMCResult GetRelationships(int offset, int size)
        {
            var result = new GetRelationshipsResult();

            return Result(result);
        }
    }
}
