using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelSwitchingRouter
{
    [Serializable]
    class MPLSPacket
    {
        public Packet ipPacket;
        public List<int> labelStack;

        public MPLSPacket(Packet ipPacket, int label)
        {
            this.ipPacket = ipPacket;
            labelStack = new List<int>(label);
        }
    }
}
