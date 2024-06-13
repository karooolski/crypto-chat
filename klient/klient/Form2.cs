using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace klient
{
    public partial class Form2 : Form
    {

        RichTextBox messageBox;
        TcpClientApp app;

        string nick = "";
        string adresat = ""; 

        public Form2()
        {
            InitializeComponent();

        }
        public Form2(string nickname)
        {
            InitializeComponent();
            nick = nickname;
            label1.Text = $"Witaj {nick}";
            richTextBox1.Text += "Uruchamianie klienta\n";
            app = new TcpClientApp(nick, "", 5000, ref richTextBox1);
            messageBox = richTextBox2;
            StopClient.stopClient = false;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string message = messageBox.Text;
            
            if(adresat!= "")
            {
                app.sendMessage(message, adresat,"message");
            }
            
        }

        // logout 
        private void button2_Click(object sender, EventArgs e)
        {
            string message = "action";
            string adresat = "serwer";
            string action = "logout";

            app.sendMessage(message, adresat, action);

            //app.stopTheTread();
            //StopClient.stopClient = true;
            Form form1 = new Form1();
            form1.Show();
            Close(); // form2.Close();
        }

        private void richTextBox3_TextChanged(object sender, EventArgs e)
        {
            adresat = richTextBox3.Text;
        }
    }

    public static class StopClient
    {
        public static bool stopClient = false;
    }

}
