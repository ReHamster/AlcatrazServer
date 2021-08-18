using QNetZ.Attributes;
using DSFServices.DDL.Models;
using QNetZ.Interfaces;
using System.Collections.Generic;
using QNetZ;
using System.Linq;
using System.Drawing;
using QNetZ.DDL;

namespace DSFServices.Services
{
	/// <summary>
	/// Hermes party service
	///		Additional layer to the Match making service AND a game session
	/// </summary>
	[RMCService(RMCProtocolId.PartyService)]
	class PartyService : RMCServiceBase
	{
		[RMCMethod(1)]
		public RMCResult SendGameIdToParty(uint id, uint toJoinId, byte gameType, string msgRequest)
		{
			var gathering = PartySessions.GatheringList.FirstOrDefault(x => x.Session.m_idMyself == id);

			if (gathering != null)
			{
				foreach(var pid in gathering.Participants)
				{
					var qclient = Context.Handler.GetQClientByClientPID(pid);

					if(qclient != null)
					{
						var notification = new NotificationEvent(NotificationEventsType.HermesPartySession, 0)
						{
							m_pidSource = Context.Client.Info.PID,
							m_uiParam1 = toJoinId,
							m_uiParam2 = gameType,
							m_strParam = $"NetZHost:{msgRequest}",
							m_uiParam3 = 0
						};

						NotificationQueue.SendNotification(Context.Handler, qclient, notification);
					}
				}
			}
			else
			{
				QLog.WriteLine(1, $"Error : PartyService.SendGameIdToParty - no gathering with gid={id}");
			}

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

			return Error(0);
		}

		[RMCMethod(4)]
		public RMCResult NotifyPartyToLeaveGame(uint id)
		{
			// in party id send to all clients
			var gathering = PartySessions.GatheringList.FirstOrDefault(x => x.Session.m_idMyself == id);

			if (gathering != null)
			{
				foreach (var pid in gathering.Participants)
				{
					var qclient = Context.Handler.GetQClientByClientPID(pid);

					if (qclient != null)
					{
						var notification = new NotificationEvent(NotificationEventsType.HermesPartySession, 2)
						{
							m_pidSource = Context.Client.Info.PID,
							m_uiParam1 = 0,
							m_uiParam2 = 0,
							m_strParam = "NotifyPartyToLeaveGame",
							m_uiParam3 = 0
						};

						NotificationQueue.SendNotification(Context.Handler, qclient, notification);
					}
				}
			}
			else
			{
				QLog.WriteLine(1, $"Error : PartyService.NotifyPartyToLeaveGame - no gathering with gid={id}");
			}

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
			var gathering = PartySessions.GatheringList.FirstOrDefault(x => x.Session.m_idMyself == partyId);

			if (gathering != null)
			{
				foreach (var pid in gathering.Participants)
				{
					var qclient = Context.Handler.GetQClientByClientPID(pid);

					if (qclient != null)
					{
						var notification = new NotificationEvent(NotificationEventsType.HermesPartySession, 4)
						{
							m_pidSource = Context.Client.Info.PID,
							m_uiParam1 = (uint)param1,
							m_uiParam2 = (uint)param2,
							m_strParam = "PartyLeaderNetZIsValid",
							m_uiParam3 = 0
						};

						NotificationQueue.SendNotification(Context.Handler, qclient, notification);
					}
				}
			}
			else
			{
				QLog.WriteLine(1, $"Error : PartyService.PartyLeaderNetZIsValid - no gathering with gid={partyId}");
			}

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
		public RMCResult SendMatchmakingStatus(uint gid, uint pid, uint gameType)
		{
			// Does pid == 0x30000 mean Search or Lobby?
			var gathering = PartySessions.GatheringList.FirstOrDefault(x => x.Session.m_idMyself == gid);

			if (gathering != null)
			{
				foreach (var participantPid in gathering.Participants)
				{
					var qclient = Context.Handler.GetQClientByClientPID(participantPid);

					if (qclient != null)
					{
						var notification = new NotificationEvent(NotificationEventsType.HermesPartySession, 6)
						{
							m_pidSource = Context.Client.Info.PID,
							m_uiParam1 = pid,
							m_uiParam2 = gameType,
							m_strParam = "",
							m_uiParam3 = 0
						};

						NotificationQueue.SendNotification(Context.Handler, qclient, notification);
					}
				}
			}
			else
			{
				QLog.WriteLine(1, $"Error : PartyService.SendMatchmakingStatus - no gathering with gid={gid}");
			}

			return Result(new { result = true });
		}

		[RMCMethod(15)]
		public RMCResult JoinMatchmakingStatus(uint gid, uint pid, bool joinSuccess)
		{
			var gathering = PartySessions.GatheringList.FirstOrDefault(x => x.Session.m_idMyself == gid);

			if (gathering != null)
			{
				foreach (var participantPid in gathering.Participants)
				{
					// don't send back the notification
					if (participantPid == Context.Client.Info.PID)
						continue;

					var qclient = Context.Handler.GetQClientByClientPID(participantPid);

					if (qclient != null)
					{
						NotificationEvent notification;
						notification = new NotificationEvent(NotificationEventsType.PartyJoinMatchmaking, 0)
						{
							m_pidSource = Context.Client.Info.PID,
							m_uiParam1 = pid,
							m_uiParam2 = (uint)(joinSuccess ? 1 : 0),
							m_strParam = "JoinMatchmakingStatus",
							m_uiParam3 = 0
						};

						NotificationQueue.SendNotification(Context.Handler, qclient, notification);
					}
				}
			}
			else
			{
				QLog.WriteLine(1, $"Error : PartyService.JoinMatchmakingStatus - no gathering with gid={gid}");
			}

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
			
		}
	}
}
