using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CableCloud
{
    class OutSocket
    {
        private Socket outSocket;
        private String nodeName;
        private int port;

        public OutSocket(int port, String nodeName)
        {
            this.nodeName = nodeName;
            this.port = port;
            waitForConnection();

        }
        private void waitForConnection()
        {
            TcpListener listener = new TcpListener(IPAddress.Any,port);
            listener.Start();
            outSocket = listener.AcceptSocket();
            Console.WriteLine("connected");
        }
        public void Send(byte[] array)
        {
            outSocket.Send(array);
        }
        private Socket createSocket(int port)
        {
            IPAddress myAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint ipe = new IPEndPoint(myAddress, port);
            return new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }
        public String getNodeName()
        {
            return nodeName;
        }
        public Socket getOutputSocket()
        {
            return outSocket;
        }
    }
}
