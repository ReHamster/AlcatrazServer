using RDVServices.DDL.Models;
using QNetZ;
using QNetZ.Attributes;
using QNetZ.DDL;
using QNetZ.Interfaces;
using System.IO;
using System;

namespace RDVServices.Services
{
	/// <summary>
	/// Authentication service (ticket granting)
	/// </summary>
	[RMCService(RMCProtocolId.TicketGrantingService)]
	public class TicketGrantingService : RMCServiceBase
	{
		private byte[] sessionKey = new byte[] {
			0x9C, 0xB0, 0x1D, 0x7A, 0x2C, 0x5A,
			0x6C, 0x5B, 0xED, 0x12, 0x68, 0x45,
			0x69, 0xAE, 0x09, 0x0D
		};

		private byte[] ticket = new byte[] {
			0x76, 0x21, 0x4B, 0xA6, 0x21, 0x96, 0xD3, 0xF3, 0x9A,
			0x8C, 0x7A, 0x27, 0x0D, 0xD9, 0xB3, 0xFA, 0x21, 0x0E,
			0xED, 0xAF, 0x42, 0x63, 0x92, 0x95, 0xC1, 0x16, 0x54,
			0x08, 0xEE, 0x6E, 0x69, 0x17, 0x35, 0x78, 0x2E, 0x6E
		};


		[RMCMethod(1)]
		public RMCResult Login(string userName)
		{
			var rendezVousConnString = "prudps:/address=#ADDRESS#;port=#PORT#;CID=1;PID=#SERVERID#;sid=1;stream=3;type=2";

			rendezVousConnString = rendezVousConnString
				.Replace("#ADDRESS#", QConfiguration.Instance.ServerBindAddress)
				.Replace("#PORT#", Context.Client.sPort.ToString())
				.Replace("#SERVERID#", Context.Client.sPID.ToString());

			ClientInfo user = DBHelper.GetUserByName(userName);

			if (user != null)
			{
				// var trackingLoginData = "01 00 01 00 69 00 00 00 4C 00 00 00 99 39 C6 CB 93 13 50 8C 0B 02 C2 0B BC E4 94 6E B8 57 D0 15 A7 A1 AB 03 57 3F C1 69 F6 8E DC 55 0A A3 72 61 81 37 EB 6C A5 0C A2 C2 66 D5 B0 C6 23 15 E5 99 5A 3C 1F EC F7 90 55 2F 33 1E B7 C1 05 52 41 83 A0 1E 3F E8 18 02 7B 3B 4A 00 70 72 75 64 70 73 3A 2F 61 64 64 72 65 73 73 3D 31 38 35 2E 33 38 2E 32 31 2E 38 33 3B 70 6F 72 74 3D 32 31 30 30 36 3B 43 49 44 3D 31 3B 50 49 44 3D 32 3B 73 69 64 3D 31 3B 73 74 72 65 61 6D 3D 33 3B 74 79 70 65 3D 32 00 00 00 00 00 01 00 00 01 00 00";
				// 
				// var m = new MemoryStream(Helper.ParseByteArray(trackingLoginData));
				// 
				// var retModel = DDLSerializer.ReadObject<Login>(m);

				// create tracking client info
				var client = Global.GetClientByUsername(userName);

				if (client != null &&
					client.endpoint != Context.Client.endpoint &&
					(DateTime.UtcNow - client.lastRecv).TotalSeconds < Constants.ClientTimeoutSeconds)
				{
					Log.WriteLine(1, $"User login request {userName} DENIED - concurrent login!");
					return Error((int)RMCErrorCode.RendezVous_ConcurrentLoginDenied);
				}

				Global.DropClient(client);

				Log.WriteLine(1, $"User login request {userName}");

				client = Global.CreateClient(Context.Client);

				client.PID = user.PID;
				client.accountId = userName;
				client.name = userName;

				var kerberos = new KerberosTicket(client.PID, Context.Client.sPID, sessionKey, ticket);

				var reply = new Login(client.PID)
				{
					retVal = (int)RMCErrorCode.Core_NoError,
					pConnectionData = new RVConnectionData()
					{
						m_urlRegularProtocols = rendezVousConnString,
					},
					strReturnMsg = "",
					pbufResponse = kerberos.toBuffer()
				};

				return Result(reply);
			}

			return Error((int)RMCErrorCode.RendezVous_InvalidUsername);
		}

