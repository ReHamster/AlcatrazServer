using QuazalWV.Attributes;
using QuazalWV.Interfaces;
using System.Collections.Generic;

namespace QuazalWV.RMCServices
{
	/// <summary>
	/// Authentication service (ticket granting)
	/// </summary>
	[RMCService(RMCP.PROTOCOL.TicketGrantingService)]
    public class TicketGrantingService : RMCServiceBase
    {
        [RMCMethod(1)]
        public RMCResult Login(RMCPacketRequestLogin login)
		{
            if (login.userName == "Tracking")
            {
                // TODO: appropriate response
                var reply = new Login()
                {
                    strUserName = login.userName,
                    retVal = 0x10001,
                    pidPrincipal = 0,
                    strReturnMsg = "Tracking_1.0:master",
                    pConnectionData = new RVConnectionData()
                    {
                        m_urlRegularProtocols = "",
                        m_urlSpecialProtocols = "",
                        m_lstSpecialProtocols = new byte[] { }
                    },
                    pbufResponse = new byte[]{ }
                };

                return Result(reply);
            }
            else
                UNIMPLEMENTED();

            return Error(0x1000);
        }

        /// <summary>
        /// Function where client login is performed by account ID and password
        /// </summary>
        /// <param name="login"></param>
        [RMCMethod(2)]
        public void LoginEx(RMCPacketRequestLoginCustomData login)
		{
            switch (login.className)
            {
                case "UbiAuthenticationLoginCustomData":
                    RMCPResponse reply = new RMCPResponseEmpty();
                    ClientInfo user = DBHelper.GetUserByName(login.username);
                    if (user != null)
                    {
                        if (user.pass == login.password)
                        {
                            reply = new RMCPacketResponseLoginCustomData(Context.Client.PID, Context.Client.sPID, Context.Client.sPort);
                            Context.Client.accountId = login.user;
                            Context.Client.name = login.username;
                            Context.Client.pass = login.password;

                            Context.Client.sessionKey = ((RMCPacketResponseLoginCustomData)reply).ticket.sessionKey;

                            SendResponseWithACK(reply);
                        }
                        else
                        {
                            SendResponseWithACK(reply, true, 0x80030065);
                        }
                    }
                    else
                    {
                        SendResponseWithACK(reply, true, 0x80030064);
                    }
                    break;
                default:
                    Log.WriteLine(1, $"[RMC Authentication] Error: Unknown Custom Data class '{login.className}'");
                    break;
            }
        }


        [RMCMethod(3)]
        public void RequestTicket(RMCPacketRequestRequestTicket login)
		{
            var reply = new RMCPacketResponseRequestTicket(Context.Client.PID, Context.Client.sPID);
            SendResponseWithACK(reply);
        }
    }
}
