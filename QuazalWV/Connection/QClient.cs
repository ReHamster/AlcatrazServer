using System.Net;

namespace QuazalWV
{
	// client connection info on particular server
	public class QClient
	{
		public uint sPID;
		public ushort sPort;

		public byte sessionID;

		public uint IDrecv;
		public uint IDsend;

		public IPEndPoint endpoint;

		public ushort seqCounter;
		public ushort seqCounterOut;
		public uint callCounterRMC;

		public ClientInfo info;      // unique client info instance FIXME: needs to be resolved differently!
	}
}
