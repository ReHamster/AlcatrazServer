﻿using QNetZ.DDL;
using QNetZ.Factory;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace QNetZ
{
	public static class Log
	{
		public delegate void LogPrintDelegate(int priority, string s, Color color);

		public static LogPrintDelegate LogFunction;

		public static int MinPriority = 10; //1..10 1=less, 10=all

		public static string LogFileName = "_RDV_log.txt";
		public static string LogPacketsFileName = "packetLog.bin";

		public static bool EnablePacketLogging = false;
		public static bool EnableFileLogging = true;

		private static readonly object _sync = new object();
		private static readonly object _filesync = new object();

		private static StringBuilder logBuffer = new StringBuilder();
		private static List<byte[]> logPackets = new List<byte[]>();

		public static void ClearLog()
		{
			if (File.Exists(LogFileName))
				File.Delete(LogFileName);

			if (File.Exists(LogPacketsFileName))
				File.Delete(LogPacketsFileName);

			lock (_sync)
			{
				logBuffer = new StringBuilder();
				logPackets = new List<byte[]>();
			}
		}

		public static void WriteLine(int priority, string s, object color = null)
		{
			if (LogFunction == null)
				return;

			try
			{
				string stamp = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + " : [" + priority.ToString("D2") + "]";

				string logstr = stamp + s;

				Color c;

				if (color != null)
					c = (Color)color;
				else
					c = Color.Black;

				if (s.ToLower().Contains("error"))
					c = Color.Red;

				if (priority <= MinPriority)
					LogFunction(priority, logstr, c);

				lock (_sync)
				{
					if(EnableFileLogging)
						logBuffer.Append(logstr + "\n");

					if(logBuffer.Length > 0 || logPackets.Count > 0)
					{
						new Thread(tSaveLog).Start();
					}
				}
			}
			catch { }
		}
		private static object HandleMethodParameters(MethodInfo method, Stream m)
		{
			// TODO: extended info
			var paramList = method.GetParameters();
			var typeList = paramList.Select(x => x.ParameterType);

			var values = DDLSerializer.ReadPropertyValues(typeList.ToArray(), m);

			var members = new Dictionary<string, object>();

			for (var i = 0; i < paramList.Count(); i++)
			{
				members[paramList[i].Name] = values[i];
			}

			return members;
		}

		public static string MakeDetailedPacketLog(byte[] data, bool isSinglePacket = false)
		{
			StringBuilder sb = new StringBuilder();
			while (true)
			{
				QPacket qp = new QPacket(data);
				sb.AppendLine("##########################################################");
				sb.AppendLine(qp.ToStringDetailed());
				if (qp.type == QPacket.PACKETTYPE.DATA && qp.m_byPartNumber == 0)
				{
					switch (qp.m_oSourceVPort.type)
					{
						case QPacket.STREAMTYPE.RVSecure:
							if (qp.flags.Contains(QPacket.PACKETFLAG.FLAG_ACK))
								break;
							sb.AppendLine("Trying to process RMC packet...");
							try
							{
								MemoryStream m = new MemoryStream(qp.payload);
								RMCPacket p = new RMCPacket(qp);

								m.Seek(p._afterProtocolOffset, SeekOrigin.Begin);

								string methodName = p.methodID.ToString();

								var serviceFactory = RMCServiceFactory.GetServiceFactory(p.proto);
								MethodInfo bestMethod = null;
								if(serviceFactory != null)
								{
									var service = serviceFactory();
									
									bestMethod = service.GetServiceMethodById(p.methodID);
									if (bestMethod != null)
										methodName = bestMethod.Name;
								}

								sb.AppendLine("\tRMC CallId : " + p.callID);
								sb.AppendLine("\tRMC Protocol : " + p.proto);
								sb.AppendLine("\tRMC Method   : " + methodName);

								if (p.isRequest)
								{
									sb.AppendLine("\tRMC Request  : " + p.isRequest);

									/*if (p.methodID == 1 && p.proto == RMCProtocolId.NotificationEventManager)
									{
										var notif = DDLSerializer.ReadObject<NotificationEvent>(m);
										sb.AppendLine(notif.ToString());
									}
									else */
									if (bestMethod != null)
									{
										sb.AppendLine("RMC Method arguments:");
										var paramValues = HandleMethodParameters(bestMethod, m);

										// serialize input parameters
										sb.Append(DDLSerializer.ObjectToString(paramValues));
									}
								}
								else
								{
									sb.AppendLine("\tRMC Response " + (p.success ? "Success" : $"Error : { p.error.ToString("X8") }"));
								}

								sb.AppendLine();
							}
							catch(Exception ex)
							{
								sb.AppendLine("Error processing RMC packet: " + ex.Message);
								sb.AppendLine();
							}
							break;
						case QPacket.STREAMTYPE.DO:
							if (qp.flags.Contains(QPacket.PACKETFLAG.FLAG_ACK))
								break;
							sb.AppendLine("Trying to unpack DO messages...");
							try
							{
								MemoryStream m = new MemoryStream(qp.payload);
								uint size = Helper.ReadU32(m);
								byte[] buff = new byte[size];
								m.Read(buff, 0, (int)size);
								DO.UnpackMessage(buff, 1, sb);
								sb.AppendLine();
							}
							catch
							{
								sb.AppendLine("Error processing DO messages");
								sb.AppendLine();
							}
							break;
					}
				}

				// sometimes happens that there are more packets in single UDP payload...

				int size2 = qp.toBuffer().Length;
				if (size2 == data.Length || isSinglePacket)
					break;

				var m2 = new MemoryStream(data);
				m2.Seek(size2, 0);
				size2 = (int)(m2.Length - m2.Position);

				if (size2 <= 8)
					break;

				data = new byte[size2];
				m2.Read(data, 0, size2);
			}
			return sb.ToString();
		}

		public static void LogPacket(bool sent, byte[] data)
		{
			if (!EnablePacketLogging)
				return;

			MemoryStream m = new MemoryStream();
			m.WriteByte(1);//version
			m.WriteByte((byte)(sent ? 1 : 0));
			Helper.WriteU32(m, (uint)data.Length);
			m.Write(data, 0, data.Length);

			lock (_sync)
			{
				logPackets.Add(m.ToArray());
			}
		}

		public static void tSaveLog(object obj)
		{
			lock (_filesync)
			{
				string buffer = null;
				lock (_sync)
				{
					buffer = logBuffer.ToString();
					logBuffer.Clear();
				}

				if (buffer != null && buffer.Length > 0)
					File.AppendAllText(LogFileName, buffer);

				byte[] packet = null;
				lock (_sync)
				{
					if (logPackets.Count != 0)
					{
						packet = logPackets[0];
						logPackets.RemoveAt(0);
					}
				}
				if (packet != null)
				{
					FileStream fs = new FileStream(LogPacketsFileName, FileMode.Append, FileAccess.Write);
					fs.Write(packet, 0, packet.Length);
					fs.Flush();
					fs.Close();
				}
				Thread.Sleep(1);
			}
		}
	}
}
