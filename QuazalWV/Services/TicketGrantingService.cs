using QuazalWV.Attributes;
using QuazalWV.Interfaces;
using QuazalWV.DDL.Models;
using System.IO;
using QuazalWV.DDL;

namespace QuazalWV.Services
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
				.Replace("#ADDRESS#", Global.serverBindAddress)
				.Replace("#PORT#", Global.serverBindPort.ToString())
				.Replace("#SERVERID#", Context.Client.sPID.ToString());

			if (userName == "Tracking")
			{
				var kerberos = new KerberosTicket(Context.Client.PID, Context.Client.sPID, sessionKey, ticket);

				var reply = new Login()
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
			else
				UNIMPLEMENTED();

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
				.Replace("#ADDRESS#", Global.serverBindAddress)
				.Replace("#PORT#", Context.Client.sPort.ToString() )
				.Replace("#SERVERID#", Context.Client.sPID.ToString());

			if(oExtraData.data != null)
			{
				ClientInfo user = DBHelper.GetUserByName(oExtraData.data.username);

				if (user != null)
				{
					if (user.pass == oExtraData.data.password)
					{
						var kerberos = new KerberosTicket(Context.Client.PID, Context.Client.sPID, sessionKey, ticket);

						Context.Client.accountId = userName;
						Context.Client.name = oExtraData.data.username;
						Context.Client.pass = oExtraData.data.password;
						Context.Client.sessionKey = sessionKey;

						var loginData = new Login(Context.Client.PID, Context.Client.sPID)
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
						return Error((int)RMCErrorCode.RendezVous_InvalidPassword);
					}
				}
				else
				{
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
			var kerberos = new KerberosTicket(Context.Client.PID, Context.Client.sPID, sessionKey, ticket);

			var ticketData = new TicketData()
			{
				retVal = (int)RMCErrorCode.Core_NoError,
				pbufResponse = kerberos.toBuffer()
			};

			return Result(ticketData);
		}
	}
}
