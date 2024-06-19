using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace klient
{
    public partial class Form1 : Form
    {
        public String nick = "";
        private string serverIP = "_not_defined_";
        private string serverPort = "_not_defined_";

        public Form1()
        {
            InitializeComponent();
        }

        private bool ValidateContact(string txt)
        {
            int val;
            if (int.TryParse(txt, out val))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool Empty(string txt)
        {
            if (txt.Equals("")) 
                return true;
            return false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (nick != "")
            {
                serverIP = richTextBox3.Text; // ktos moze zrobic ctrl v wiec to tez tu powinno byc
                serverPort = richTextBox2.Text;
                
                if (Empty(serverIP))
                    serverIP = "127.0.0.1";
                if (Empty(serverPort))
                    serverPort = "5000";

                bool validPort1 = ValidateContact(serverPort);

                if (validPort1)
                {
                    Form form2 = new Form2(nick, serverIP, serverPort);
                    if (TcpClientApp.connected)
                    {
                        form2.Show();
                        Hide(); // form.Hide();
                    } else
                    {
                        MessageBox.Show($"[2406190014] Blad podlaczenia!!", "Blad!",
                                                         MessageBoxButtons.OK,
                                                         MessageBoxIcon.Question);
                    } 

                } else
                {
                    MessageBox.Show($"[2406190024] Blad podlaczenia!!", "Blad!",
                                                         MessageBoxButtons.OK,
                                                         MessageBoxIcon.Question);
                }
            }

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            nick = richTextBox1.Text;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Application.Exit(); // w tym moemencie w apliakcji nie ma żadnych watkow wiec moge tylko zamknac proces
            Environment.Exit(0);
        }

        private void richTextBox3_TextChanged(object sender, EventArgs e)
        {
            serverIP = richTextBox3.Text;
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            serverPort = richTextBox2.Text;
        }
    }
}
