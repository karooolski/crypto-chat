using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace serwer
{
    public partial class Form1 : Form
    {

        String nick;
        string port;

        public Form1()
        {
            InitializeComponent();
            richTextBox1.Text = "5000";
            //richTextBox2.Text = TcpServer.GetDefaultGateway().ToString(); //GetServerIPV4();
            richTextBox2.Text = TcpServer.GetIPV4_2();
        }

        public static string GetServerIPV4()
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());

            foreach (IPAddress address in ipHostInfo.AddressList)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                    return address.ToString();
            }

            return string.Empty;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            port = richTextBox1.Text;
            if (port != "")
            {
                Form form3 = new Form3(port);
                form3.Show();
                Hide();
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            port = richTextBox1.Text;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox2_TextChanged(object sender, EventArgs e) // richTextBox2
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
