using QuazalWV.Attributes;
using QuazalWV.DDL.Models;
using QuazalWV.Interfaces;
using System.Collections.Generic;

namespace QuazalWV.Services
{
	[RMCService(RMCProtocolId.GameSessionService)]
	public class GameSessionService : RMCServiceBase
	{
		[RMCMethod(1)]
		public RMCResult CreateSession(GameSession gameSession)
		{
			Log.WriteLine(1, "CreateSession : GameSession {");
			Log.WriteLine(1, $"    m_typeID = {gameSession.m_typeID}");
			Log.WriteLine(1, $"    m_attributes = ");
			int n = 0;
			foreach(var attr in gameSession.m_attributes)
			{
				Log.WriteLine(1, $"        [{n}] = ID { attr.ID }, Value = { attr.Value }");
				n++;
			}
			Log.WriteLine(1, "}");

			var result = new GameSessionKey();
			result.m_sessionID = 22046;
			result.m_typeID = gameSession.m_typeID;

			return Result(result);
		}


		[RMCMethod(2)]
		public RMCResult UpdateSession(GameSessionUpdate gameSessionUpdate)
		{
			Log.WriteLine(1, "UpdateSession : GameSessionUpdate {");
			Log.WriteLine(1, $"    m_sessionKey = {gameSessionUpdate.m_sessionKey.m_sessionID}");
			Log.WriteLine(1, $"    m_attributes = ");
			int n = 0;
			foreach (var attr in gameSessionUpdate.m_attributes)
			{
				Log.WriteLine(1, $"        [{n}] = ID { attr.ID }, Value = { attr.Value }");
				n++;
			}
			Log.WriteLine(1, "}");

			UNIMPLEMENTED();
			return Error(0);
		}


		[RMCMethod(3)]
		public void DeleteSession()
		{
			UNIMPLEMENTED();
		}


		[RMCMethod(4)]
		public void MigrateSession()
		{
			UNIMPLEMENTED();
		}


		[RMCMethod(5)]
		public void LeaveSession()
		{
			UNIMPLEMENTED();
		}


		[RMCMethod(6)]
		public void GetSession()
		{
			UNIMPLEMENTED();
		}


		[RMCMethod(7)]
		public void SearchSessions()
		{
			UNIMPLEMENTED();
		}


		[RMCMethod(8)]
		public RMCResult AddParticipants(GameSessionKey gameSessionKey, IEnumerable<uint> publicParticipantIDs, IEnumerable<uint> privateParticipantIDs)
		{
			Log.WriteLine(1, "AddParticipants : gameSessionKey, publicParticipantIDs, privateParticipantIDs {");
			Log.WriteLine(1, $"    gameSessionKey = {gameSessionKey.m_sessionID}");
			Log.WriteLine(1, $"    publicParticipantIDs = ");
			int n = 0;
			foreach (var p in publicParticipantIDs)
			{
				Log.WriteLine(1, $"        [{n}] = {p}");
				n++;
			}
			Log.WriteLine(1, $"    privateParticipantIDs = ");
			n = 0;
			foreach (var p in privateParticipantIDs)
			{
				Log.WriteLine(1, $"        [{n}] = {p}");
				n++;
			}
			Log.WriteLine(1, "}");

			UNIMPLEMENTED();
			return Error(0);
		}


		[RMCMethod(9)]
		public void RemoveParticipants()
		{
			UNIMPLEMENTED();
		}


		[RMCMethod(10)]
		public void GetParticipantCount()
		{
			UNIMPLEMENTED();
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
		public RMCResult RegisterURLs(IEnumerable<string> stationURLs)
		{
			Log.WriteLine(1, "RegisterURLs : stationURLs {");
			foreach (var url in stationURLs)
			{
				Log.WriteLine(1, $"    {url}");
			}
			Log.WriteLine(1, "}");
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
		public RMCResult AbandonSession(uint partyId)
		{
			// remove current client from partyId
			// AND delete session?

			UNIMPLEMENTED();
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
