﻿using QNetZ.DDL;
using QNetZ.Factory;
using QNetZ.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace QNetZ
{
	public static class RMC
    {
        public static void HandlePacket(QPacketHandlerPRUDP handler, QPacket p, QClient client)
        {
            client.sessionID = p.m_bySessionID;

            if (p.uiSeqId > client.seqCounter)
                client.seqCounter = p.uiSeqId;

            WriteLog(10, "Handling packet...");

            var rmc = new RMCPacket(p);
            if (rmc.isRequest)
                HandleRequest(handler, client, p, rmc);
            else
                HandleResponse(handler, client, p, rmc);
        }

        public static void HandleResponse(QPacketHandlerPRUDP handler, QClient client, QPacket p, RMCPacket rmc)
        {
            ProcessResponse(client, p, rmc);
            WriteLog(1, "Received Response : " + rmc.ToString());
        }

        public static void ProcessResponse(QClient client, QPacket p, RMCPacket rmc)
        {
            WriteLog(1, "Got response for Protocol " + rmc.proto + " = " + (rmc.success ? "Success" : $"Fail : { rmc.error.ToString("X8") } for callID = { rmc.callID }"));
        }

        private static object[] HandleMethodParameters(MethodInfo method, Stream m)
		{
            // TODO: extended info
            var typeList = method.GetParameters().Select(x => x.ParameterType);

            return DDLSerializer.ReadPropertyValues(typeList.ToArray(), m);
        }

        public static void HandleRequest(QPacketHandlerPRUDP handler, QClient client, QPacket p, RMCPacket rmc)
        {
            MemoryStream m = new MemoryStream(p.payload);

			m.Seek(rmc._afterProtocolOffset, SeekOrigin.Begin);

            if (rmc.callID > client.callCounterRMC)
                client.callCounterRMC = rmc.callID;

            WriteLog(2, "Request to handle : " + rmc.ToString());

            string payload = rmc.PayLoadToString();

            if (payload != "")
                WriteLog(5, payload);

			var rmcContext = new RMCContext(rmc, handler, client, p);

			// create service instance
			var serviceFactory = RMCServiceFactory.GetServiceFactory(rmc.proto);

			if(serviceFactory != null)
			{
				var serviceInstance = serviceFactory();
				var bestMethod = serviceInstance.GetServiceMethodById(rmc.methodID);

				if (bestMethod != null)
				{
					// set the execution context
					serviceInstance.Context = rmcContext;

					// call method
					var parameters = HandleMethodParameters(bestMethod, m);
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
            string payload = reply.PayloadToString();

            if (payload != "")
                WriteLog(5, "Response Data Content : \n" + payload);

			handler.SendACK(p, client);
            SendResponsePacket(handler, p, rmc, client, reply, useCompression, error);
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
			rmc.callID = ++client.callCounterRMC;

			var rmcRequestData = rmc.ToBuffer();

			QPacket np = new QPacket(p.toBuffer());
			np.flags = new List<QPacket.PACKETFLAG>() { QPacket.PACKETFLAG.FLAG_RELIABLE | QPacket.PACKETFLAG.FLAG_NEED_ACK };
			np.m_uiSignature = client.IDsend;
			np.usesCompression = useCompression;

			handler.MakeAndSend(client, p, np, rmcRequestData);
		}

        private static void WriteLog(int priority, string s)
        {
            Log.WriteLine(priority, "[RMC] " + s);
        }

    }
}