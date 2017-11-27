using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelSwitchingRouter
{
    class InOutModule
    {
        protected int portNumber;
        public InOutModule() { }
    }

    class InModule : InOutModule
    {
        private FIB fib;
        private List<FIB.Entry> list;

        public InModule(int portNumber, FIB subMasterFIB)
        {
            this.portNumber = portNumber;
            fib = subMasterFIB;
        }

        public InModule(int portNumber, List<FIB.Entry> list)
        {
            this.portNumber = portNumber;
            this.list = list;
        }

        public List<MPLSPacket> Route(MPLSPack pack)
        {
            List<MPLSPacket> packets = UnpackPack(pack);
            //packets.ForEach(ChangeLabel);
            return packets;
        }
        
        public void processPacket(Packet packet)
        {

        }
        
        public void processPack(MPLSPack mplsPack)
        {

        }

        public int getPortNumber()
        {
            return portNumber;
        }

        public List<MPLSPacket> UnpackPack(MPLSPack pack)
        {
            return pack.Unpack();
            //ForEach(ChangeLabel);

        }
        /*
        private void ChangeLabel(MPLSPacket packet)
        {
            Tuple<string, int> FIBOutput = fib.GetOutput(InOutModule.portNumber, packet.GetLabelFromStack());
            String port = FIBOutput.Item1;
            int label = FIBOutput.Item2;
            packet.PutLabelOnStack(label);
        }
        */
    }
    
        class OutModule : InOutModule
        {
            List<MPLSPacket> packetBuffer;
            public OutModule(int number)
            {
                portNumber = number;
                packetBuffer = new List<MPLSPacket>();
            }

            public void addToBuffer(MPLSPacket packet)
            {
                packetBuffer.Add(packet);
            }

            public delegate void packIsReadyDelegate();
            public event packIsReadyDelegate sendPackage;

        }

    }
}



