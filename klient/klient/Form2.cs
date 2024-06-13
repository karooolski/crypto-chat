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
            string message = messageBox.ToString();
            app.sendMessage(message, "abdul");
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        // w ten box wpisujesz goscia z ktorym piszesz 
        private void richTextBox3_TextChanged(object sender, EventArgs e)
        {
           //adresat = richTextBox3.Text;
        }
    }
}
