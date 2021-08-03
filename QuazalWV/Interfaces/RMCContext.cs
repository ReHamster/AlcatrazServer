using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuazalWV.Interfaces
{
	public class RMCContext
	{
		public RMCContext(RMCP rmc, ClientInfo clientInfo, QPacket packet)
		{
			RMC = rmc;
			Client = clientInfo;
			Packet = packet;
		}

		public readonly RMCP RMC;
		public readonly ClientInfo Client;
		public readonly QPacket Packet;
	}
}
