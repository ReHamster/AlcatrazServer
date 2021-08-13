using System.Net;

namespace QNetZ
{
	// client connection info on particular server
	public class QClient
	{
		public uint sPID;
		public ushort sPort;

		public byte sessionID;

		public uint IDrecv;		// connection signature for recieving
		public uint IDsend;		// connection signature for sending

		public IPEndPoint endpoint;

		public ushort seqCounter;
		public ushort seqCounterOut;
		public uint callCounterRMC;

		public ClientInfo info;      // unique client info instance FIXME: needs to be resolved differently!
	}
}
