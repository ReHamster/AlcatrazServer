using DSFServices.DDL.Models;
using QNetZ;
using QNetZ.Attributes;
using QNetZ.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DSFServices.Services
{
	/// <summary>
	/// Game session 
	///		Implements the sessions responsible for the gameplay process
	/// </summary>
	[RMCService(RMCProtocolId.GameSessionService)]
	public class GameSessionService : RMCServiceBase
	{
		static uint GameSessionCounter = 200600;

		static readonly List<GameSessionData> Sessions = new List<GameSessionData>();

		[RMCMethod(1)]
		public RMCResult CreateSession(GameSession gameSession)
		{
			var plInfo = Context.Client.Info;
			var newSession = new GameSessionData();
			Sessions.Add(newSession);

			newSession.Id = ++GameSessionCounter;
			newSession.HostPID = plInfo.PID;
			newSession.Session.m_typeID = gameSession.m_typeID;

			foreach (var attr in gameSession.m_attributes)
				newSession.Session.m_attributes.Add(attr);

			// return key
			var result = new GameSessionKey();
			result.m_sessionID = newSession.Id;
			result.m_typeID = newSession.Session.m_typeID;

			return Result(result);
		}


		[RMCMethod(2)]
		public RMCResult UpdateSession(GameSessionUpdate gameSessionUpdate)
		{
			var session = Sessions
				.FirstOrDefault(x => x.Id == gameSessionUpdate.m_sessionKey.m_sessionID && 
									 x.Session.m_typeID == gameSessionUpdate.m_sessionKey.m_typeID);

			if(session != null)
			{
				// update or add attributes
				foreach (var attr in gameSessionUpdate.m_attributes)
				{
					var updAttr = session.Session.m_attributes.FirstOrDefault(x => x.ID == attr.ID);
					if (updAttr != null)
						updAttr.Value = attr.Value;
					else
						session.Session.m_attributes.Add(attr);
				}
			}

			return Error(0);
		}


		[RMCMethod(3)]
		public RMCResult DeleteSession(GameSessionKey gameSessionKey)
		{
			UNIMPLEMENTED();
			return Error(0);
		}


		[RMCMethod(4)]
		public RMCResult MigrateSession(GameSessionKey gameSessionKey)
		{
			var gameSessionKeyMigrated = new GameSessionKey();
			UNIMPLEMENTED();
			return Result(gameSessionKeyMigrated);
		}


		[RMCMethod(5)]
		public RMCResult LeaveSession(GameSessionKey gameSessionKey)
		{
			UNIMPLEMENTED();
			return Error(0);
		}


		[RMCMethod(6)]
		public RMCResult GetSession(GameSessionKey gameSessionKey)
		{
			var searchResult = new GameSessionSearchResult();

			var session = Sessions.FirstOrDefault(x => x.Id == gameSessionKey.m_sessionID && x.Session.m_typeID == gameSessionKey.m_typeID);

			if(session != null)
			{
				searchResult = new GameSessionSearchResult()
				{
					m_hostPID = session.HostPID,
					m_hostURLs = session.HostURLs,
					m_attributes = session.Session.m_attributes,
					m_sessionKey = new GameSessionKey()
					{
						m_sessionID = session.Id,
						m_typeID = session.Session.m_typeID
					}
				};
			}

			return Result(searchResult);
		}


		[RMCMethod(7)]
		public RMCResult SearchSessions(uint m_typeID, uint m_queryID, IEnumerable<GameSessionProperty> m_parameters)
		{
			// TODO: where to hold m_queryID??? Are there notifications?

			var sessions = Sessions.Where(x => x.Session.m_typeID == m_typeID).ToArray();

			var resultList = new List<GameSessionSearchResult>();

			foreach (var ses in sessions)
			{
				// if all parameters match the found attributes, add as search result
				if(m_parameters.All(p => ses.Session.m_attributes.Any(sa => p.ID == sa.ID && p.Value == sa.Value)))
				{
					resultList.Add(new GameSessionSearchResult()
					{
						m_hostPID = ses.HostPID,
						m_hostURLs = ses.HostURLs,
						m_attributes = ses.Session.m_attributes,
						m_sessionKey = new GameSessionKey()
						{
							m_sessionID = ses.Id,
							m_typeID = ses.Session.m_typeID
						},
					});
				}
			}

			return Result(resultList);
		}


		[RMCMethod(8)]
		public RMCResult AddParticipants(GameSessionKey gameSessionKey, IEnumerable<uint> publicParticipantIDs, IEnumerable<uint> privateParticipantIDs)
		{
			var session = Sessions
				.FirstOrDefault(x => x.Id == gameSessionKey.m_sessionID && 
									 x.Session.m_typeID == gameSessionKey.m_typeID);

			if(session != null)
			{
				foreach (var pid in publicParticipantIDs)
					session.PublicParticipants.Add(pid);

				foreach (var pid in privateParticipantIDs)
					session.Participants.Add(pid);

				foreach(var pid in session.AllParticipants)
				{
					var player = NetworkPlayers.GetPlayerInfoByPID(pid);
					if(player != null)
					{
						player.GameData().CurrentSessionID = gameSessionKey.m_sessionID;
						player.GameData().CurrentSessionTypeID = gameSessionKey.m_typeID;
					}
				}
			}
			else
			{
				QLog.WriteLine(1, $"Error : GameSessionService.AddParticipants - no session with id={gameSessionKey.m_sessionID}");
			}

			return Error(0);
		}


		[RMCMethod(9)]
		public RMCResult RemoveParticipants(GameSessionKey gameSessionKey, IEnumerable<uint> participantIDs)
		{
			var session = Sessions
				.FirstOrDefault(x => x.Id == gameSessionKey.m_sessionID &&
									 x.Session.m_typeID == gameSessionKey.m_typeID);

			if (session != null)
			{
				foreach (var pid in participantIDs)
				{
					var player = NetworkPlayers.GetPlayerInfoByPID(pid);
					if (player != null)
					{
						player.GameData().CurrentSessionID = uint.MaxValue;
						player.GameData().CurrentSessionTypeID = uint.MaxValue;
					}
				}

				foreach (var pid in participantIDs)
					session.PublicParticipants.Remove(pid);

				foreach (var pid in participantIDs)
					session.Participants.Remove(pid);
			}
			else
			{
				QLog.WriteLine(1, $"Error : GameSessionService.RemoveParticipants - no session with id={gameSessionKey.m_sessionID}");
			}

			return Error(0);
		}


		[RMCMethod(10)]
		public RMCResult GetParticipantCount(GameSessionKey gameSessionKey, IEnumerable<uint> participantIDs)
		{
			UNIMPLEMENTED();
			return Error(0);
		}


		[RMCMethod(11)]
		public void GetParticipants()
		{
			UNIMPLEMENTED();
		}


		[RMCMethod(12)]
		public void SendInvitation()
		{
			UNIMPLEMENTED();
		}


		[RMCMethod(13)]
		public void GetInvitationReceivedCount()
		{
			UNIMPLEMENTED();
		}


		[RMCMethod(14)]
		public void GetInvitationsReceived()
		{
			UNIMPLEMENTED();
		}


		[RMCMethod(15)]
		public void GetInvitationSentCount()
		{
			UNIMPLEMENTED();
		}


		[RMCMethod(16)]
		public void GetInvitationsSent()
		{
			UNIMPLEMENTED();
		}


		[RMCMethod(17)]
		public void AcceptInvitation()
		{
			UNIMPLEMENTED();
		}


		[RMCMethod(18)]
		public void DeclineInvitation()
		{
			UNIMPLEMENTED();
		}


		[RMCMethod(19)]
		public void CancelInvitation()
		{
			UNIMPLEMENTED();
		}


		[RMCMethod(20)]
		public void SendTextMessage()
		{
			UNIMPLEMENTED();
		}


		[RMCMethod(21)]
		public RMCResult RegisterURLs(IEnumerable<string> stationURLs, GameSessionKey gameSessionKey)
		{
			QLog.WriteLine(1, "RegisterURLs : stationURLs {");
			foreach (var url in stationURLs)
			{
				QLog.WriteLine(1, $"    {url}");
			}
			QLog.WriteLine(1, "}");
			// TODO:
			UNIMPLEMENTED();
			return Error(0);
		}


		[RMCMethod(22)]
		public void JoinSession()
		{
			UNIMPLEMENTED();
		}


		[RMCMethod(23)]
		public RMCResult AbandonSession(GameSessionKey gameSessionKey)
		{
			var plInfo = Context.Client.Info;
			var myPlayerId = plInfo.PID;
			var session = Sessions
				.FirstOrDefault(x => x.Id == gameSessionKey.m_sessionID && 
									 x.Session.m_typeID == gameSessionKey.m_typeID);

			if(session != null)
			{
				plInfo.GameData().CurrentSessionTypeID = uint.MaxValue;
				plInfo.GameData().CurrentSessionID = uint.MaxValue;

				session.PublicParticipants.Remove(myPlayerId);
				session.Participants.Remove(myPlayerId);
			}
			else
			{
				QLog.WriteLine(1, $"Error : GameSessionService.RemoveParticipants - no session with id={gameSessionKey.m_sessionID}");
			}

			if (session.PublicParticipants.Count == 0 && session.Participants.Count == 0)
			{
				Sessions.Remove(session);
			}

			return Error(0);
		}


		[RMCMethod(24)]
		public void SearchSessionsWithParticipants()
		{
			UNIMPLEMENTED();
		}


		[RMCMethod(25)]
		public void GetSessions()
		{
			UNIMPLEMENTED();
		}


		[RMCMethod(26)]
		public void GetParticipantsURLs()
		{
			UNIMPLEMENTED();
		}


		[RMCMethod(27)]
		public void MigrateSessionHost()
		{
			UNIMPLEMENTED();
		}


		[RMCMethod(28)]
		public void SplitSession()
		{
			UNIMPLEMENTED();
		}


		[RMCMethod(29)]
		public void SearchSocialSessions()
		{
			UNIMPLEMENTED();
		}


		[RMCMethod(30)]
		public void ReportUnsuccessfulJoinSessions()
		{
			UNIMPLEMENTED();
		}


	}
}
