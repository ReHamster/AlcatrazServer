using DSFServices.DDL.Models;
using QNetZ;
using QNetZ.Attributes;
using QNetZ.DDL;
using QNetZ.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace DSFServices.Services
{
	/// <summary>
	/// Hermes 
	/// </summary>
	[RMCService(RMCProtocolId.MatchMakingService)]
	class MatchMakingService : RMCServiceBase
	{
		static uint GatheringIdCounter = 39000;
		static List<PartySessionGathering> GatheringList = new List<PartySessionGathering>();
		static List<SentInvitation> InvitationList = new List<SentInvitation>();

		[RMCMethod(1)]
		public RMCResult RegisterGathering(AnyData<HermesPartySession> anyGathering)
		{
			if(anyGathering.data != null)
			{
				var gathering = anyGathering.data;
				gathering.m_idMyself = ++GatheringIdCounter;
				GatheringList.Add(new PartySessionGathering(gathering));

				QLog.WriteLine(5, () => $"RegisterGathering : HermesPartySession:Gathering {DDLSerializer.ObjectToString(gathering)}");

				return Result(new { gatheringId = gathering.m_idMyself });
			}

			return Error((uint)RMCErrorCode.RendezVous_DDLMismatch);
		}

		[RMCMethod(2)]
		public RMCResult UnregisterGathering(uint idGathering)
		{
			bool result = false;
			var gathering = GatheringList.FirstOrDefault(x => x.Session.m_idMyself == idGathering);

			if(gathering != null)
			{
				// FIXME: are notifications sent?
				GatheringList.Remove(gathering);
				result = true;
			}

			return Result(new { retVal = result });
		}

		[RMCMethod(3)]
		public void UnregisterGatherings(ICollection<uint> lstGatherings)
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(4)]
		public RMCResult UpdateGathering(AnyData<HermesPartySession> anyGathering)
		{
			bool result = false;
			if (anyGathering.data != null)
			{

				QLog.WriteLine(5, () => $"UpdateGathering : HermesPartySession:Gathering {DDLSerializer.ObjectToString(anyGathering.data)}");

				var srcGathering = anyGathering.data;
				var gathering = GatheringList.FirstOrDefault(x => x.Session.m_idMyself == srcGathering.m_idMyself);

				if (gathering != null)
				{
					gathering.Session.m_pidOwner = srcGathering.m_pidOwner;
					gathering.Session.m_pidHost = srcGathering.m_pidHost;
					gathering.Session.m_uiMinParticipants = srcGathering.m_uiMinParticipants;
					gathering.Session.m_uiMaxParticipants = srcGathering.m_uiMaxParticipants;
					gathering.Session.m_uiParticipationPolicy = srcGathering.m_uiParticipationPolicy;
					gathering.Session.m_uiPolicyArgument = srcGathering.m_uiPolicyArgument;
					gathering.Session.m_uiFlags = srcGathering.m_uiFlags;
					gathering.Session.m_uiState = srcGathering.m_uiState;

					gathering.Session.m_strDescription = srcGathering.m_strDescription;
					gathering.Session.m_freePublicSlots = srcGathering.m_freePublicSlots;
					gathering.Session.m_freePrivateSlots = srcGathering.m_freePrivateSlots;
					gathering.Session.m_maxPrivateSlots = srcGathering.m_maxPrivateSlots;
					gathering.Session.m_privacySettings = srcGathering.m_privacySettings;
					gathering.Session.m_name = srcGathering.m_name;
					gathering.Session.m_buffurizedOwnerId = srcGathering.m_buffurizedOwnerId;

					// FIXME: are notifications sent?

					result = true;
				}

			}

			return Result(new { retVal = result });
		}

		[RMCMethod(5)]
		public RMCResult Invite(uint idGathering, ICollection<uint> lstPrincipals, string strMessage)
		{
			bool result = false;
			var gathering = GatheringList.FirstOrDefault(x => x.Session.m_idMyself == idGathering);

			if (gathering != null)
			{
				var invitations = lstPrincipals.Select(x => new SentInvitation
				{
					SentById = Context.Client.info.PID,
					GatheringId = idGathering,
					GuestId = x,
					Message = strMessage
				}).ToList();

				// remove old
				InvitationList.RemoveAll(x => invitations.Any(y => x.GatheringId == y.GatheringId && x.GuestId == y.GuestId));

				// add new
				InvitationList.AddRange(invitations);

				result = true;
			}

			return Result(new { retVal = result });
		}

		[RMCMethod(6)]
		public RMCResult AcceptInvitation(uint idGathering, string strMessage)
		{
			bool result = false;
			var gathering = GatheringList.FirstOrDefault(x => x.Session.m_idMyself == idGathering);
			var invitation = InvitationList.FirstOrDefault(x => x.GatheringId == idGathering && x.GuestId == Context.Client.info.PID);

			if (gathering != null && invitation != null)
			{
				// send notification to invitation sender
				var qsender = Context.Handler.GetQClientByClientPID(invitation.SentById);

				if(qsender != null)
				{
					// accepted invitation event
					var senderNotification = new NotificationEvent(NotificationEventsType.ParticipationEvent, 4)
					{
						m_pidSource = Context.Client.info.PID,
						m_uiParam1 = idGathering,
						m_uiParam2 = Context.Client.info.PID,
						m_strParam = strMessage,
						m_uiParam3 = 0
					};

					NotificationQueue.SendNotification(Context.Handler, qsender, senderNotification);
				}

				gathering.Participants.Add(Context.Client.info.PID);

				// send to all party members
				foreach (var pid in gathering.Participants)
				{
					var qclient = Context.Handler.GetQClientByClientPID(pid);

					if(qclient != null)
					{
						var notification = new NotificationEvent(NotificationEventsType.ParticipationEvent, 1)
						{
							m_pidSource = Context.Client.info.PID,
							m_uiParam1 = idGathering,
							m_uiParam2 = Context.Client.info.PID,
							m_strParam = strMessage,
							m_uiParam3 = 0
						};

						NotificationQueue.SendNotification(Context.Handler, qclient, notification);
					}
				}

				result = true;
			}

			return Result(new { retVal = result });
		}

		[RMCMethod(7)]
		public void DeclineInvitation()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(8)]
		public void CancelInvitation()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(9)]
		public RMCResult GetInvitationsSent(uint idGathering)
		{
			var myUserPid = Context.Client.info.PID;
			var list = InvitationList
				.Where(x => x.GatheringId == idGathering && x.SentById == myUserPid)
				.Select(x => new Invitation()
				{
					m_idGathering = x.GatheringId,
					m_idGuest = x.GuestId,
					m_strMessage = x.Message
				});
			return Result(list);
		}

		[RMCMethod(10)]
		public RMCResult GetInvitationsReceived()
		{
			var myUserPid = Context.Client.info.PID;
			var list = InvitationList
				.Where(x => x.GuestId == myUserPid)
				.Select(x => new Invitation()
			{
				m_idGathering = x.GatheringId,
				m_idGuest = x.GuestId,
				m_strMessage = x.Message
			});
			return Result(list);
		}

		[RMCMethod(11)]
		public RMCResult Participate(uint idGathering, string strMessage)
		{
			bool result = false;
			var gathering = GatheringList.FirstOrDefault(x => x.Session.m_idMyself == idGathering);

			if (gathering != null)
			{
				gathering.Participants.Add(Context.Client.info.PID);
				result = true;
			}

			return Result(new { retVal = result });
		}

		[RMCMethod(12)]
		public RMCResult CancelParticipation(uint idGathering, string strMessage)
		{
			bool result = false;
			var gathering = GatheringList.FirstOrDefault(x => x.Session.m_idMyself == idGathering);

			if (gathering != null)
			{
				gathering.Participants.Remove(Context.Client.info.PID);
				result = true;
			}

			return Result(new { retVal = result });
		}

		[RMCMethod(13)]
		public void GetParticipants()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(14)]
		public void AddParticipants()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(15)]
		public void GetDetailedParticipants()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(16)]
		public void GetParticipantsURLs()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(17)]
		public void FindByType()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(18)]
		public void FindByDescription()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(19)]
		public void FindByDescriptionRegex()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(20)]
		public void FindByID()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(21)]
		public RMCResult FindBySingleID(uint id)
		{
			bool result = false;
			var gathering = GatheringList.FirstOrDefault(x => x.Session.m_idMyself == id);

			if (gathering != null)
				result = true;
			else
				gathering = new PartySessionGathering();

			return Result(new { bResult = result, pGathering = new AnyData<HermesPartySession>(gathering.Session) });
		}

		[RMCMethod(22)]
		public void FindByOwner()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(23)]
		public void FindByParticipants()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(24)]
		public void FindInvitations()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(25)]
		public void FindBySQLQuery()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(26)]
		public void LaunchSession()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(27)]
		public void UpdateSessionURL()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(28)]
		public void GetSessionURL()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(29)]
		public void GetState()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(30)]
		public void SetState()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(31)]
		public void ReportStats()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(32)]
		public void GetStats()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(33)]
		public void DeleteGathering()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(34)]
		public void GetPendingDeletions()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(35)]
		public void DeleteFromDeletions()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(36)]
		public void MigrateGatheringOwnershipV1()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(37)]
		public void FindByDescriptionLike()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(38)]
		public RMCResult RegisterLocalURL(uint gid, StationURL url)
		{
			UNIMPLEMENTED($"uint gid = {gid}, string url = {url}");
			return Error(0);
		}

		[RMCMethod(39)]
		public RMCResult RegisterLocalURLs(uint gid, IEnumerable<StationURL> urls)
		{
			var gathering = GatheringList.FirstOrDefault(x => x.Session.m_idMyself == gid);

			if (gathering != null)
			{
				var newUrls = urls.Where(x => !gathering.Urls.Any(u => u.urlString == x.urlString));

				gathering.Urls.AddRange(newUrls);
			}

			return Error(0);
		}

		[RMCMethod(40)]
		public RMCResult UpdateSessionHostV1(uint gid)
		{
			return Error(0);
		}

		[RMCMethod(41)]
		public RMCResult GetSessionURLs(uint gid)
		{
			var gathering = GatheringList.FirstOrDefault(x => x.Session.m_idMyself == gid);

			if (gathering != null)
			{
				return Result(gathering.Urls);
			}

			return Error(0);
		}

		[RMCMethod(42)]
		public void UpdateSessionHost()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(43)]
		public void UpdateGatheringOwnership()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(44)]
		public void MigrateGatheringOwnership()
		{
			UNIMPLEMENTED();
		}
	}
}
