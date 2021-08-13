using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QNetZ
{
	public class DO
	{
		public static void HandlePacket(QPacketHandlerPRUDP handler, QPacket p, QClient client)
		{
			QLog.WriteLine(1, "Error : Duplicated objects are not implemented on this server.");
		}
	}
}
