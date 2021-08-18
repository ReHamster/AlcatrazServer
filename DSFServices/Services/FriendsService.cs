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
			var plInfo = Context.Client.Info;
			var myUserPid = plInfo.PID;

			using (var db = DBHelper.GetDbContext())
			{
				var foundUser = db.Users
					.AsNoTracking()
					.Where(x => x.Id != myUserPid)
					.FirstOrDefault(x => x.PlayerNickName == strPlayerName);

				if (foundUser != null)
				{
					// FIXME: There is some problem with game that it does not bring up UI
					db.UserRelationships.Add(new Relationship { 
						Details = uiDetails,
						User1Id = myUserPid,
						User2Id = foundUser.Id,
						ByRelationShip = 3
					});
					db.SaveChanges();
					
					result = true;

					// send notification
					var notification = new NotificationEvent(NotificationEventsType.FriendEvent, 0)
					{
						m_pidSource = myUserPid,
						m_uiParam1 = myUserPid,       // i'm just guessing
						m_uiParam2 = 2,
						m_strParam = ""
					};

					// send to proper client
					// FIXME: save in db and send notification again in GetDetailedList???
					var qClient = Context.Handler.GetQClientByClientPID(foundUser.Id);

					if(qClient != null)
						NotificationQueue.SendNotification(Context.Handler, qClient, notification);
				}
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
			var plInfo = Context.Client.Info;
			var myUserPid = plInfo.PID;

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

					// send notification

					var notification = new NotificationEvent(NotificationEventsType.FriendEvent, 0)
					{
						m_pidSource = myUserPid,
						m_uiParam1 = foundUser.Id,		// i'm just guessing
						m_uiParam2 = 1
					};

					// should be that sent to friend too?
					NotificationQueue.SendNotification(Context.Handler, Context.Client, notification);
				}
			}

			return Result(new { retVal = result });
		}

		[RMCMethod(6)]
		public RMCResult DeclineFriendship(uint uiPlayer)
		{
			var plInfo = Context.Client.Info;
			var myUserPid = plInfo.PID;

			// send notification
			var notification = new NotificationEvent(NotificationEventsType.FriendEvent, 0)
			{
				m_pidSource = myUserPid,
				m_uiParam1 = myUserPid,       // i'm just guessing
				m_uiParam2 = 3
			};

			// send to proper client
			// FIXME: save in db and send notification again in GetDetailedList???
			var qClient = Context.Handler.GetQClientByClientPID(uiPlayer);

			if (qClient != null)
			{
				NotificationQueue.SendNotification(Context.Handler, qClient, notification);
			}

			return Result(new { retVal = true });
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

			var plInfo = Context.Client.Info;
			var myUserPid = plInfo.PID;

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
						m_strStatus = "",
						m_uiDetails = x.Details,
						m_byRelationship = (byte)x.ByRelationShip
					}).ToArray();
			}

			return Result(result);
		}

		[RMCMethod(13)]
		public RMCResult GetRelationships(int offset, int size)
		{
			var result = new RelationshipsResult();

			var myUserPid = Context.Client.Info.PID;
			using (var db = DBHelper.GetDbContext())
			{
				var relations = db.UserRelationships
					.Include(x => x.User1)
					.Include(x => x.User2)
					.AsNoTracking()
					.Where(x => x.User1Id == myUserPid || x.User2Id == myUserPid);

				result.uiTotalCount = (uint)relations.Count();

				var relationsPage = relations.Skip(offset).Take(size).ToList();   // apply pagination

				result.lstRelationshipsList = relationsPage.Select(x =>

					{
						var swap = x.User1Id == myUserPid;
						var res = new RelationshipData()
						{
							m_pid = swap ? x.User2Id : x.User1Id,
							m_strName = swap ? x.User2.PlayerNickName : x.User1.PlayerNickName,
							m_byStatus = (byte)(NetworkPlayers.Players.Any(p => p.PID == (swap ? x.User2Id : x.User1Id)) ? 1 : 0),
							m_uiDetails = x.Details,
							m_byRelationship = (byte)x.ByRelationShip
						};
						return res;
					});

			}

			return Result(result);
		}
	}
}
