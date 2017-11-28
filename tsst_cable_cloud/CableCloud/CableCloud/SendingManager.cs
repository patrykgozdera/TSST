using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CableCloud
{
    class SendingManager
    {
        private static List<OutSocket> outSockets;

        public static void init()
        {
            outSockets = new List<OutSocket>();
        }

        public static void addOutSocket(OutSocket outSocket)
        {
            outSockets.Add(outSocket);
        }
        //to ma byc synchronizowane
        public static void Send(byte[] dataToSend, String sourceNode, int sourcePort)
        {
            String destinationNodeName = ConnectionsTable.getDestinationNodeName(sourceNode, sourcePort);
            OutSocket outputSocket = getAssociatedSocket(destinationNodeName);
            outputSocket.Send(dataToSend);
        }

        private static OutSocket getAssociatedSocket(string destinationNodeName)
        {
            OutSocket socketToReturn = null;
            String nodeName = null;
            for (int i=0; i<outSockets.Count; i++)
            {
                OutSocket outputSocket = outSockets.ElementAt(i);
                nodeName = outputSocket.getNodeName();
                if (nodeName.Equals(destinationNodeName))
                    socketToReturn = outputSocket;
            }
            return socketToReturn;
        }
    }
}
