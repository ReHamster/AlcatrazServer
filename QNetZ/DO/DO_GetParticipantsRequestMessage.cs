using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QNetZ
{
    public static class DO_GetParticipantsRequestMessage
    {
        public static byte[] HandleMessage(QClient client, byte[] data)
        {
            QLog.WriteLine(2, "[DO] Handling DO_GetParticipantsRequestMessage...");
            return DO_GetParticipantsResponseMessage.Create(data);
        }
    }
}
