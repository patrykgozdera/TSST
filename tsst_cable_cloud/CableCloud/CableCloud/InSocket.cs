using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using tsst_client;

namespace CableCloud
{
    public class InSocket
    {
        static Socket inputSocket = null;
        public Message messageOut = new Message();
        public Message messageIn = new Message();

        private string nodeName;
        private int port;


        public InSocket()
        {
            //Console.Title = "Cable Cloud";            
        }

        public InSocket(int _port, String associatedNodeName)
        {
            //GetNewInSocket(_port);
            nodeName = associatedNodeName;
            port = _port;
            ListenForConnection();
        }

        private void GetNewInSocket(int p)
        {
            inputSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, p);
            //inputSocket.Bind(remoteEP);
        }
        public void ListenForConnection()
        {
            TcpListener listener = new TcpListener(port);
            listener.Start();
            inputSocket = listener.AcceptSocket();
            Console.WriteLine("connected");
        }

        public void ListenForIncomingData()
        {

            int i = 1;
            while (true)
            {
                byte[] objectSize = new byte[4];
                inputSocket.Receive(objectSize, 0, 4, SocketFlags.None);
                int messageSize = BitConverter.ToInt32(objectSize, 0);
                Console.WriteLine(messageSize);

                byte[] bytes = new byte[messageSize];
                inputSocket.Receive(bytes, 0, messageSize, SocketFlags.None);
                Console.WriteLine("Received packet from node: " + nodeName);


                /*Thread t;
                t = new Thread(() =>
                {
                    messageIn = GetDeserializedMessage(bytes);
                    Console.WriteLine(i + ": " + messageIn.s + " | " + messageIn.output_port + " | " + messageIn.timestamp);
                    //SendSingleMessage(messageIn);
                    i++;
                }
                );
                t.Start()
                */
            }
        }



        private Message GetDeserializedMessage(byte[] b)
        {
            Message m = new Message();
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(b, 0, b.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            m = (Message)binForm.Deserialize(memStream);
            return m;
        }

        private byte[] GetSerializedMessage(Message mes)    //Serializacja bajtowa
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, mes);
            return ms.ToArray();
        }

    }
}