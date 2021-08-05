using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuazalWV.Interfaces
{
	public class RMCContext
	{
		public RMCContext(RMCPacket rmc, ClientInfo clientInfo, QPacket packet)
		{
			RMC = rmc;
			Client = clientInfo;
			Packet = packet;
		}

		public readonly RMCPacket RMC;
		public readonly ClientInfo Client;
		public readonly QPacket Packet;
	}
}
