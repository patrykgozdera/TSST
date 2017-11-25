using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelSwitchingRouter
{
    class InOutModule
    {
        protected static String portNumber;
        public InOutModule() { }
    }

    class InModule : InOutModule
    {
        private FIB fib;
        private string interfaceAddress;
        private List<FIB.Entry> list;

        public InModule(String number, FIB subMasterFIB)
        {
            portNumber = number;
            fib = subMasterFIB;
        }

        public InModule(string interfaceAddress, List<FIB.Entry> list)
        {
            this.interfaceAddress = interfaceAddress;
            this.list = list;
        }

        public List<MPLSPacket> Route(MPLSPack pack)
        {
            List<MPLSPacket> packets = UnpackPack(pack);
            packets.ForEach(ChangeLabel);
            return packets;
        }

        public List<MPLSPacket> UnpackPack(MPLSPack pack)
        {
            return pack.Unpack();
            //ForEach(ChangeLabel);

        }

        private void ChangeLabel(MPLSPacket packet)
        {
            Tuple<String, int> FIBOutput = fib.GetOutput(portNumber, packet.GetLabelFromStack());
            String port = FIBOutput.Item1;
            int label = FIBOutput.Item2;
            packet.PutLabelOnStack(label);
        }
    }

    class OutModule : InOutModule
    {
        List<MPLSPacket> packetBuffer;
        public OutModule(String number)
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



