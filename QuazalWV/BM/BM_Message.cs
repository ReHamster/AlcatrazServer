﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuazalWV
{
    public abstract class BM_Message
    {
        public byte msgType = 0xA;
        public ushort msgID;
        public List<BM_Param> paramList = new List<BM_Param>();
        public static byte[] Make(BM_Message msg)
        {
            MemoryStream m = new MemoryStream();
            Helper.WriteU8(m, msg.msgType);
            Helper.WriteU16LE(m, msg.msgID);
            byte[] buff;
            foreach (BM_Param p in msg.paramList)
                switch (p.type)
                {
                    case BM_Param.PARAM_TYPE.Integer:
                        Helper.WriteU8(m, 0);
                        Helper.WriteU32LE(m, (uint)(int)p.data);
                        break;
                    case BM_Param.PARAM_TYPE.Float:
                        Helper.WriteU8(m, 0);
                        Helper.WriteFloatLE(m, (float)p.data);
                        break;
                    case BM_Param.PARAM_TYPE.Buffer:
                        buff = (byte[])p.data;
                        Helper.WriteU8(m, 0x80);
                        Helper.WriteU16LE(m, (ushort)buff.Length);
                        m.Write(buff, 0, buff.Length);
                        break;
                }
            buff = m.ToArray();
            m = new MemoryStream();
            Helper.WriteU16(m, (ushort)(buff.Length + 2));
            Helper.WriteU16(m, (ushort)buff.Length);
            m.Write(buff, 0, buff.Length);
            Helper.WriteU32(m, 0);
            return m.ToArray();
        }

        public static byte[] HandleMessage(QClient client, Stream s)
        {
			ClientInfo ci = client.info;

			if (Helper.ReadU16(s) < 5 || Helper.ReadU16(s) < 3)
                return null;
            byte type = Helper.ReadU8(s);
            if (type != 0xA)
                return null;
            ushort msgID = (ushort)((Helper.ReadU8(s) << 8) | Helper.ReadU8(s));
            List<byte[]> msgs = new List<byte[]>();

            switch(msgID)
            {
                case 0x96:
                    return Entitiy_CMD.HandleMsg(client, s);
                case 0x99:
                    Helper.ReadU8(s);
                    ushort size = Helper.ReadU16LE(s);
                    byte[] payload = new byte[size];
                    s.Read(payload, 0, size);
                    payload[0x11] = 0;
                    payload[0x12] = 0x27;
                    msgs.Add(DO_RMCRequestMessage.Create(client.seqCounterOut++,
                        0x1006,
                        new DupObj(DupObjClass.Station, 1),
                        new DupObj(DupObjClass.NET_MessageBroker, 5),
                        (ushort)DO_RMCRequestMessage.DOC_METHOD.ProcessMessage,
                        Make(new MSG_ID_SendReplicaData(payload))
                        ));
                    break;
                case 0xA3:
                    if (!ci.playerCreateStuffSent1)
                    {
                        msgs.Add(DO_RMCRequestMessage.Create(client.seqCounterOut++,
                            0x1006,
                            new DupObj(DupObjClass.Station, 1),
                            new DupObj(DupObjClass.NET_MessageBroker, 5),
                            (ushort)DO_RMCRequestMessage.DOC_METHOD.ProcessMessage,
                            Make(new MSG_ID_Net_Obj_Create(0x2C, 0x15, new OCP_AbstractPlayerEntity(1).MakePayload()))
                            ));
                        msgs.Add(DO_RMCRequestMessage.Create(client.seqCounterOut++,
                            0x1006,
                            new DupObj(DupObjClass.Station, 1),
                            new DupObj(DupObjClass.NET_MessageBroker, 5),
                            (ushort)DO_RMCRequestMessage.DOC_METHOD.ProcessMessage,
                            Make(new MSG_ID_Entity_Cmd(ci, 0x33))
                            ));
						ci.playerCreateStuffSent1 = true;
                    }
                    msgs.Add(DO_RMCRequestMessage.Create(client.seqCounterOut++,
                        0x1006,
                        new DupObj(DupObjClass.Station, 1),
                        new DupObj(DupObjClass.NET_MessageBroker, 5),
                        (ushort)DO_RMCRequestMessage.DOC_METHOD.ProcessMessage,
                        Make(new MSG_ID_NetRule_Synchronize(ci.netRulesState))
                        ));
                    break;
                case 0x266:
                    break;
                case 0x325:
                    msgs.Add(DO_RMCRequestMessage.Create(client.seqCounterOut++,
                        0x1006,
                        new DupObj(DupObjClass.Station, 1),
                        new DupObj(DupObjClass.NET_MessageBroker, 5),
                        (ushort)DO_RMCRequestMessage.DOC_METHOD.ProcessMessage,
                        Make(new MSG_ID_BM_StartRound())
                        ));
                    break;
            }
            if (msgs.Count > 0)
                return DO_BundleMessage.Create(ci, msgs);
            else
                return null;
        }
    }
}
