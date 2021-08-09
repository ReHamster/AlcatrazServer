using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QNetZ
{
    public abstract class DupObjPayload
    {
        public abstract byte[] toBuffer();

        public abstract string getDesc(string tabs = "");
    }
}
