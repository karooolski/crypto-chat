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
            MessagePort messagePort = new MessagePort(message, adresat, "message");

            if (adresat != "")
            {
                app.sendMessage(messagePort);
            }

        }

        // logout 
        private void button2_Click(object sender, EventArgs e)
        {
            string message = "action";
            string adresat = "serwer";
            string action = "logout";

            MessagePort messagePort = new MessagePort(nick, message, adresat, action);

            app.sendMessage(messagePort);

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

        private void label2_Click(object sender, EventArgs e)
        {

        }

        // request crypted chat
        private void button3_Click(object sender, EventArgs e)
        {
            string action = "requestEncryptedChat";
            int range = 10000; // 1 - 10 000 z tego range bedzie wybierana liczba pierwsza \
            int generated_prime_number = PrimeNumberGenerator.generate(range);
            string message = $"{nick}: Wyslales uzytkownikowi {adresat} prosbe o rozpoczecie szyfrowanego czatu, liczba {generated_prime_number}";
            MessagePort messagePort = new MessagePort(nick, message, adresat, action, generated_prime_number);
            app.sendMessage(messagePort);
            
        }
    }

    public static class StopClient
    {
        public static bool stopClient = false;
    }

}
