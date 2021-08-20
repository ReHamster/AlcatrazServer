using Alcatraz.Context.Entities;
using DSFServices.DDL.Models;
using Microsoft.EntityFrameworkCore;
using QNetZ;
using QNetZ.Attributes;
using QNetZ.DDL;
using QNetZ.Interfaces;
using RDVServices;
using System.Collections.Generic;
using System.IO;
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
					var existringRequest = db.UserRelationships
						.FirstOrDefault(x => x.User1Id == myUserPid && x.User2Id == foundUser.Id ||
											 x.User1Id == foundUser.Id && x.User2Id == myUserPid);

					if(existringRequest != null)
					{
						return Result(new { retVal = false });
					}

					// add new relationship with ID 3
					db.UserRelationships.Add(new UserRelationship { 
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
						m_strParam = strMessage
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
					var existringRequest = db.UserRelationships
						.FirstOrDefault(x => x.User1Id == myUserPid && x.User2Id == foundUser.Id ||
											 x.User1Id == foundUser.Id && x.User2Id == myUserPid);

					if (existringRequest != null)
					{
						existringRequest.ByRelationShip = 1;
						db.SaveChanges();
					}
					else
					{
						return Result(new { retVal = false });
					}

					result = true;

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
		public RMCResult ClearRelationship(uint uiPlayer)
		{
			bool result = false;
			var plInfo = Context.Client.Info;
			var myUserPid = plInfo.PID;

			using (var db = DBHelper.GetDbContext())
			{
				var existringRequest = db.UserRelationships
					.FirstOrDefault(x => x.User1Id == myUserPid && x.User2Id == uiPlayer ||
										 x.User1Id == uiPlayer && x.User2Id == myUserPid);

				if(existringRequest != null)
				{
					db.UserRelationships.Remove(existringRequest);
					db.SaveChanges();

					result = true;
				}
			}

			return Result(new { retVal = result });
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
#if false
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
						new UserRelationship
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
#else
			var result = new List<FriendData>();
#endif
			return Result(result);
		}

		[RMCMethod(13)]
		public RMCResult GetRelationships(int offset, int size)
		{
			var result = new RelationshipsResult();

			var relationshipsBytes = "0B 00 00 00 0B 00 00 00 AB D6 05 00 09 00 6D 63 6E 61 6C 6C 79 6F 00 01 00 00 00 00 00 E9 3B 08 00 0C 00 53 6E 6F 6F 70 79 42 6C 61 6E 6B 00 01 00 00 00 00 00 22 3D 08 00 0B 00 7A 75 63 6B 69 6C 6F 61 6B 73 00 01 00 00 00 00 00 B5 43 08 00 0F 00 67 6F 6C 64 65 6E 5F 73 6C 65 6E 64 65 72 00 01 00 00 00 00 00 23 44 08 00 0C 00 4E 69 6B 6B 69 43 68 61 6E 39 32 00 01 00 00 00 00 00 D0 45 08 00 0C 00 61 64 72 69 61 61 6E 39 31 30 30 00 01 00 00 00 00 00 2B 47 08 00 0E 00 52 61 63 69 6E 67 46 72 65 61 6B 39 35 00 01 00 00 00 00 00 8C 4D 08 00 0E 00 56 65 72 79 48 6F 74 50 65 72 73 6F 6E 00 01 00 00 00 00 00 F3 4D 08 00 0C 00 56 6F 72 74 65 78 53 74 6F 72 65 00 01 00 00 00 00 00 97 4F 08 00 0D 00 53 49 44 45 53 57 49 50 45 31 32 37 00 01 00 00 00 00 00 47 BD 05 00 0E 00 56 6F 72 74 65 78 4C 65 42 65 6C 67 65 00 03 00 00 00 00 01 ";

			var resultTest = DDLSerializer.ReadObject<RelationshipsResult>(new MemoryStream(Helper.ParseByteArray(relationshipsBytes)));

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
