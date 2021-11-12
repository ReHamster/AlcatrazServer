using QNetZ.DDL;
using QNetZ.Factory;
using QNetZ.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace QNetZ
{
	public static class RMC
	{
		public static void HandlePacket(QPacketHandlerPRUDP handler, QPacket p, QClient client)
		{
			client.SessionID = p.m_bySessionID;

			if (p.uiSeqId > client.SeqCounter)
				client.SeqCounter = p.uiSeqId;

			var rmc = new RMCPacket(p);
			if (rmc.isRequest)
				HandleRequest(handler, client, p, rmc);
			else
				HandleResponse(handler, client, p, rmc);
		}

		public static void HandleResponse(QPacketHandlerPRUDP handler, QClient client, QPacket p, RMCPacket rmc)
		{
			WriteLog(2, "Received Response : " + rmc.ToString());
			WriteLog(2, "Got response for Protocol " + rmc.proto + " = " + (rmc.success ? "Success" : $"Fail : { rmc.error.ToString("X8") } for callID = { rmc.callID }"));

			handler.SendACK(p, client);
		}

		public static void HandleRequest(QPacketHandlerPRUDP handler, QClient client, QPacket p, RMCPacket rmc)
		{
			if (rmc.callID > client.CallCounterRMC)
				client.CallCounterRMC = rmc.callID;

			WriteLog(2, "Request : " + rmc.ToString());

			MemoryStream m = new MemoryStream(p.payload);
			m.Seek(rmc._afterProtocolOffset, SeekOrigin.Begin);

			var rmcContext = new RMCContext(rmc, handler, client, p);

			// create service instance
			var serviceFactory = RMCServiceFactory.GetServiceFactory(rmc.proto);

			if (serviceFactory != null)
			{
				var serviceInstance = serviceFactory();
				var bestMethod = serviceInstance.GetServiceMethodById(rmc.methodID);

				if (bestMethod != null)
				{
					// set the execution context
					serviceInstance.Context = rmcContext;

					// call method
					// TODO: extended info
					var typeList = bestMethod.GetParameters().Select(x => x.ParameterType);
					var parameters = DDLSerializer.ReadPropertyValues(typeList.ToArray(), m);

					QLog.WriteLine(5, () => "[RMC] Request parameters: " + DDLSerializer.ObjectToString(parameters));

					var returnValue = bestMethod.Invoke(serviceInstance, parameters);

					if (returnValue != null)
					{
						if (typeof(RMCResult).IsAssignableFrom(returnValue.GetType()))
						{
							var rmcResult = (RMCResult)returnValue;

							SendResponseWithACK(
								handler,
								rmcContext.Packet,
								rmcContext.RMC,
								rmcContext.Client,
								rmcResult.Response,
								rmcResult.Compression, rmcResult.Error);
						}
						else
						{
							// TODO: try to cast and create RMCPResponseDDL???
						}
					}

					return;
				}
				else
				{
					WriteLog(1, $"Error: No method '{ rmc.methodID }' registered for protocol '{ rmc.proto }'");
				}
			}
			else
			{
				WriteLog(1, $"Error: No service registered for packet protocol '{ rmc.proto }' (protocolId = { (int)rmc.proto })");
			}
		}

		public static void SendResponseWithACK(QPacketHandlerPRUDP handler, QPacket p, RMCPacket rmc, QClient client, RMCPResponse reply, bool useCompression = true, uint error = 0)
		{
			WriteLog(2, "Response : " + reply.ToString());
			QLog.WriteLine(5, () => "[RMC] Response data : \n" + reply.PayloadToString());

			handler.SendACK(p, client);

			SendResponsePacket(handler, p, rmc, client, reply, useCompression, error);
		}

		public static void SendRMCCall(QPacketHandlerPRUDP handler, QClient client, RMCProtocolId protoId, uint methodId, RMCPRequest requestData)
		{
			var packet = new QPacket();

			packet.m_oSourceVPort = new QPacket.VPort(0x31);
			packet.m_oDestinationVPort = new QPacket.VPort(0x3f);

			packet.type = QPacket.PACKETTYPE.DATA;
			packet.flags = new List<QPacket.PACKETFLAG>() { QPacket.PACKETFLAG.FLAG_RELIABLE | QPacket.PACKETFLAG.FLAG_NEED_ACK };
			packet.payload = new byte[0];
			packet.m_bySessionID = client.SessionID;

			var rmc = new RMCPacket();

			rmc.proto = protoId;
			rmc.methodID = methodId;

			QLog.WriteLine(2, $"[RMC] Sending call { protoId }.{ methodId }");
			QLog.WriteLine(5, () => "[RMC] Call data : " + requestData.PayloadToString());

			SendRequestPacket(handler, packet, rmc, client, requestData, true, 0);
		}

		private static void SendResponsePacket(QPacketHandlerPRUDP handler, QPacket p, RMCPacket rmc, QClient client, RMCPResponse reply, bool useCompression, uint error)
		{
			rmc.isRequest = false;
			rmc.response = reply;
			rmc.error = error;

			var rmcResponseData = rmc.ToBuffer();

			QPacket np = new QPacket(p.toBuffer());
			np.flags = new List<QPacket.PACKETFLAG>() { QPacket.PACKETFLAG.FLAG_NEED_ACK, QPacket.PACKETFLAG.FLAG_RELIABLE };
			np.m_oSourceVPort = p.m_oDestinationVPort;
			np.m_oDestinationVPort = p.m_oSourceVPort;
			np.m_uiSignature = client.IDsend;
			np.usesCompression = useCompression;

			handler.MakeAndSend(client, p, np, rmcResponseData);
		}

		public static void SendRequestPacket(QPacketHandlerPRUDP handler, QPacket p, RMCPacket rmc, QClient client, RMCPRequest request, bool useCompression, uint error)
		{
			rmc.isRequest = true;
			rmc.request = request;
			rmc.error = error;
			rmc.callID = ++client.CallCounterRMC;

			var rmcRequestData = rmc.ToBuffer();

			QPacket np = new QPacket(p.toBuffer());
			np.flags = new List<QPacket.PACKETFLAG>() { QPacket.PACKETFLAG.FLAG_RELIABLE | QPacket.PACKETFLAG.FLAG_NEED_ACK };
			np.m_uiSignature = client.IDsend;
			np.usesCompression = useCompression;

			handler.MakeAndSend(client, p, np, rmcRequestData);
		}

		private static void WriteLog(int priority, string s)
		{
			QLog.WriteLine(priority, $"[RMC] {s}");
		}
	}
}
