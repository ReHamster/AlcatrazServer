using QuazalWV.Attributes;
using QuazalWV.DDL.Models;
using QuazalWV.Interfaces;
using System.Collections.Generic;

namespace QuazalWV.Services
{
	[RMCService(RMCProtocolId.PartyService)]
	class PartyService : RMCServiceBase
	{
		[RMCMethod(1)]
		public RMCResult SendGameIdToParty(uint id, uint toJoinId, int gameType, string msgRequest)
		{
			UNIMPLEMENTED($"uint id = {id}, uint toJoinId = {toJoinId}, int gameType = {gameType}, string msgRequest = {msgRequest}");
			return Error(0);
		}

		[RMCMethod(2)]
		public void SendGameIdToPlayerByName(string playerName, uint toJoinId, int gameType, string msgRequest)
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(3)]
		public void SendGameIdToPlayerByID(uint pid, uint toJoinId, int gameType, string msgRequest)
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(4)]
		public RMCResult NotifyPartyToLeaveGame(uint id)
		{
			UNIMPLEMENTED();
			return Error(0);
		}

		[RMCMethod(5)]
		public void NotifyPlayerToLeavePartyByName(string playerName)
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(6)]
		public void NotifyPlayerToLeavePartyByID(uint id)
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(7)]
		public void SendNewPartyIdToParty(uint oldParty, uint newParty)
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(8)]
		public RMCResult ShouldBecomeNewPartyHost(uint pid, uint partyId)
		{
			UNIMPLEMENTED();

			// newHostId = pid

			return Result(new { newHostId = 1u });
		}

		[RMCMethod(9)]
		public void PartyLeaderNetZIsValid(uint partyId, int param1, int param2)
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(10)]
		public void QueryMatchmaking(uint toMatchmaking, uint fromParty, int nbPlayers, int applyMask)
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(11)]
		public void ResponseMatchmaking(uint toParty, uint fromMatchmaking, int approved)
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(12)]
		public void PartyProbeSessions(uint gid, uint pid, string packedSessions)
		{

			UNIMPLEMENTED();
		}

		[RMCMethod(13)]
		public void PartyProbeSessionResults(uint gid, uint pid, string packedSessions)
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(14)]
		public RMCResult SendMatchmakingStatus(uint gid, uint pid)
		{
			UNIMPLEMENTED();
			return Result(new { result = true });
		}

		[RMCMethod(15)]
		public RMCResult JoinMatchmakingStatus(uint gid, uint pid, bool joinSuccess)
		{
			UNIMPLEMENTED();
			return Result(new { result = true });
		}

		[RMCMethod(16)]
		public RMCResult PlayerShouldLeave(uint gid, uint pid)
		{
			UNIMPLEMENTED();
			return Result(new { result = false });
		}

		[RMCMethod(17)]
		public RMCResult IsPartyMemberActive(uint gid, uint pid)
		{
			UNIMPLEMENTED();
			return Result(new { isMemberActive = true });
		}

		[RMCMethod(18)]
		public void MigrateTo(uint pid, uint oldGathering, uint newGathering)
		{
			UNIMPLEMENTED();
		}
	}
}
