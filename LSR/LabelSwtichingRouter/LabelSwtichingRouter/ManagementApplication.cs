using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LabelSwitchingRouter
{
    class ManagementApplication
    {
        Socket MAInputSocket;
        IPAddress ipAddress;

        private void Listen()
        {
            MAInputSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            String ipString = Config.getProperty("NMSAdress");
            ipAddress = IPAddress.Parse(ipString);
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, Int32.Parse(Config.getProperty("NMSPort")));
            MAInputSocket.Bind(remoteEP);

            while (true)
            {
                MAInputSocket.Listen(0);
                Socket MAOutputSocket = MAInputSocket.Accept();
                Thread t;
                t = new Thread(() => ReceiveMessage(MAOutputSocket));
                t.Start();
            }
        }

        private void ReceiveMessage(Socket clientSocket)
        {
            byte[] bytes = new byte[clientSocket.SendBufferSize];

            int readByte = clientSocket.Receive(bytes);

            Message m = new Message();
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(bytes, 0, bytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            m = (Message)binForm.Deserialize(memStream);

            Console.WriteLine(m.s);
            Console.WriteLine(m.inti);
            Console.WriteLine(m.port);

            Console.WriteLine("Finished.");
            Console.ReadKey();
        }

    }


}
