using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMS
{
    [Serializable]
    public class Command
    {
        public string agentInterface;
        public int agentPort;
        public int inPort;
        public int inLabel;
        public int outPort;
        public int outLabel;

        public Command(string ai, int ap, int ip, int il, int op, int ol)
        {
            this.agentInterface = ai;
            this.agentPort = ap;
            this.inPort = ip;
            this.inLabel = il;
            this.outPort = op;
            this.outLabel = ol;
        }

        public Command()
        { }
    }
}
