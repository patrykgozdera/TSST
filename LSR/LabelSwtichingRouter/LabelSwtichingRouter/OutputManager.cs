using LabelSwitchingRouter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LabelSwitchingRouter
{
    class OutputManager
    {
        private Socket outputSocket;

        public OutputManager()
        {
            outputSocket = initializeOutputSocket();
        }
        private Socket initializeOutputSocket()
        {
            int port = Config.getIntegerProperty("CableCloudInPort");
            String address = Config.getProperty("CableCloudAddress");
            Socket createdSocket = createSocket(address, port);
            return createdSocket;
        }
        private Socket createSocket(String address, int port)
        {
            IPEndPoint ipe = new IPEndPoint(long.Parse(address), port);
            return new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }
    }
}
    
