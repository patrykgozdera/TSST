using LabelSwitchingRouter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelSwitchingRouter
{
    class LabelSwitchingRouter
    {
        FIB fib;
        List<InPort> inPorts;
        List<OutPort> outPorts;
        int numberOfInputModules, numberOfOutputModules;

        public LabelSwitchingRouter()
        {
            fib = new FIB();
            numberOfInputModules = GetInputModulesNumber();
            numberOfOutputModules = GetOutputModulesNumber();
            inPorts = new List<InPort>();
            outPorts = new List<OutPort>();

            CreateInPorts(numberOfInputModules);
            CreateOutPorts(numberOfOutputModules);
        }

        private int GetInputModulesNumber()
        {
            int number = Config.getIntegerProperty("NumberOfInputPorts");
            return number;
        }
        private int GetOutputModulesNumber()
        {
            int number = Config.getIntegerProperty("NumberOfOutputPorts");
            return number;

        }

        private void CreateOutPorts(int numberOfOutputPorts)
        {
            for (int i = 0; i < numberOfOutputPorts; i++)
            {
                int portNumber = Config.getIntegerProperty("OutPortNumber" + i);
                OutPort outPort = new OutPort(portNumber);
                outPorts.Add(outPort);
            }

        }

        private void CreateInPorts(int numberOfInputPorts)
        {
            for (int i = 0; i < numberOfInputPorts; i++)
            {
                int portNumber = int.Parse(Config.getProperty("InPortNumber" + i));
                InPort inPort = new InPort(portNumber, fib.ReturnSubTable(portNumber));
                inPorts.Add(inPort);
            }
        }

        public void RouteIncoming(object oSender, object received) //jak to nazwać?
        {
            InPort inPort;
            int destinationPort;
            if (received.GetType() == typeof(Packet))
            {
                Packet receivedPacket = (Packet)received;
                destinationPort = GetPortNumber(receivedPacket);
                inPort = GetInPort(destinationPort);
                MPLSPacket processedPacket = inPort.processPacket(receivedPacket);
            }
            else if (received.GetType() == typeof(MPLSPack))
            {
                MPLSPack receivedPack = (MPLSPack)received;
                destinationPort = receivedPack.destinationPort;
                inPort = GetInPort(destinationPort);
                List<MPLSPacket> processedPackets = inPort.processPack(receivedPack);
                foreach (MPLSPacket packet in processedPackets) {
                    Commutate(packet);
                } 
            }
        }

        private int GetPortNumber(Packet receivedPacket)
        {
            int portNumber = receivedPacket.destinationPort;
            return portNumber;
        }

        private InPort GetInPort(int portNumber)
        {
            foreach (InPort port in inPorts)
            {
                int comparedPortNumber = port.getPortNumber();
                if (comparedPortNumber == portNumber)
                    return port;
            }
            return null;
        }

        private void Commutate(MPLSPacket packet)
        {
            int packetOutPort = packet.destinationPort;
            int portNumber;
            foreach (OutPort port in outPorts)
            {
                portNumber = port.GetPortNumber;
                if (packetOutPort == portNumber)
                {
                    port.addToBuffer(packet);
                    return;
                }
            }
        }
      
    }

}
