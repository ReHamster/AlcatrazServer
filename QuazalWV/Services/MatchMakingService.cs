using QuazalWV.Attributes;
using QuazalWV.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace QuazalWV.Services
{
	/// <summary>
	/// Hermes 
	/// </summary>
	[RMCService(RMCProtocol.MatchMakingService)]
	class MatchMakingService : RMCServiceBase
	{
		[RMCMethod(1)]
		public RMCResult RegisterGathering(/* TODO: model*/)
		{
			uint result = 1;

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
		public RMCResult UpdateGathering(/*Data<Gathering> anyGathering*/)
		{
			return Error(0);
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
			return Result(new { result = true });
		}

		[RMCMethod(12)]
		public void CancelParticipation()
		{
			UNIMPLEMENTED();
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
			return Error(0);
		}

		[RMCMethod(39)]
		public RMCResult RegisterLocalURLs(uint gid, IEnumerable<string> urls)
		{
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
