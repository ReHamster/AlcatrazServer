using DSFServices.DDL.Models;
using QNetZ;
using QNetZ.Attributes;
using QNetZ.DDL;
using QNetZ.Interfaces;
using System.Collections.Generic;

namespace DSFServices.Services
{
	/// <summary>
	/// Hermes 
	/// </summary>
	[RMCService(RMCProtocolId.MatchMakingService)]
	class MatchMakingService : RMCServiceBase
	{
		[RMCMethod(1)]
		public RMCResult RegisterGathering(AnyData<HermesPartySession> anyGathering)
		{
			uint result = 39704;
			if(anyGathering.data != null)
			{
				Log.WriteLine(1, "RegisterGathering : HermesPartySession:Gathering {");
				Log.WriteLine(1, $"    m_freePublicSlots = {anyGathering.data.m_freePublicSlots }");
				Log.WriteLine(1, $"    m_freePrivateSlots = { anyGathering.data.m_freePrivateSlots }");
				Log.WriteLine(1, $"    m_maxPrivateSlots = {anyGathering.data. m_maxPrivateSlots }");
				Log.WriteLine(1, $"    m_privacySettings = { anyGathering.data.m_privacySettings }");
				Log.WriteLine(1, $"    m_name = { anyGathering.data.m_name }");
				Log.WriteLine(1, $"    m_buffurizedOwnerId = { anyGathering.data.m_buffurizedOwnerId }");
				Log.WriteLine(1, $"    base.m_idMyself = {anyGathering.data.m_idMyself}");
				Log.WriteLine(1, $"    base.m_pidOwner = {anyGathering.data.m_pidOwner}");
				Log.WriteLine(1, $"    base.m_pidHost = {anyGathering.data.m_pidHost}");
				Log.WriteLine(1, $"    base.m_uiMinParticipants = {anyGathering.data.m_uiMinParticipants}");
				Log.WriteLine(1, $"    base.m_uiMaxParticipants = {anyGathering.data.m_uiMaxParticipants}");
				Log.WriteLine(1, $"    base.m_uiParticipationPolicy = {anyGathering.data.m_uiParticipationPolicy}");
				Log.WriteLine(1, $"    base.m_uiPolicyArgument = {anyGathering.data.m_uiPolicyArgument}");
				Log.WriteLine(1, $"    base.m_uiFlags = {anyGathering.data.m_uiFlags}");
				Log.WriteLine(1, $"    base.m_uiState = {anyGathering.data.m_uiState}");
				Log.WriteLine(1, $"    base.m_strDescription = {anyGathering.data.m_strDescription}");
				Log.WriteLine(1, "}");
			}

			// return 39704

			UNIMPLEMENTED();

			return Result(new { gatheringId = result });
		}

		[RMCMethod(2)]
		public void UnregisterGathering()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(3)]
		public void UnregisterGatherings()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(4)]
		public RMCResult UpdateGathering(AnyData<HermesPartySession> anyGathering)
		{
			//m_idMyself = gathering ID 39704

			if (anyGathering.data != null)
			{
				Log.WriteLine(1, "UpdateGathering : HermesPartySession:Gathering {");
				Log.WriteLine(1, $"    m_freePublicSlots = {anyGathering.data.m_freePublicSlots }");
				Log.WriteLine(1, $"    m_freePrivateSlots = { anyGathering.data.m_freePrivateSlots }");
				Log.WriteLine(1, $"    m_maxPrivateSlots = {anyGathering.data.m_maxPrivateSlots }");
				Log.WriteLine(1, $"    m_privacySettings = { anyGathering.data.m_privacySettings }");
				Log.WriteLine(1, $"    m_name = { anyGathering.data.m_name }");
				Log.WriteLine(1, $"    m_buffurizedOwnerId = { anyGathering.data.m_buffurizedOwnerId }");
				Log.WriteLine(1, $"    base.m_idMyself = {anyGathering.data.m_idMyself}");
				Log.WriteLine(1, $"    base.m_pidOwner = {anyGathering.data.m_pidOwner}");
				Log.WriteLine(1, $"    base.m_pidHost = {anyGathering.data.m_pidHost}");
				Log.WriteLine(1, $"    base.m_uiMinParticipants = {anyGathering.data.m_uiMinParticipants}");
				Log.WriteLine(1, $"    base.m_uiMaxParticipants = {anyGathering.data.m_uiMaxParticipants}");
				Log.WriteLine(1, $"    base.m_uiParticipationPolicy = {anyGathering.data.m_uiParticipationPolicy}");
				Log.WriteLine(1, $"    base.m_uiPolicyArgument = {anyGathering.data.m_uiPolicyArgument}");
				Log.WriteLine(1, $"    base.m_uiFlags = {anyGathering.data.m_uiFlags}");
				Log.WriteLine(1, $"    base.m_uiState = {anyGathering.data.m_uiState}");
				Log.WriteLine(1, $"    base.m_strDescription = {anyGathering.data.m_strDescription}");
				Log.WriteLine(1, "}");
			}

			// return True

			UNIMPLEMENTED();
			return Result(new { result = true });
		}

		[RMCMethod(5)]
		public void Invite()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(6)]
		public void AcceptInvitation()
		{
			UNIMPLEMENTED();
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
		public void GetInvitationsSent()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(10)]
		public void GetInvitationsReceived()
		{
			UNIMPLEMENTED();
		}

		[RMCMethod(11)]
		public RMCResult Participate(uint idGathering, string strMessage)
		{
			UNIMPLEMENTED($"uint idGathering = { idGathering }, string strMessage = { strMessage }");
			return Result(new { result = true });
		}

		[RMCMethod(12)]
		public RMCResult CancelParticipation()
		{
			UNIMPLEMENTED();
			return Result(new { result = true });
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
		public void FindBySingleID()
		{
			UNIMPLEMENTED();
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
		public RMCResult RegisterLocalURL(uint gid, string url)
		{
			UNIMPLEMENTED($"uint gid = {gid}, string url = {url}");
			return Error(0);
		}

		[RMCMethod(39)]
		public RMCResult RegisterLocalURLs(uint gid, IEnumerable<string> urls)
		{

			UNIMPLEMENTED($"uint gid = {gid}");
			return Error(0);
		}

		[RMCMethod(40)]
		public RMCResult UpdateSessionHostV1(uint gid)
		{
			return Error(0);
		}

		[RMCMethod(41)]
		public void GetSessionURLs()
		{
			UNIMPLEMENTED();
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
