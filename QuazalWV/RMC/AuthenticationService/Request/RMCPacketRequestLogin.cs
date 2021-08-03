using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuazalWV
{
    public class RMCPacketRequestLogin : RMCPRequest
    {
        public string userName;

        public RMCPacketRequestLogin()
        { 
        }

        public RMCPacketRequestLogin(Stream s)
        {
            userName = Helper.ReadString(s);
        }

        public override byte[] ToBuffer()
        {
            MemoryStream result = new MemoryStream();
            Helper.WriteString(result, userName);
            return result.ToArray();
        }

        public override string ToString()
        {
            return "[Login Request : userName=" + userName + "]";
        }

        public override string PayloadToString()
        {
            return "";
        }
    }
}
