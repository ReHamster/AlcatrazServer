using QuazalWV.Attributes;
using QuazalWV.Interfaces;

namespace QuazalWV.RMCServices
{
    /// <summary>
    /// Secure connection service protocol
    /// </summary>
	[RMCService(RMCP.PROTOCOL.SecureConnectionService)]
    public class SecureConnectionService : RMCServiceBase
    {
        [RMCMethod(2)]
        public void RequestConnectionData()
        {
            UNIMPLEMENTED();
        }

        [RMCMethod(3)]
        public void RequestUrls()
        {
            UNIMPLEMENTED();
        }

        [RMCMethod(4)]
        public void RegisterEx(RMCPacketRequestRegisterEx request)
		{
            switch (request.className)
            {
                case "UbiAuthenticationLoginCustomData":
                    var reply = new RMCPacketResponseRegisterEx(Context.Client.PID);
                    SendResponseWithACK(reply);
                    break;
                default:
                    Log.WriteLine(1, $"[RMC Secure] Error: Unknown Custom Data class {request.className}");
                    break;
            }
        }

        [RMCMethod(5)]
        public void TestConnectivity()
        {
            UNIMPLEMENTED();
        }

        [RMCMethod(6)]
        public void UpdateURLs()
        {
            UNIMPLEMENTED();
        }

        [RMCMethod(7)]
        public void ReplaceURL()
        {
            UNIMPLEMENTED();
        }

        [RMCMethod(8)]
        public void SendReport()
        {
            UNIMPLEMENTED();
        }
    }
}
