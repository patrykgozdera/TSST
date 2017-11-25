using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelSwitchingRouter
{
    class InOutModule
    {

        protected static int portNumber;

        public InOutModule() { }


    }

    class InModule : InOutModule
    {
        private FIB fib;
        public InModule(FIB subMasterFIB) { }

        public void receivePack(MPLSPack pack)
        {
            pack.Unpack().ForEach(Route);


        }

        private void Route(MPLSPacket packet)
        {
            fib.GetOutput(portNumber, packet.GetLabelFromStack());

        }
    }

    class OutModule : InOutModule
    {
        List<MPLSPacket> packetBuffer;
        OutModule() {
            packetBuffer = new List<MPLSPacket>();
        }

        public void addToBuffer(MPLSPacket packet) {
            packetBuffer.Add(packet);

        }

    }


}
