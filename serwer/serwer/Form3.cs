using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security;

namespace serwer
{
    public partial class Form3 : Form
    {
        public static RichTextBox textboxGlobal = null;

        TcpServer serwer;

        public void startServer(RichTextBox richTextBox1, string port)
        {
            serwer = new TcpServer(ref richTextBox1, port);
            serwer.StartServer();
        }
        public Form3(string port)
        {
            InitializeComponent();
            richTextBox1.Text = "[Serwer] Witamy \n";
            textboxGlobal = richTextBox1;
            richTextBox2.Text = TcpServer.GetIPV4_2(); // GetServerIPV4
            richTextBox3.Text = TcpServer.GetDefaultGateway().ToString();
            richTextBox4.Text = port;

            Thread startserwer = new Thread(() => startServer(textboxGlobal, port));
            startserwer.Start();
        }


        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            // scroll it automatically
            richTextBox1.ScrollToCaret();
        }

        // Exit 
        private void button1_Click(object sender, EventArgs e) // https://stackoverflow.com/questions/13046019/winforms-application-exit-vs-environment-exit-vs-form-close
        {
            Environment.Exit(0); // wylacza wszyskkie watki itp
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textboxGlobal != null)
            {
                textboxGlobal.Text = $"[Serwer] Witamy \nAdres IP Serwera {serwer.ipServer}\nDefault gateway:{serwer.defaultGateway}\nSerwer nasłuchuje na porcie {serwer.port}...";
            }
        }

        // IPV4 ----------------------------------------------------------
        private void richTextBox2_TextChanged(object sender, EventArgs e) // richTextBox2
        {

        }

        // Default gateway ------------------------------------------------
        private void richTextBox3_TextChanged(object sender, EventArgs e)
        {

        }

        // port ---------------------------------------------------------
        private void richTextBox4_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
