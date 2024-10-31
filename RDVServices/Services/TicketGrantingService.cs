using Alcatraz.DTO.Helpers;
using RDVServices.DDL.Models;
using QNetZ;
using QNetZ.Attributes;
using QNetZ.DDL;
using QNetZ.Interfaces;
using QNetZ.Connection;
using System.Collections.Generic;
using System;
using System.Net;

namespace RDVServices.Services
{
	/// <summary>
	/// Authentication service (ticket granting)
	/// </summary>
	[RMCService(RMCProtocolId.TicketGrantingService)]
	public class TicketGrantingService : RMCServiceBase
	{
		[RMCMethod(1)]
		public RMCResult Login(string userName)
		{
			var hostAddress = string.IsNullOrWhiteSpace(QConfiguration.Instance.ServerBindAddress) ? Dns.GetHostName() : QConfiguration.Instance.ServerBindAddress;
#if false
			var rdvConnectionString = new StationURL(
				"prudps",
				hostAddress,
				new Dictionary<string, int>() {
					{ "port", Context.Client.sPort },
					{ "CID", 1 },
					{ "PID", (int)Context.Client.sPID },
					{ "sid", 1 },
					{ "stream", 3 },
					{ "type", 2 }	// Public, not BehindNAT
				});

			PlayerInfo playerInfo = null;

			if (userName == "guest" || userName == "Tracking")
			{
				QLog.WriteLine(1, $"User login request {userName}");

				// TODO: do not create player info for Tracking

				playerInfo = NetworkPlayers.CreatePlayerInfo(Context.Client);
				if (userName == "Tracking")
					playerInfo.PID = 0;
				else if (userName == "guest")
					playerInfo.PID = 100;

				playerInfo.AccountId = userName;
				playerInfo.Name = userName;
			}
			else
			{
				var user = DBHelper.GetUserByName(userName);
				if (user == null)
					return Error((int)ErrorCode.RendezVous_InvalidUsername);

				// create tracking client info
				playerInfo = NetworkPlayers.GetPlayerInfoByUsername(userName);

				if (playerInfo != null &&
					!playerInfo.Client.Endpoint.Equals(Context.Client.Endpoint) &&
					playerInfo.Client.TimeSinceLastPacket < Constants.ClientTimeoutSeconds)
				{
					QLog.WriteLine(1, $"User login request {userName} DENIED - concurrent login!");
					return Error((int)ErrorCode.RendezVous_ConcurrentLoginDenied);
				}

				QLog.WriteLine(1, $"User login request {userName}");

				playerInfo = NetworkPlayers.CreatePlayerInfo(Context.Client);
				playerInfo.PID = user.Id;
				playerInfo.AccountId = user.Username;
				playerInfo.Name = user.Username;
			}

			var kerberos = new KerberosTicket(playerInfo.PID, Context.Client.sPID, Constants.SessionKey, Constants.TicketData);
			var reply = new Login(playerInfo.PID)
			{
				retVal = (int)ErrorCode.Core_NoError,
				pConnectionData = new RVConnectionData()
				{
					m_urlRegularProtocols = rdvConnectionString,
				},
				strReturnMsg = "",
				pbufResponse = kerberos.ToBuffer(Constants.NetZJadePassword)
			};

			return Result(reply);
#endif
			return Error(0);
		}

