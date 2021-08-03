﻿using QuazalWV.Attributes;
using QuazalWV.Factory;
using QuazalWV.Helpers;
using QuazalWV.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;

namespace QuazalWV
{
	public static class RMC
    {
        public static void HandlePacket(UdpClient udp, QPacket p)
        {
            ClientInfo client = Global.GetClientByIDrecv(p.m_uiSignature);
            if (client == null)
                return;
            client.sessionID = p.m_bySessionID;
            if (p.uiSeqId > client.seqCounter)
                client.seqCounter = p.uiSeqId;
            client.udp = udp;
            if (p.flags.Contains(QPacket.PACKETFLAG.FLAG_ACK))
                return;
            WriteLog(10, "Handling packet...");
            RMCP rmc = new RMCP(p);
            if (rmc.isRequest)
                HandleRequest(client, p, rmc);
            else
                HandleResponse(client, p, rmc);
        }

        public static void HandleResponse(ClientInfo client, QPacket p, RMCP rmc)
        {
            ProcessResponse(client, p, rmc);
            WriteLog(1, "Received Response : " + rmc.ToString());
        }

        public static void ProcessResponse(ClientInfo client, QPacket p, RMCP rmc)
        {
            MemoryStream m = new MemoryStream(p.payload);
            m.Seek(rmc._afterProtocolOffset, 0);
            rmc.success = m.ReadByte() == 1;
            if (rmc.success)
            {
                rmc.callID = Helper.ReadU32(m);
                rmc.methodID = Helper.ReadU32(m);
            }
            else
            {
                rmc.error = Helper.ReadU32(m);
                rmc.callID = Helper.ReadU32(m);
            }
            WriteLog(1, "Got response for Protocol " + rmc.proto + " = " + (rmc.success ? "Success" : "Fail"));
        }

        private static object[] HandleMethodParameters(MethodInfo method, Stream m)
		{
            // TODO: extended info
            var typeList = method.GetParameters().Select(x => x.ParameterType);

            return DDLHelper.ReadPropertyValues(typeList.ToArray(), m);
        }

