using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CableCloud
{
    public class InputSocket
    {
        public Socket inputSocket = null;

        public InputSocket()
        { }

        public InputSocket(int p)
        {
            NewInputSocket(p);                            
        }

        public void NewInputSocket(int p)
        {
            inputSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipAdd = IPAddress.Parse("127.0.0.1");
            IPEndPoint remoteEP = new IPEndPoint(ipAdd, p);
            inputSocket.Bind(remoteEP);
        }
    }
}
