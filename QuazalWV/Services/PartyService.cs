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

		/*
		RMC Request  : True
		RMC Protocol : NotificationEventManager
		RMC Method   : 1
		Notification :
		Source           : 0x00084504
		Type             : 1004
		SubType          : 6
		Param 1          : 0x00030000
		Param 2          : 0xFFFFFFFF
		Param String     : 
		Param 3          : 0x00000000

		//-------------------------------------------------

		RMC Request  : True
		RMC Protocol : NotificationEventManager
		RMC Method   : 1
		Notification :
		Source           : 0x00084504
		Type             : 1004
		SubType          : 0
		Param 1          : 0x00005614
		Param 2          : 0x00000001
		Param String     : NetZHost:prudp:/address=192.168.1.211;port=3074;RVCID=759825|prudp:/address=92.46.131.79;port=3074;sid=15;type=3;RVCID=759825|
		Param 3          : 0x00000000

		 */

		[RMCMethod(14)]
		public RMCResult SendMatchmakingStatus(uint gid, uint pid)
		{
			UNIMPLEMENTED();

			var notification = new NotificationEvent(NotificationType.PartyEvent, 6)
			{
				m_pidSource = Context.Client.info.PID,
				m_uiParam1 = pid, // gid ???
				m_uiParam2 = 0xffffffff,
				m_strParam = "",
			};

			RMC.SendNotification(Context.Handler, Context.Client, notification);

			return Result(new { result = true });
		}

		[RMCMethod(15)]
		public RMCResult JoinMatchmakingStatus(uint gid, uint pid, bool joinSuccess)
		{
			var connStrings = $"NetZHost:{string.Join("|", Global.clientStationURLs)}|";

			var notification = new NotificationEvent(NotificationType.PartyEvent, 0)
			{
				m_pidSource = pid,
				m_uiParam1 = 0x5614, // gid ???
				m_uiParam2 = gid,
				m_strParam = connStrings,
			};

			RMC.SendNotification(Context.Handler, Context.Client, notification);

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
