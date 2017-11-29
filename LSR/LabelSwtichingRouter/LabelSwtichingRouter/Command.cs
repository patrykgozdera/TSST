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
        public string agentId;
        public int agentPort;
        public int inPort;
        public int inLabel;
        public int outPort;
        public int outLabel;
        public int newLabel;
        public int removeLabel;
        public string ipAdress;

        public Command(string ai, int ap, int ip, int il, int op, int ol, int nl, int rl, string ipa)
        {
            agentId = ai;
            agentPort = ap;
            inPort = ip;
            inLabel = il;
            outPort = op;
            outLabel = ol;
            newLabel = nl;
            removeLabel = rl;
            ipAdress = ipa;
        }

        public Command()
        { }
    }
}