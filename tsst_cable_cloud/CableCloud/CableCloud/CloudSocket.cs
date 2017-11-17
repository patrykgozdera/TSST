using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using tsst_client;

namespace CableCloud
{
    public class CloudSocket
    {
        public CloudSocket()
        {
            this.ReceiveMessage();
        }

        private void ReceiveMessage()
        {
            Socket clListenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipAdd = IPAddress.Parse("127.0.0.1");
            IPEndPoint remoteEP = new IPEndPoint(ipAdd, 20000);
            clListenerSocket.Bind(remoteEP);
            clListenerSocket.Listen(0);
            Socket clientSocket = clListenerSocket.Accept();
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

            Console.WriteLine("Finished.");
            Console.ReadKey();
           
        }



        /*
        private void ReceiveXmlMessage()
        {
            Socket clListenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipAdd = IPAddress.Parse("127.0.0.1");
            IPEndPoint remoteEP = new IPEndPoint(ipAdd, 20000);
            clListenerSocket.Bind(remoteEP);
            clListenerSocket.Listen(0);
            Socket clientSocket = clListenerSocket.Accept();
            byte[] bytes = new byte[clientSocket.SendBufferSize];

            int readByte = clientSocket.Receive(bytes);
            byte[] rData = null; 

            for (int i=0; i <= readByte; i++)
            {
                rData = new byte[readByte];
                Array.Copy(bytes, rData, readByte);
            }
                Console.WriteLine("From client: " + System.Text.Encoding.UTF8.GetString(rData));
            do
            {
                readByte = clientSocket.Receive(bytes);
                byte[] rData = new byte[readByte];
                Array.Copy(bytes, rData, readByte);
                Console.WriteLine("From client: " + System.Text.Encoding.UTF8.GetString(rData));
             } while (readByte > 0);

            Message m = new Message();
            XmlSerializer serializer = new XmlSerializer(typeof(Message));
            StringReader reader = new StringReader(System.Text.Encoding.UTF8.GetString(rData));
            //string ij = reader.ReadToEnd();
            // var subReq = (Message) serializer.Deserialize(reader);

            m = (Message)serializer.Deserialize(reader);
           
            Console.WriteLine(m.s);
            Console.WriteLine(m.inti);
            
            Console.WriteLine("Finished.");
            Console.ReadKey();

        }*/
    }
}
