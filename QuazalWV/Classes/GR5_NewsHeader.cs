﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuazalWV
{
    public class GR5_NewsHeader
    {
        public uint m_ID;
        public uint m_recipientID;
        public uint m_recipientType;
        public uint m_publisherPID;
        public string m_publisherName;
        public ulong m_publicationTime;
        public ulong m_displayTime;
        public ulong m_expirationTime;
        public string m_title;
        public string m_link;
        public void toBuffer(Stream s)
        {
            Helper.WriteU32(s, m_ID);
            Helper.WriteU32(s, m_recipientID);
            Helper.WriteU32(s, m_recipientType);
            Helper.WriteU32(s, m_publisherPID);
            Helper.WriteString(s, m_publisherName);
            Helper.WriteU64(s, m_publicationTime);
            Helper.WriteU64(s, m_displayTime);
            Helper.WriteU64(s, m_expirationTime);
            Helper.WriteString(s, m_title);
            Helper.WriteString(s, m_link);
        }
    }
}
