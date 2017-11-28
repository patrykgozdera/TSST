using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelSwitchingRouter
{
    class MPLSPacket : MPLSObject
    {
        public Packet ipPacket;

        public MPLSPacket(Packet ipPacket, int label)
        {
            this.ipPacket = ipPacket;
            labelStack = new Stack<int>(label);
        }
    }
}
