using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelSwitchingRouter
{
    class InPort
    {
        protected int portNumber;
        private FIB fib;
        private List<FIB.Entry> list;

        public InPort(int portNumber, FIB subMasterFIB)
        {
            this.portNumber = portNumber;
            fib = subMasterFIB;
        }

        public MPLSPacket processPacket(Packet packet)
        {
            MPLSPacket labeledMPLS = SetLabel(packet);
            return labeledMPLS;
        }

        public List<MPLSPacket> processPack(MPLSPack mplsPack)
        {
            List<MPLSPacket> packets = UnpackPack(mplsPack);
            packets.ForEach(ChangeLabel);
            return packets;
        }

        public List<MPLSPacket> UnpackPack(MPLSPack pack)
        {
            return pack.Unpack();
        }

        private MPLSPacket SetLabel(Packet packet)
        {
            int label = fib.ExchangeIpAddressForLabel(packet.destinationAddress);
            MPLSPacket mplspacket = new MPLSPacket(packet, label);
            ChangeLabel(mplspacket);
            return mplspacket;
        }

        private void ChangeLabel(MPLSPacket packet)
        {
            int[] FIBOutput = fib.GetOutput(packet.destinationPort, packet.GetLabelFromStack());
            int port = FIBOutput[0];
            int label = FIBOutput[1];
            packet.destinationPort = port;
            packet.PutLabelOnStack(label);
        }

        public int getPortNumber()
        {
            return portNumber;
        }
    }

    class OutPort
    {
        protected int portNumber;
        List<MPLSPacket> packetBuffer;

        public int GetPortNumber { get; internal set; }

        public OutPort(int number)
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




