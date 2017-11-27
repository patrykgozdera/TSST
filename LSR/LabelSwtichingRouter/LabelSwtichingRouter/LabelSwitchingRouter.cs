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
        List<InOutModule> inPorts;
        List<InOutModule> outPorts;
        int numberOfInputModules, numberOfOutputModules;

        public LabelSwitchingRouter()
        {
            fib = new FIB();
            numberOfInputModules = getInputModulesNumber();
            numberOfOutputModules = getOutputModulesNumber();
            inPorts = new List<InOutModule>();
            outPorts = new List<InOutModule>();

            createInModules(numberOfInputModules);
            createOutModules(numberOfOutputModules);
        }
        private int getInputModulesNumber()
        {
            int number = Config.getIntegerProperty("NumberOfInputPorts");
            return number;
        }
        private int getOutputModulesNumber()
        {
            int number = Config.getIntegerProperty("NumberOfOutputPorts");
            return number;

        }

        private void createOutModules(int numberOfOutputPorts)
        {
            for (int i = 0; i < numberOfOutputPorts; i++)
            {
                int portNumber = Config.getIntegerProperty("OutPortNumber" + i);
                OutModule outPort = new OutModule(portNumber);
                outPorts.Add(outPort);
            }

        }

        private void createInModules(int numberOfInputPorts)
        {
            for (int i = 0; i < numberOfInputPorts; i++)
            {
                int portNumber = int.Parse(Config.getProperty("InPortNumber" + i));
                InModule inPort = new InModule(portNumber, fib.ReturnSubTable(portNumber));
                inPorts.Add(inPort);
            }
        }

        public void passToInModule(object oSender, object received)
        {
            InModule inPort;
            int destinationPort;
            if (received.GetType() == typeof(Packet))
            {
                Packet receivedPacket = (Packet)received;
                int destinationInPort = getPortNumber(receivedPacket);
                inPort= getInPort(destinationInPort);
                inPort.processPacket(receivedPacket);
            }
            else if (received.GetType() == typeof(MPLSPack))
            {
                MPLSPack receivedPack = (MPLSPack)received;
                destinationPort=receivedPack.destinationPort;
                inPort = getInPort(destinationPort);
                inPort.processPack(receivedPack);
            }


        }
        private int getPortNumber(Packet receivedPacket)
        {
            int portNumber = receivedPacket.destinationPort;
            return portNumber;
        }
        private InModule getInPort(int portNumber)
        {
            InModule inPort = null;
            InModule comparedPort = null;
            for(int i=0; i<inPorts.Count; i++)
            {
                comparedPort = (InModule)inPorts.ElementAt(i);
                int comparedPortNumber = comparedPort.getPortNumber();
                if (comparedPortNumber == portNumber)
                    inPort = comparedPort;
            }
            return inPort;                
        }

    }
}
