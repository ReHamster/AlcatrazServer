using System;
using System.Net;

namespace QNetZ
{
	// client connection info on particular server
	public class QClient
	{
		public QClient(uint recvId, IPEndPoint ep)
		{
			IDrecv = recvId;
			LastPacketTime = DateTime.UtcNow;
			endpoint = ep;
		}

		public uint sPID;				// server PID
		public ushort sPort;			// server port
		public IPEndPoint endpoint;     // client endpoint
		public DateTime LastPacketTime;

		public byte sessionID;

		public uint IDrecv;		// connection signature for recieving
		public uint IDsend;		// connection signature for sending

		public ushort seqCounter;
		public ushort seqCounterOut;
		public uint callCounterRMC;

		public PlayerInfo info;      // unique player info instance
	}
}
