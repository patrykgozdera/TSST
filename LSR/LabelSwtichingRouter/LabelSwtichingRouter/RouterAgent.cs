﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMS;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace LabelSwitchingRouter
{
    class RouterAgent
    {
        static Socket output_socket = null;
        static Socket inputSocket = null;
        static Socket foreignSocket = null;
        public Command inCommand, outCommand;
        private List<InPort> inPorts;
        private FIB fib;
        private int outport;
        private string _interface;

        public RouterAgent(FIB fib, List<InPort> inPorts)
        {
            this.fib = fib;
            inCommand = new Command();
            outCommand = new Command();
            this.inPorts = inPorts;
            _interface = Config.getProperty("NMSInterface");
            outport = Config.getIntegerProperty("NMSListenPort");

            SendSingleCommand(outCommand);

            inputSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipAdd = IPAddress.Parse("127.0.0.1");
            IPEndPoint remoteEP = new IPEndPoint(ipAdd, outport);
            inputSocket.Bind(remoteEP);

            Listen();
        }

        public void Listen()
        {
            while (true)
            {
                inputSocket.Listen(0);
                foreignSocket = inputSocket.Accept();
                byte[] bytes = new byte[foreignSocket.SendBufferSize];
                int readByte = foreignSocket.Receive(bytes);

                Thread t;
                t = new Thread(() =>
                {
                    inCommand = GetDeserializedCommand(bytes);
                    fib.AddEntry(inCommand.inPort, inCommand.inLabel, inCommand.outPort, inCommand.outLabel, inCommand.newLabel, inCommand.removeLabel, inCommand.ipAdress);
                    fib.UpdatePortsRoutingTables(inPorts);
                }
                );
                t.Start();



            }
        }

        private void SendSingleCommand(Command cm)
        {
            cm = new Command(_interface, outport, 0, 0, 0, 0, 0, 0, "null");
            Thread tr;
            tr = new Thread(() =>
            {
                output_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ipAdd = IPAddress.Parse("127.0.0.1");
                IPEndPoint remoteEP = new IPEndPoint(ipAdd, 7386);
                output_socket.Connect(remoteEP);
                output_socket.Send(GetSerializedCommand(cm));
            }
            );
            tr.Start();
        }

        private Command GetDeserializedCommand(byte[] b)
        {
            Command c = new Command();
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(b, 0, b.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            c = (Command)binForm.Deserialize(memStream);
            return c;
        }

        private byte[] GetSerializedCommand(Command com)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, com);
            return ms.ToArray();
        }
    }
}
