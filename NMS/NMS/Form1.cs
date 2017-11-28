using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NMS
{
    public partial class Form1 : Form
    {
        static Socket output_socket = null;
        static Socket inputSocket = null;
        static Socket foreignSocket = null;
        public Command inCommand = new Command();
        public Command outCommand = new Command();
        private int outputPort;

        public Form1()
        {
            InitializeComponent();            
            if (!backgroundWorker1.IsBusy)
            {
                backgroundWorker1.RunWorkerAsync();
            }            
        }

        private void update_b_Click(object sender, EventArgs e)
        {
            outCommand.agentInterface = "Add";
            outCommand.agentPort = comboBox1.SelectedIndex;
            outCommand.inPort = Int32.Parse(textBox_ip.Text);
            outCommand.inLabel = Int32.Parse(textBox_il.Text);
            outCommand.outPort = Int32.Parse(textBox_op.Text);
            outCommand.outLabel = Int32.Parse(textBox_ol.Text);
            SendSingleCommand(outCommand);
        }
        private void delete_b_Click(object sender, EventArgs e)
        {
            outCommand.agentInterface = "Delete";
            outCommand.agentPort = comboBox1.SelectedIndex;
            outCommand.inPort = Int32.Parse(textBox_ip.Text);
            outCommand.inLabel = Int32.Parse(textBox_il.Text);
            outCommand.outPort = Int32.Parse(textBox_op.Text);
            outCommand.outLabel = Int32.Parse(textBox_ol.Text);
            SendSingleCommand(outCommand);
        }

        public void Listen()
        {
            inputSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipAdd = IPAddress.Parse("127.0.0.1");
            IPEndPoint remoteEP = new IPEndPoint(ipAdd, 7386);
            inputSocket.Bind(remoteEP);
            int i = 1;
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
                    outputPort = inCommand.agentPort;
                    comboBox1.Invoke(new Action(delegate ()
                    {
                    comboBox1.Items.Add(inCommand.agentPort);
                    }
                    ));
                    listBox2.Invoke(new Action(delegate ()
                    {
                        listBox2.Items.Add(inCommand.agentInterface + " " + inCommand.agentPort);
                        listBox2.SelectedIndex = listBox1.Items.Count - 1;
                    }));

                    if(inCommand.agentInterface != null)
                    {
                        ParseConfig();
                    }
                    i++;
                }
                );
                t.Start();
            }
        }

        private void SendSingleCommand(Command cm)
        {
            Thread tr;
            tr = new Thread(() =>
            {
                Connect();
                output_socket.Send(GetSerializedCommand(cm));
                listBox1.Invoke(new Action(delegate ()
                {
                    listBox1.Items.Add(cm.inPort + " " + cm.inLabel + " " + cm.outPort + " " + cm.outLabel);
                    listBox1.SelectedIndex = listBox1.Items.Count - 1;
                }));
            }
            );
            tr.Start();
        }

        private void Connect()                  //Połączenie
        {
            output_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipAdd = IPAddress.Parse("127.0.0.1");
            IPEndPoint remoteEP = new IPEndPoint(ipAdd, outputPort); //Int32.Parse(ConfigurationManager.AppSettings["output_port"]));
            output_socket.Connect(remoteEP);
        }


        private void ParseConfig()
        {
            
                string line;
                StreamReader file = new StreamReader("C:\\Users\\Patryk-student ELKI\\Documents\\TSST\\tsst_nms\\NMS\\NMS\\bin\\Debug\\" + inCommand.agentInterface + ".txt");
                while ((line = file.ReadLine()) != null)
                {
                    string[] integerStrings = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    
                    outCommand.inPort = Int32.Parse(integerStrings[0]);
                    outCommand.inLabel = Int32.Parse(integerStrings[1]);
                    outCommand.outPort = Int32.Parse(integerStrings[2]);
                    outCommand.outLabel = Int32.Parse(integerStrings[3]);
                    SendSingleCommand(outCommand);
                    listBox2.Invoke(new Action(delegate ()
                    {
                        listBox2.Items.Add(outCommand.inPort + " " + outCommand.inLabel + " " + outCommand.outPort + " " + outCommand.outLabel);
                        listBox2.SelectedIndex = listBox1.Items.Count - 1;
                    }));
                }
            
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

        private byte[] GetSerializedCommand(Command com)    //Serializacja bajtowa
        {            
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, com);
            return ms.ToArray();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Listen();
            }
            catch (IOException)
            {
                listBox1.Invoke(new Action(delegate ()
                {
                    listBox1.Items.Add("Problem with communication");
                }));
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                MessageBox.Show("Processing cancelled", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(e.Result.ToString(), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
            private int GetPort()
        {
            int p = 0;
            for (int i = 1; i <= Config.getIntegerProperty("NbOfRouters"); i++)
            {
                if (inCommand.agentInterface == Config.getProperty("Agent" + i.ToString()))
                {
                    p = inCommand.agentPort;
                    outCommand.inPort = Config.getIntegerProperty("InPort" + i.ToString());
                    outCommand.inLabel = Config.getIntegerProperty("InLabel" + i.ToString());
                    outCommand.outPort = Config.getIntegerProperty("OutPort" + i.ToString());
                    outCommand.outLabel = Config.getIntegerProperty("OutLabel" + i.ToString());
                }
            }                       
            return p;
        }
                
    }
}