		/// <summary>
		/// Function where client login is performed by account ID and password
		/// </summary>
		/// <param name="login"></param>
		[RMCMethod(2)]
		public RMCResult LoginEx(string userName, AnyData<UbiAuthenticationLoginCustomData> oExtraData)
		{
			var rendezVousConnString = "prudps:/address=#ADDRESS#;port=#PORT#;CID=1;PID=#SERVERID#;sid=1;stream=3;type=2";

			rendezVousConnString = rendezVousConnString
				.Replace("#ADDRESS#", QConfiguration.Instance.ServerBindAddress)
				.Replace("#PORT#", QConfiguration.Instance.BackendServiceServerPort.ToString())
				.Replace("#SERVERID#", Context.Client.sPID.ToString());

			if(oExtraData.data != null)
			{
				var client = Global.GetClientByUsername(userName);

				if (client != null &&
					client.endpoint != Context.Client.endpoint &&
					(DateTime.UtcNow - client.lastRecv).TotalSeconds < Constants.ClientTimeoutSeconds)
				{
					Log.WriteLine(1, $"User login request {userName} DENIED - concurrent login!");
					return Error((int)RMCErrorCode.RendezVous_ConcurrentLoginDenied);
				}

				Global.DropClient(client);

				ClientInfo user = DBHelper.GetUserByName(oExtraData.data.username);

				if (user != null)
				{
					if (user.pass == oExtraData.data.password)
					{
						Log.WriteLine(1, $"User login request {userName}");
						client = Global.CreateClient(Context.Client);

						Context.Client.info = client;   // TEMPORARY

						client.PID = user.PID;
						client.sessionKey = sessionKey;
						client.accountId = userName;
						client.name = oExtraData.data.username;
						client.pass = oExtraData.data.password;

						var kerberos = new KerberosTicket(client.PID, Context.Client.sPID, sessionKey, ticket);

						var loginData = new Login(client.PID)
						{
							retVal = (int)RMCErrorCode.Core_NoError,
							pConnectionData = new RVConnectionData()
							{
								m_urlRegularProtocols = rendezVousConnString
							},
							strReturnMsg = "",
							pbufResponse = kerberos.toBuffer()
						};

						return Result(loginData);
					}
					else
					{
						Log.WriteLine(1, $"User login request {userName} DENIED - invalid password");
						return Error((int)RMCErrorCode.RendezVous_InvalidPassword);
					}
				}
				else
				{
					Log.WriteLine(1, $"User login request {userName} DENIED - invalid user name");
					return Error((int)RMCErrorCode.RendezVous_InvalidUsername);
				}
			}
			else
			{
				Log.WriteLine(1, $"[RMC Authentication] Error: Unknown Custom Data class '{oExtraData.className}'");
			}

			return Error((int)RMCErrorCode.RendezVous_ClassNotFound);
		}

		[RMCMethod(3)]
		public RMCResult RequestTicket(uint sourcePID, uint targetPID)
		{
			// var requestTicketData = "01 00 01 00 4C 00 00 00 49 54 E6 90 C2 C9 E0 FF 2F 16 B5 10 CB 10 69 E7 BB 57 D0 15 A7 A1 AB 03 EC 22 2F BD 50 76 4C D8 4E 71 A1 E4 03 8C C9 6C 65 EB C6 23 74 78 2A E8 86 33 B4 51 91 D1 50 83 81 44 4E FA 88 36 70 2D DE C2 D6 B4 CF E8 0B ED 68 23 DB 7F ";
			// var m = new MemoryStream(Helper.ParseByteArray(requestTicketData));
			// var retModel = DDLSerializer.ReadObject<TicketData>(m);

			var kerberos = new KerberosTicket(sourcePID, targetPID, sessionKey, ticket);

			var ticketData = new TicketData()
			{
				retVal = (int)RMCErrorCode.Core_NoError,
				pbufResponse = kerberos.toBuffer()
			};

			return Result(ticketData);
		}
	}
}
