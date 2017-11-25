using LabelSwtichingRouter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LabelSwitchingRouter
{
    class InputManager
    {
        private Socket inputSocket;

        public delegate void ReceivedDelegate(object oSender, PacketPackage packet);
        public event ReceivedDelegate ProcessPackage;

        public InputManager()
        {
            inputSocket = initalizeInputSocket();
        }
        private Socket initalizeInputSocket()
        {
            int port = Config.getIntegerProperty("CableCloudOutPort");
            String address = Config.getProperty("CableCloudAddress");
            Socket createdSocket = createSocket(address, port);
            return createdSocket;
        }
        private Socket createSocket(String address, int port)
        {
            IPEndPoint ipe = new IPEndPoint(long.Parse(address), port);
            return new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }
        public void waitForInput()
        {
            FireRecievedEvent(new PacketPackage());
        }
        public void FireRecievedEvent(PacketPackage package)
        {
            if (null != ProcessPackage)
                ProcessPackage(this, package);
        }
    }
}
