using Alcatraz.Context.Entities;
using DSFServices.DDL.Models;
using Microsoft.EntityFrameworkCore;
using QNetZ;
using QNetZ.Attributes;
using QNetZ.Interfaces;
using RDVServices;
using System.Collections.Generic;
using System.Linq;

namespace DSFServices.Services
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
		public RMCResult AddFriendByName(string strPlayerName, uint uiDetails, string strMessage)
		{
			bool result = false;
			var myUserPid = Context.Client.info.PID;

			using (var db = DBHelper.GetDbContext())
			{
				var foundUser = db.Users
					.AsNoTracking()
					.Where(x => x.Id != myUserPid)
					.FirstOrDefault(x => x.PlayerNickName == strPlayerName);

				if (foundUser != null)
					result = true;

				// TODO: send notification???
			}

			return Result(new { retVal = result });
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
		public RMCResult AcceptFriendship(uint uiPlayer)
		{
			bool result = false;
			var myUserPid = Context.Client.info.PID;

			using (var db = DBHelper.GetDbContext())
			{
				var foundUser = db.Users
					.AsNoTracking()
					.FirstOrDefault(x => x.Id == uiPlayer);

				if (foundUser != null)
				{
					result = true;

					db.UserRelationships.Add(new Relationship()
					{
						User1Id = myUserPid,
						User2Id = uiPlayer
					});

					db.SaveChanges();
				}

				// TODO: send notification???
			}

			return Result(new { retVal = result });
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
			IEnumerable<FriendData> result;

			var myUserPid = Context.Client.info.PID;
			using (var db = DBHelper.GetDbContext())
			{
				var relations = db.UserRelationships
					.Include(x => x.User1)
					.Include(x => x.User2)
					.AsNoTracking()
					.Where(x => x.User1Id == myUserPid || x.User2Id == myUserPid)
					.Where(x => x.ByRelationShip == byRelationship)
					.Select(x => x.User2Id == myUserPid ?
						new Relationship
						{  // swap list
							User1Id = x.User2Id,
							User1 = x.User2,
							User2Id = x.User1Id,
							User2 = x.User1,
						} : x);

				if (bReversed) // hmmmm
					relations = relations.Reverse();

				// complete the list
				result = relations.Select(x =>
					new FriendData()
					{
						m_pid = x.User2Id,
						m_strName = x.User2.PlayerNickName,
						m_strStatus = $"Status: {x.Status}",
						m_uiDetails = 0,
						m_byRelationship = (byte)x.ByRelationShip
					}).ToArray();
			}

			return Result(result);
		}

		[RMCMethod(13)]
		public RMCResult GetRelationships(int offset, int size)
		{
			var result = new RelationshipsResult();

			var myUserPid = Context.Client.info.PID;
			using (var db = DBHelper.GetDbContext())
			{
				var relations = db.UserRelationships
					.Include(x => x.User1)
					.Include(x => x.User2)
					.AsNoTracking()
					.Where(x => x.User1Id == myUserPid || x.User2Id == myUserPid);

				result.uiTotalCount = (uint)relations.Count();

				relations = relations.Skip(offset).Take(size)	// apply pagination
					.Select(x => x.User2Id == myUserPid ?
						new Relationship {  // swap list
							User1Id = x.User2Id,
							User1 = x.User2,
							User2Id = x.User1Id,
							User2 = x.User1,
						} : x);

				// complete the list
				result.lstRelationshipsList = relations.Select(x =>
					new RelationshipData(){
						m_pid = x.User2Id,
						m_strName = x.User2.PlayerNickName,
						m_byStatus = (byte)x.Status,
						m_uiDetails = 0,
						m_byRelationship = (byte)x.ByRelationShip
					}).ToArray();
			}

			return Result(result);
		}
	}
}
