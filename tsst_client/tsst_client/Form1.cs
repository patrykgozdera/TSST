using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace tsst_client
{
    public partial class Form1 : Form
    {
        Socket client_socket = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void connect_Click(object sender, EventArgs e)
        {
            client_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipAdd = IPAddress.Parse("127.0.0.1");
            IPEndPoint remoteEP = new IPEndPoint(ipAdd, Int32.Parse(ConfigurationManager.AppSettings["client_port"]));
            client_socket.Connect(remoteEP);
        }

        private void send_Click(object sender, EventArgs e)
        {
            SendMessage();
            logs_list.Items.Add("Message send: " +message_tb.Text);
        }

        private void SendMessage()
        {            
            client_socket.Send(GetSerializedObject());

        }

        private byte[] GetSerializedObject()
        {
            Message message = new Message(message_tb.Text, 123);
            //StreamWriter streamWriter = new StreamWriter(new Message(message_tb.Text, 123));
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, message);
            return ms.ToArray();
        }

        private string GetSerializeXmlObject()
        {
            XmlSerializer xsSubmit = new XmlSerializer(typeof(Message));
            var subReq = new Message(message_tb.Text, 123);
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
