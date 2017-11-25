using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelSwitchingRouter
{

    class MPLSPack : MPLS
    {
        private int destinationPort = -1;
        private List<MPLSPacket> packets;

        MPLSPack(List<MPLSPacket> packets)
        {
            this.packets = packets;


        }


        public List<MPLSPacket> Unpack()
        {
            return packets;
        }

    }
}
