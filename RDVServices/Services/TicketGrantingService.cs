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
		private byte[] ticket = new byte[] {
			0x76, 0x21, 0x4B, 0xA6, 0x21, 0x96, 0xD3, 0xF3, 0x9A,
			0x8C, 0x7A, 0x27, 0x0D, 0xD9, 0xB3, 0xFA, 0x21, 0x0E,
			0xED, 0xAF, 0x42, 0x63, 0x92, 0x95, 0xC1, 0x16, 0x54,
			0x08, 0xEE, 0x6E, 0x69, 0x17, 0x35, 0x78, 0x2E, 0x6E
		};


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
					{ "type", 2 }
				});

			var user = DBHelper.GetUserByName(userName);

			if (user != null)
			{
				// var trackingLoginData = "01 00 01 00 69 00 00 00 4C 00 00 00 99 39 C6 CB 93 13 50 8C 0B 02 C2 0B BC E4 94 6E B8 57 D0 15 A7 A1 AB 03 57 3F C1 69 F6 8E DC 55 0A A3 72 61 81 37 EB 6C A5 0C A2 C2 66 D5 B0 C6 23 15 E5 99 5A 3C 1F EC F7 90 55 2F 33 1E B7 C1 05 52 41 83 A0 1E 3F E8 18 02 7B 3B 4A 00 70 72 75 64 70 73 3A 2F 61 64 64 72 65 73 73 3D 31 38 35 2E 33 38 2E 32 31 2E 38 33 3B 70 6F 72 74 3D 32 31 30 30 36 3B 43 49 44 3D 31 3B 50 49 44 3D 32 3B 73 69 64 3D 31 3B 73 74 72 65 61 6D 3D 33 3B 74 79 70 65 3D 32 00 00 00 00 00 01 00 00 01 00 00";
				// 
				// var m = new MemoryStream(Helper.ParseByteArray(trackingLoginData));
				// 
				// var retModel = DDLSerializer.ReadObject<Login>(m);

				// create tracking client info
				var plInfo = NetworkPlayers.GetPlayerInfoByUsername(userName);

				if (plInfo != null &&
					!plInfo.Client.Endpoint.Equals(Context.Client.Endpoint) &&
					(DateTime.UtcNow - plInfo.Client.LastPacketTime).TotalSeconds < Constants.ClientTimeoutSeconds)
				{
					QLog.WriteLine(1, $"User login request {userName} DENIED - concurrent login!");
					return Error((int)RMCErrorCode.RendezVous_ConcurrentLoginDenied);
				}

				QLog.WriteLine(1, $"User login request {userName}");

				plInfo = NetworkPlayers.CreatePlayerInfo(Context.Client);

				plInfo.PID = user.Id;
				plInfo.AccountId = user.Username;
				plInfo.Name = user.Username;

				var kerberos = new KerberosTicket(plInfo.PID, Context.Client.sPID, Constants.SessionKey, ticket);

				var reply = new Login(plInfo.PID)
				{
					retVal = (int)RMCErrorCode.Core_NoError,
					pConnectionData = new RVConnectionData()
					{
						m_urlRegularProtocols = rdvConnectionString,
					},
					strReturnMsg = "",
					pbufResponse = kerberos.toBuffer()
				};

				return Result(reply);
			}

			return Error((int)RMCErrorCode.RendezVous_InvalidUsername);
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
					{ "type", 2 }
				});

			if (oExtraData.data != null)
			{
				ErrorCode loginCode = ErrorCode.Core_NoError;

				var plInfo = NetworkPlayers.GetPlayerInfoByUsername(userName);

				if(plInfo != null)
				{
					if (plInfo.Client != null &&
						!plInfo.Client.Endpoint.Equals(Context.Client.Endpoint) &&
						(DateTime.UtcNow - plInfo.Client.LastPacketTime).TotalSeconds < Constants.ClientTimeoutSeconds)
					{
						QLog.WriteLine(1, $"User login request {userName} - concurrent login!");
						loginCode = ErrorCode.RendezVous_ConcurrentLoginDenied;
					}
					else
					{
						NetworkPlayers.DropPlayerInfo(plInfo);
					}
				}

				var user = DBHelper.GetUserByName(oExtraData.data.username);

				if (user != null)
				{
					bool passwordCheckResult;
					try
					{
						var hashPassword = $"{user.Id}-{user.PlayerNickName}";
						passwordCheckResult = oExtraData.data.password == user.Password || SecurePasswordHasher.Verify(hashPassword, oExtraData.data.password);
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
					plInfo = NetworkPlayers.CreatePlayerInfo(Context.Client);

					plInfo.PID = user.Id;
					plInfo.AccountId = userName;
					plInfo.Name = oExtraData.data.username;

					var kerberos = new KerberosTicket(plInfo.PID, Context.Client.sPID, Constants.SessionKey, ticket);

					var loginData = new Login(plInfo.PID)
					{
						retVal = (uint)loginCode,
						pConnectionData = new RVConnectionData()
						{
							m_urlRegularProtocols = rdvConnectionString
						},
						strReturnMsg = "",
						pbufResponse = kerberos.toBuffer()
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
			var kerberos = new KerberosTicket(sourcePID, targetPID, Constants.SessionKey, ticket);

			var ticketData = new TicketData()
			{
				retVal = (int)ErrorCode.Core_NoError,
				pbufResponse = kerberos.toBuffer()
			};

			return Result(ticketData);
		}
	}
}
