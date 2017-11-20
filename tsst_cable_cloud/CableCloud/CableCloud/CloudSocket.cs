﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using tsst_client;

namespace CableCloud
{
    public class CloudSocket
    {
        static Socket output_socket = null;
        static Socket inputSocket = null;
        static Socket foreignSocket = null;
        public Message messageOut = new Message();
        public Message messageIn = new Message();
        List<string> lines = new List<string>();
        List<int> lines2 = new List<int>();

        public CloudSocket()
        {
            Console.Title = "Cable Cloud";
            ParseConfig();
        }

        public void Listen()
        {
            inputSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipAdd = IPAddress.Parse("127.0.0.1");
            IPEndPoint remoteEP = new IPEndPoint(ipAdd, 20000);
            inputSocket.Bind(remoteEP);
            int i = 1;
            while(true)
            {
                inputSocket.Listen(0);
                foreignSocket = inputSocket.Accept();
                byte[] bytes = new byte[foreignSocket.SendBufferSize];
                int readByte = foreignSocket.Receive(bytes);

                Thread t;
                t = new Thread(() =>
                {
                    messageIn = GetDeserializedMessage(bytes);
                    Console.WriteLine(i + ": " + messageIn.s + " | " + messageIn.output_port + " | " + messageIn.timestamp);
                    SendSingleMessage(messageIn);
                    i++;
                }
                );
                t.Start();
            }
        }

        private void SendSingleMessage(Message sm)
        {
            Thread tr;
            tr = new Thread(() =>
            {
                Connect();                
                output_socket.Send(GetSerializedMessage(sm));                              
            }
            );
            tr.Start();
        }

        private void Connect()                  //Połączenie
        {
            output_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipAdd = IPAddress.Parse("127.0.0.1");
            IPEndPoint remoteEP = new IPEndPoint(ipAdd, GetPort()); //Int32.Parse(ConfigurationManager.AppSettings["output_port"]));
            output_socket.Connect(remoteEP);
        }

        private int GetPort()
        {
            int p = 0; 
            if (messageIn._interface == lines[0] && messageIn.output_port == lines2[0])
                p = lines2[1];
            else if (messageIn._interface == lines[2] && messageIn.output_port == lines2[2])
                p = lines2[3];

            return p;
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
            //StreamWriter streamWriter = new StreamWriter(new Message(message_tb.Text, 123));
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, mes);
            return ms.ToArray();
        }

        private void ParseConfig()
        {

            TextReader tr1 = new StreamReader("cable_cloud.txt");           
            while (tr1.Peek() >= 0)
            {
                lines.Add(tr1.ReadLine());
                lines2.Add(Int32.Parse(tr1.ReadLine()));
            }       
               // Console.WriteLine(lines[0] + lines[1] + lines[2] + lines[3]);
                //Console.WriteLine(lines2[0] +","+ lines2[1] + "," + lines2[2] + "," + lines2[3]);

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
