using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CableCloud
{
    class LinksRow
    {
        private String sourceNodeName;
        private int sourceNodePort;
        private String destinationNodeName;

        public LinksRow(String sourceNodeName, int sourceNodePort, String destinationNodeName)
        {
            this.sourceNodeName = sourceNodeName;
            this.sourceNodePort = sourceNodePort;
            this.destinationNodeName = destinationNodeName;
        }
        public String getSourceNodeName()
        {
            return sourceNodeName;
        }
        public int getSourceNodePort()
        {
            return sourceNodePort;
        }
        public String getDestinationNodeName()
        {
            return destinationNodeName;
        }
    }
}
