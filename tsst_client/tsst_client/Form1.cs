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
        Socket client_socket = null;
        Socket listenerSocket = null;
        Socket foreignSocket = null;
        Message message = new Message();

        public Form1()
        {
            InitializeComponent();             
            if (!backgroundWorker1.IsBusy)
            {
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void Connect()                  //Połączenie
        {
            client_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipAdd = IPAddress.Parse("127.0.0.1");
            IPEndPoint remoteEP = new IPEndPoint(ipAdd, Int32.Parse(ConfigurationManager.AppSettings["client_port"]));
            client_socket.Connect(remoteEP);           
        }               

        private void send_Click(object sender, EventArgs e)
        {
            SendMessage();            
        }

        private void SendMessage()              //Wysyłanie. Wysyła określoną liczbę pakietów co określony odstęp czasu. 
        {
            Thread thread;
            thread = new Thread(async () =>
            {
                for(int i=1; i <= Int32.Parse(nb_of_m_tb.Text); i++)
                {
                    Connect();
                    client_socket.Send(GetSerializedObject());
                    logs_list.Invoke(new Action(delegate ()
                    {
                        logs_list.Items.Add(i + ": " + message.s + " | " + message.inti);
                        logs_list.SelectedIndex = logs_list.Items.Count - 1;
                    }));                    
                    //Thread.Sleep(Int32.Parse(delay_tb.Text));
                    await Task.Delay(Int32.Parse(delay_tb.Text));
                }
            }
            );
            thread.Start();

        }


        private byte[] GetSerializedObject()    //Serializacja bajtowa
        {
            string random_meassage;
            random_meassage = message_tb.Text + RandomString();
            message = new Message(random_meassage, 123, Int32.Parse(ConfigurationManager.AppSettings["client_port"]));
            //StreamWriter streamWriter = new StreamWriter(new Message(message_tb.Text, 123));
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, message);
            return ms.ToArray();
        }

        private void Listen()             //Socket nasłuchujący na przychodzące wiadomośc
        {
            listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipAdd = IPAddress.Parse("127.0.0.1");
            IPEndPoint remoteEP = new IPEndPoint(ipAdd, Int32.Parse(ConfigurationManager.AppSettings["foreign_port"]));
            listenerSocket.Bind(remoteEP);            

            while (true)
            {
                listenerSocket.Listen(0);                
                foreignSocket = listenerSocket.Accept();
                Thread t;
                t = new Thread(() => ReceiveMessage());
                t.Start();

            }
        }           


        private void ReceiveMessage()        //Deserializacja
        {
            byte[] bytes = new byte[foreignSocket.SendBufferSize];

            int readByte = foreignSocket.Receive(bytes);

            Message m = new Message();
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(bytes, 0, bytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            m = (Message)binForm.Deserialize(memStream);

            receive_logs_list.Items.Add(m.s);
            receive_logs_list.Items.Add(m.inti);                   
            
        }      

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)         //Backgrounworker - w tle nasłuchuje
        {
            try
            {
                Listen();            
            }
            catch(IOException)
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

       
  
        //////////////////////////////////////////////////////////////////
       



        private string GetSerializeXmlObject()      //Serializacja do XMLa
        {
            XmlSerializer xsSubmit = new XmlSerializer(typeof(Message));
            var subReq = new Message(message_tb.Text, 123, Int32.Parse(ConfigurationManager.AppSettings["client_port"]));
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