		/// <summary>
		/// Function where client login is performed by account ID and password
		/// </summary>
		/// <param name="login"></param>
		[RMCMethod(2)]
		public RMCResult LoginEx(string userName, AnyData<UbiAuthenticationLoginCustomData> oExtraData)
		{
			var hostAddress = string.IsNullOrWhiteSpace(QConfiguration.Instance.ServerBindAddress) ? Dns.GetHostName() : QConfiguration.Instance.ServerBindAddress;

			var rdvConnectionString = new StationURL(
				"prudps",
				hostAddress,
				new Dictionary<string, int>() {
					{ "port", QConfiguration.Instance.BackendServiceServerPort },
					{ "CID", 1 },
					{ "PID", (int)Context.Client.sPID },
					{ "sid", 1 },
					{ "stream", 3 },
					{ "type", 2 }	// Public, not BehindNAT
				});

			if (oExtraData.data != null)
			{
				ErrorCode loginCode = ErrorCode.Core_NoError;

				var playerInfo = NetworkPlayers.GetPlayerInfoByUsername(userName);

				if(playerInfo != null)
				{
					if (playerInfo.Client != null &&
						!playerInfo.Client.Endpoint.Equals(Context.Client.Endpoint) &&
						playerInfo.Client.TimeSinceLastPacket < Constants.ClientTimeoutSeconds)
					{
						QLog.WriteLine(1, $"User login request {userName} - concurrent login!");
						loginCode = ErrorCode.RendezVous_ConcurrentLoginDenied;
					}
					else
					{
						NetworkPlayers.DropPlayerInfo(playerInfo);
					}
				}

				var user = DBHelper.GetUserByName(oExtraData.data.username);

				if (user != null)
				{
					bool passwordCheckResult;
					try
					{
						var hashPassword = $"{user.Id}-{user.PlayerNickName}";
						passwordCheckResult = SecurePasswordHasher.Verify(hashPassword, oExtraData.data.password);
					}
					catch(Exception _)
					{
						passwordCheckResult = false;
					}
					
					if (passwordCheckResult)
					{
						QLog.WriteLine(1, $"User login request {userName} - success");
					}
					else
					{
						QLog.WriteLine(1, $"User login request {userName} - invalid password");

						loginCode = ErrorCode.RendezVous_InvalidPassword;
					}
				}
				else
				{
					QLog.WriteLine(1, $"User login request {userName} - invalid user name");
					loginCode = ErrorCode.RendezVous_InvalidUsername;
				}

				if(loginCode != ErrorCode.Core_NoError)
				{
					var loginData = new Login(0)
					{
						retVal = (uint)loginCode,
						pConnectionData = new RVConnectionData()
						{
							m_urlRegularProtocols = new StationURL("prudp:/")
						},
						strReturnMsg = "",
						pbufResponse = new byte[] { }
					};

					return Result(loginData);
				}
				else
				{
					playerInfo = NetworkPlayers.CreatePlayerInfo(Context.Client);
					playerInfo.PID = user.Id;
					playerInfo.AccountId = userName;
					playerInfo.Name = oExtraData.data.username;

					var kerberos = new KerberosTicket(playerInfo.PID, Context.Client.sPID, Constants.SessionKey, Constants.TicketData);

					var loginData = new Login(playerInfo.PID)
					{
						retVal = (uint)loginCode,
						pConnectionData = new RVConnectionData()
						{
							m_urlRegularProtocols = rdvConnectionString
						},
						strReturnMsg = "",
						pbufResponse = kerberos.ToBuffer(Constants.UbiDummyPassword)
					};

					return Result(loginData);
				}
			}
			else
			{
				QLog.WriteLine(1, $"[RMC Authentication] Error: Unknown Custom Data class '{oExtraData.className}'");
			}

			return Error((int)ErrorCode.RendezVous_ClassNotFound);
		}

		[RMCMethod(3)]
		public RMCResult RequestTicket(uint sourcePID, uint targetPID)
		{
			string ticketKey = Constants.UbiDummyPassword;
			if (sourcePID == 0)
				ticketKey = Constants.NetZJadePassword;
			else if (sourcePID == 100)
				ticketKey = Constants.NetZGuestPassword;

			var kerberos = new KerberosTicket(sourcePID, targetPID, Constants.SessionKey, Constants.TicketData);

			var ticketData = new TicketData()
			{
				retVal = (int)ErrorCode.Core_NoError,
				pbufResponse = kerberos.ToBuffer(ticketKey)
			};

			return Result(ticketData);
		}
	}
}
