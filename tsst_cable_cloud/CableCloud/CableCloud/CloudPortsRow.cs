using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CableCloud
{
    class CloudPortsRow
    {
        private String nodeName;
        private int inPort;
        private int outPort;

        public CloudPortsRow(String nodeName, int inPort, int outPort)
        {
            this.nodeName = nodeName;
            this.inPort = inPort;
            this.outPort = outPort;
        }
        public int getInPort()
        {
            return inPort;
        }
        public int getOutPort()
        {
            return outPort;
        }
        public String getNodeName()
        {
            return nodeName;
        }
    }
}
