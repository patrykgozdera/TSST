using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace tsst_client
{
    public partial class Form1 : Form
    {
        static Socket output_socket = null;
        static Socket inputSocket = null;
        static Socket foreignSocket = null;
        Message messageOut = new Message();
        Message messageIn = new Message();

        public Form1()
        {
            InitializeComponent();
        }

        private void Connect()                  //Połączenie
        {
            output_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipAdd = IPAddress.Parse("127.0.0.1");
            IPEndPoint remoteEP = new IPEndPoint(ipAdd, Int32.Parse(textBox1.Text)); //Int32.Parse(ConfigurationManager.AppSettings["output_port"]));
            output_socket.Connect(remoteEP);

        }

        private void button1_Click(object sender, EventArgs e)
        {

            Connect();
            if (!backgroundWorker1.IsBusy)
            {
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void send_Click(object sender, EventArgs e)
        {
            SendMessage(messageOut);
        }

        private void SendMessage(Message packet)              //Wysyłanie. Wysyła określoną liczbę pakietów co określony odstęp czasu. 
        {
            string random_meassage;
            Thread thread;
            thread = new Thread(async () =>
            {
                for (int i = 1; i <= Int32.Parse(nb_of_m_tb.Text); i++)
                {
                    random_meassage = message_tb.Text + RandomString();
                    packet = new Message(random_meassage, Int32.Parse(textBox2.Text), Int32.Parse(textBox1.Text), GetTimeStamp(), "I1");
                    //Connect();
                    if (output_socket.Connected)
                    {
                        int messageSize = GetSerializedMessage(packet).Length;
                        byte[] mSize = BitConverter.GetBytes(messageSize);

                        output_socket.Send(mSize);
                        output_socket.Send(GetSerializedMessage(packet));
                        logs_list.Invoke(new Action(delegate ()
                        {
                            logs_list.Items.Add(i + ": " + messageSize + "|" + packet.s + " | " + packet.output_port + " | " + packet.timestamp);
                            logs_list.SelectedIndex = logs_list.Items.Count - 1;
                        }));

                    }

                    //Thread.Sleep(Int32.Parse(delay_tb.Text));
                    await Task.Delay(Int32.Parse(delay_tb.Text));
                }
            }
            );
            thread.Start();
        }

        private void SendSingleMessage(Message sm)
        {
            Thread tr;
            tr = new Thread(() =>
            {
                Connect();
                if (sm._interface == "I1")             //
                    sm._interface = "I2";              //do usunięcia (testy)
                else if (sm._interface == "I2")        //
                    sm._interface = "I3";
                else
                    sm._interface = "I1";
                output_socket.Send(GetSerializedMessage(sm));
                logs_list.Invoke(new Action(delegate ()
                {
                    logs_list.Items.Add(": " + sm.s + " | " + sm.output_port + " | " + sm.timestamp);
                    logs_list.SelectedIndex = logs_list.Items.Count - 1;
                }));
            }
            );
            tr.Start();
        }

        private void Listen()             //Socket nasłuchujący na przychodzące wiadomośc
        {
            inputSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipAdd = IPAddress.Parse("127.0.0.1");
            IPEndPoint remoteEP = new IPEndPoint(ipAdd, Int32.Parse(textBox2.Text)); //Int32.Parse(ConfigurationManager.AppSettings["input_port"]));
            inputSocket.Connect(remoteEP);
            int i = 1;

            while (true)
            {
                //inputSocket.Listen(0);                
                //foreignSocket = inputSocket.Accept();
                byte[] objectSize = new byte[4];
                inputSocket.Receive(objectSize, 0, 4, SocketFlags.None);
                int messageSize = BitConverter.ToInt32(objectSize, 0);

                byte[] bytes = new byte[messageSize];
                int readByte = inputSocket.Receive(bytes, 0, messageSize, SocketFlags.None);
                Thread t;
                t = new Thread(() =>
                {
                    messageIn = GetDeserializedMessage(bytes);
                    receive_logs_list.Invoke(new Action(delegate ()
                    {
                        receive_logs_list.Items.Add(i + ": " + messageSize + "|" + messageIn.s + " | " + messageIn.output_port + " | " + messageIn.timestamp);
                        receive_logs_list.SelectedIndex = receive_logs_list.Items.Count - 1;
                    }));
                    //SendSingleMessage(messageIn);
                    i++;
                }
                );
                t.Start();

            }
        }

        private byte[] GetSerializedMessage(Message mes)    //Serializacja bajtowa
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, mes);
            return ms.ToArray();
        }

        private Message GetDeserializedMessage(byte[] b)        //Deserializacja
        {
            Message m = new Message();
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(b, 0, b.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            m = (Message)binForm.Deserialize(memStream);
            return m;
        }




        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)         //Backgrounworker - w tle nasłuchuje
        {
            try
            {
                Listen();
            }
            catch (IOException)
            {
                logs_list.Invoke(new Action(delegate ()
                {
                    logs_list.Items.Add("Problem with communication");
                }));
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)   //obsługa BGW
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

        private string RandomString()       //Dokleja randomowego stringa z tej puli o randomowej długości z danego przedziału (tak ma być)
        {
            Random random = new Random();
            int string_length = random.Next(1, 5);
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, string_length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private string GetTimeStamp()
        {
            DateTime dateTime = DateTime.Now;
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss.ff");
        }


        //////////////////////////////////////////////////////////////////




        private string GetSerializeXmlObject()      //Serializacja do XMLa
        {
            XmlSerializer xsSubmit = new XmlSerializer(typeof(Message));
            var subReq = new Message(message_tb.Text, 123, Int32.Parse(ConfigurationManager.AppSettings["client_port"]), GetTimeStamp(), "I1");
            //Message mes = new Message();
            //mes.s = message_tb.Text;

            var sw = new StringWriter();
            XmlTextWriter writer = new XmlTextWriter(sw);
            xsSubmit.Serialize(writer, subReq);
            var xml = sw.ToString();
            return xml;
        }

    }
}