        public static void HandleRequest(ClientInfo client, QPacket p, RMCP rmc)
        {
            MemoryStream m = new MemoryStream(p.payload);

            m.Seek(rmc._afterProtocolOffset, 0);
            rmc.callID = Helper.ReadU32(m);
            rmc.methodID = Helper.ReadU32(m);

            if (rmc.callID > client.callCounterRMC)
                client.callCounterRMC = rmc.callID;

            WriteLog(2, "Request to handle : " + rmc.ToString());

            string payload = rmc.PayLoadToString();

            if (payload != "")
                WriteLog(5, payload);

            var rmcContext = new RMCContext(rmc, client, p);

            // create service instance
            var serviceInstance = RMCServiceFactory.GetServiceInstance(rmc.proto);
            if (serviceInstance != null)
            {
                // set the execution context
                serviceInstance.Context = rmcContext;

                MethodInfo bestMethod = null;

                // find suitable method
                var allMethods = serviceInstance.GetType().GetMethods();
                foreach (var method in allMethods)
                {
                    var rmcMethodAttr = (RMCMethodAttribute)method.GetCustomAttributes(typeof(RMCMethodAttribute), true).SingleOrDefault();
                    if (rmcMethodAttr != null)
                    {
                        if (rmcMethodAttr.MethodId == rmc.methodID)
                        {
                            bestMethod = method;
                            break;
                        }
                    }
                }

                // call method
                if (bestMethod != null)
                {
                    var parameters = HandleMethodParameters(bestMethod, m);
                    bestMethod.Invoke(serviceInstance, parameters);

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

        public static void SendResponseWithACK(UdpClient udp, QPacket p, RMCP rmc, ClientInfo client, RMCPResponse reply, bool useCompression = true, uint error = 0)
        {
            WriteLog(2, "Response : " + reply.ToString());
            string payload = reply.PayloadToString();
            if (payload != "")
                WriteLog(5, "Response Data Content : \n" + payload);
            SendACK(udp, p, client);
            SendResponsePacket(udp, p, rmc, client, reply, useCompression, error);
        }

        private static void SendACK(UdpClient udp, QPacket p, ClientInfo client)
        {
            QPacket np = new QPacket(p.toBuffer());
            np.flags = new List<QPacket.PACKETFLAG>() { QPacket.PACKETFLAG.FLAG_ACK };
            np.m_oSourceVPort = p.m_oDestinationVPort;
            np.m_oDestinationVPort = p.m_oSourceVPort;
            np.m_uiSignature = client.IDsend;
            np.payload = new byte[0];
            np.payloadSize = 0;
            WriteLog(10, "send ACK packet");
            Send(udp, np, client);
        }

        private static void SendResponsePacket(UdpClient udp, QPacket p, RMCP rmc, ClientInfo client, RMCPResponse reply, bool useCompression, uint error)
        {
            MemoryStream m = new MemoryStream();
            if ((ushort)rmc.proto < 0x7F)
            {
                Helper.WriteU8(m, (byte)rmc.proto);
            }
            else
            {
                Helper.WriteU8(m, 0x7F);
                Helper.WriteU16(m, (ushort)rmc.proto);
            }
            byte[] buff;
            if (error == 0)
            {
                Helper.WriteU8(m, 0x1);
                Helper.WriteU32(m, rmc.callID);
                Helper.WriteU32(m, rmc.methodID | 0x8000);
                buff = reply.ToBuffer();
                if(buff != null) m.Write(buff, 0, buff.Length);                
            }
            else
            {
                Helper.WriteU8(m, 0);
                Helper.WriteU32(m, error);
                Helper.WriteU32(m, rmc.callID);
            } 
            buff = m.ToArray();
            m = new MemoryStream();
            Helper.WriteU32(m, (uint)buff.Length);
            m.Write(buff, 0, buff.Length);
            QPacket np = new QPacket(p.toBuffer());
            np.flags = new List<QPacket.PACKETFLAG>() { QPacket.PACKETFLAG.FLAG_NEED_ACK };
            np.m_oSourceVPort = p.m_oDestinationVPort;
            np.m_oDestinationVPort = p.m_oSourceVPort;
            np.m_uiSignature = client.IDsend;
            MakeAndSend(client, np, m.ToArray());
        }
        
        public static void SendRequestPacket(UdpClient udp, QPacket p, RMCP rmc, ClientInfo client, RMCPResponse packet, bool useCompression, uint error)
        {
            MemoryStream m = new MemoryStream();
            if ((ushort)rmc.proto < 0x7F)
                Helper.WriteU8(m, (byte)((byte)rmc.proto | 0x80));
            else
            {
                Helper.WriteU8(m, 0xFF);
                Helper.WriteU16(m, (ushort)rmc.proto);
            }
            byte[] buff;
            if (error == 0)
            {
                Helper.WriteU32(m, rmc.callID);
                Helper.WriteU32(m, rmc.methodID);
                buff = packet.ToBuffer();
                m.Write(buff, 0, buff.Length);
            }
            else
            {
                Helper.WriteU32(m, error);
                Helper.WriteU32(m, rmc.callID);
            }
            buff = m.ToArray();
            m = new MemoryStream();
            Helper.WriteU32(m, (uint)buff.Length);
            m.Write(buff, 0, buff.Length);
            QPacket np = new QPacket(p.toBuffer());
            np.flags = new List<QPacket.PACKETFLAG>() { QPacket.PACKETFLAG.FLAG_NEED_ACK };
            np.m_uiSignature = client.IDsend;
            MakeAndSend(client, np, m.ToArray());
        }

        public static void MakeAndSend(ClientInfo client, QPacket np, byte[] data)
        {
            MemoryStream m = new MemoryStream(data);
            if (data.Length < 0x3C3)
            {
                np.uiSeqId++;
                np.payload = data;
                np.payloadSize = (ushort)np.payload.Length;
                WriteLog(10, "sent packet");
                Send(client.udp, np, client);
            }
            else
            {
                np.flags.Add(QPacket.PACKETFLAG.FLAG_HAS_SIZE);
                int pos = 0;
                m.Seek(0, 0);
                np.m_byPartNumber = 0;
                while (pos < data.Length)
                {
                    np.uiSeqId++;
                    bool isLast = false;
                    int len = 0x3C3;
                    if (len + pos >= data.Length)
                    {
                        len = data.Length - pos;
                        isLast = true;
                    }
                    if (!isLast)
                        np.m_byPartNumber++;
                    else
                        np.m_byPartNumber = 0;
                    byte[] buff = new byte[len];
                    m.Read(buff, 0, len);
                    np.payload = buff;
                    np.payloadSize = (ushort)np.payload.Length;
                    Send(client.udp, np, client);
                    pos += len;
                }
                WriteLog(10, "sent packets");
            }
        }

        public static void Send(UdpClient udp, QPacket p, ClientInfo client)
        {
            byte[] data = p.toBuffer();
            StringBuilder sb = new StringBuilder();
            foreach (byte b in data)
                sb.Append(b.ToString("X2") + " ");
            WriteLog(5, "send : " + p.ToStringShort());
            WriteLog(10, "send : " + sb.ToString());
            WriteLog(10, "send : " + p.ToStringDetailed());
            udp.Send(data, data.Length, client.ep);
            Log.LogPacket(true, data);
        }

        public static void SendNotification(ClientInfo client, uint source, uint type, uint subType, uint param1, uint param2, uint param3, string paramStr)
        {
            WriteLog(1, "Send Notification: [" + source.ToString("X8") + " " 
                                         + type.ToString("X8") + " "
                                         + subType.ToString("X8") + " " 
                                         + param1.ToString("X8") + " "
                                         + param2.ToString("X8") + " "
                                         + param3.ToString("X8") + " \""
                                         + paramStr + "\"]");
            MemoryStream m = new MemoryStream();
            Helper.WriteU32(m, source);
            Helper.WriteU32(m, type * 1000 + subType);
            Helper.WriteU32(m, param1);
            Helper.WriteU32(m, param2);
            Helper.WriteU16(m, (ushort)(paramStr.Length + 1));
            foreach (char c in paramStr)
                m.WriteByte((byte)c);
            m.WriteByte(0);
            Helper.WriteU32(m, param3);
            byte[] payload = m.ToArray();
            QPacket q = new QPacket();
            q.m_oSourceVPort = new QPacket.VPort(0x31);
            q.m_oDestinationVPort = new QPacket.VPort(0x3f);
            q.type = QPacket.PACKETTYPE.DATA;
            q.flags = new List<QPacket.PACKETFLAG>();
            q.payload = new byte[0];
            q.uiSeqId = (ushort)(++client.seqCounter);
            q.m_bySessionID = client.sessionID;
            RMCP rmc = new RMCP();
            rmc.proto = RMCP.PROTOCOL.NotificationEventManager;
            rmc.methodID = 1;
            rmc.callID = ++client.callCounterRMC;
            RMCPCustom reply = new RMCPCustom();
            reply.buffer = payload;
            RMC.SendRequestPacket(client.udp, q, rmc, client, reply, true, 0);
        }

        private static void WriteLog(int priority, string s)
        {
            Log.WriteLine(priority, "[RMC] " + s);
        }

    }
}
