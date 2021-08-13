using QNetZ.Attributes;
using DSFServices.DDL.Models;
using QNetZ.Interfaces;
using System.Collections.Generic;
using QNetZ;

namespace DSFServices.Services
{
	[RMCService(RMCProtocolId.PartyService)]
	class PartyService : RMCServiceBase
	{
		[RMCMethod(1)]
		public RMCResult SendGameIdToParty(uint id, uint toJoinId, byte gameType, string msgRequest)
		{
			UNIMPLEMENTED($"uint id = {id}, uint toJoinId = {toJoinId}, int gameType = {gameType}, string msgRequest = {msgRequest}");

			/*
			 
			 {
				"id": 39704,
				  "toJoinId": 22046,		// GameSession ID
				  "gameType": 1879078401,		// flags?		0x70007601
				  "msgRequest": QUAZAL PRUDP URLS
			}
			 */

			/*
			 SEND TO ALL CIENTS 
			NotificationEvent {
				m_pidSource = 541956			// Client PID ? sPID?
				m_uiType = (PartyEvent, 0)
				m_uiParam1 = 22046				// toJoinId
				m_uiParam2 = 1					// hmmmm
				m_strParam = QUAZAL PRUDP URLS in msgRequest
			}
			 */

			var notification = new NotificationEvent(NotificationEventsType.HermesPartySession, 0)
			{
				m_pidSource = Context.Client.Info.PID,
				m_uiParam1 = toJoinId,
				m_uiParam2 = gameType,
				m_strParam = $"NetZHost:{msgRequest}",
				m_uiParam3 = 0
			};

			NotificationQueue.SendNotification(Context.Handler, Context.Client, notification);
			//NotificationQueue.AddNotification(notification, Context.Client, 500);


			return Error(0);
		}

		[RMCMethod(2)]
		public void SendGameIdToPlayerByName(string playerName, uint toJoinId, byte gameType, string msgRequest)
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(3)]
		public RMCResult SendGameIdToPlayerByID(uint pid, uint toJoinId, byte gameType, string msgRequest)
		{
			// send to single client with PID only
			var qclient = Context.Handler.GetQClientByClientPID(pid);

			if(qclient != null)
			{
				var notification = new NotificationEvent(NotificationEventsType.HermesPartySession, 1)
				{
					m_pidSource = Context.Client.Info.PID,
					m_uiParam1 = toJoinId,
					m_uiParam2 = gameType,
					m_strParam = $"NetZHost:{msgRequest}",
					m_uiParam3 = 0
				};

				NotificationQueue.SendNotification(Context.Handler, qclient, notification);
			}

			UNIMPLEMENTED();
			return Error(0);
		}

		[RMCMethod(4)]
		public RMCResult NotifyPartyToLeaveGame(uint id)
		{
			// in party id send to all clients
			/*
			 NotificationEvent {
				m_pidSource = 541956
				m_uiType = (PartyEvent, 2)
				m_uiParam1 = 0
				m_uiParam2 = 0
				m_strParam = NotifyPartyToLeaveGame
			}

			 */

			var notification = new NotificationEvent(NotificationEventsType.HermesPartySession, 2)
			{
				m_pidSource = Context.Client.Info.PID,
				m_uiParam1 = 0,
				m_uiParam2 = 0,
				m_strParam = "NotifyPartyToLeaveGame",
				m_uiParam3 = 0
			};

			NotificationQueue.SendNotification(Context.Handler, Context.Client, notification);

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
		public RMCResult PartyLeaderNetZIsValid(uint partyId, int param1, int param2)
		{
			var notification = new NotificationEvent(NotificationEventsType.HermesPartySession, 4)
			{
				m_pidSource = Context.Client.Info.PID,
				m_uiParam1 = (uint)param1,
				m_uiParam2 = (uint)param2,
				m_strParam = "PartyLeaderNetZIsValid",
				m_uiParam3 = 0
			};

			NotificationQueue.SendNotification(Context.Handler, Context.Client, notification);
			/*
			SEND TO ALL in partyId
			NotificationEvent {
				m_pidSource = 541956
				m_uiType = (PartyEvent, 4)
				m_uiParam1 = 0
				m_uiParam2 = 0
				m_strParam = "PartyLeaderNetZIsValid"
			}*/


			UNIMPLEMENTED();
			return Error(0);
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
			/*
			 {
			  "gid": 39704,		// gathering ID
			  "pid": 196608		// 0x30000
			}
			 */

			/*
			SEND TO ALL:
			
			NotificationEvent {
				m_pidSource = 541956		// Context.Client.info.PID? Context.Client.info.sPID?
				m_uiType = (PartyEvent, 6)
				m_uiParam1 = 196608	 		// 0x30000 = pid
				m_uiParam2 = -1
				m_strParam = 
			}
			 */

			UNIMPLEMENTED();

			// TODO: search all clients in "gid" and send them notifications

			var notification = new NotificationEvent(NotificationEventsType.HermesPartySession, 6)
			{
				m_pidSource = Context.Client.Info.PID,
				m_uiParam1 = pid,
				m_uiParam2 = 0xffffffff,
				m_strParam = "",
				m_uiParam3 = 0
			};

			NotificationQueue.SendNotification(Context.Handler, Context.Client, notification);

			//NotificationQueue.AddNotification(notification, Context.Client, 500);

			return Result(new { result = true });
		}

		[RMCMethod(15)]
		public RMCResult JoinMatchmakingStatus(uint gid, uint pid, bool joinSuccess)
		{
			/*
				{
				  "gid": 39704,				// gathering ID
				  "pid": 541956,			// 0x84504
				  "joinSuccess": true
				}

			 */

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
