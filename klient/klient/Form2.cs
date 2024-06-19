using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace klient
{
    public partial class Form2 : Form
    {
        string plainText;
        RichTextBox messageBox;
        TcpClientApp app;

        string nick = "";
        string adresat = "";
        string serverIP = "127.0.0.1";
        int serverPort = 5000;
        string myIP = "None";

        public Form2(string nickname, string _serverIP_, string _serverPort_)
        {
            InitializeComponent();
            //myIP = TcpClientApp.GetDefaultGateway().ToString();
            myIP = TcpClientApp.GetIPV4_2();

            if (_serverIP_ == "_not_defined_" || _serverIP_ == "")
            {
                serverIP = "127.0.0.1";
            }
            else
            {
                serverIP = _serverIP_;
            }
            if (_serverPort_ == "_not_defined_" || _serverPort_ == "")
            {
                serverPort = 5000; //Int32.Parse(_serverPort_);
            }
            else
            {
                try
                {
                    serverPort = Int32.Parse(_serverPort_); ; //Input string was not in a correct format.”
                }
                catch (Exception e)
                {
                    serverPort = 5000;
                }

            }
            string _ipv4_ = TcpClientApp.GetIPV4_2();
            string default_gateway = TcpClientApp.GetDefaultGateway().ToString();
            richTextBox5.Text = $"myIPV4: {_ipv4_}\nserverIP:{serverIP}\nport:{serverPort}\ndefault_gatwway: {default_gateway}";
            nick = nickname;
            label1.Text = $"Witaj {nick}";
            string msgtxt = $"Uruchamianie klienta\nIP:{serverIP}, port:{serverPort}";
            richTextBox1.AppendText(msgtxt);
            app = new TcpClientApp(nick, serverIP, serverPort, ref richTextBox1, myIP);
            messageBox = richTextBox2;
            StopClient.stopClient = false;
        }

        public void initialiseConnection()
        {
           
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            // scroll it automatically
            richTextBox1.ScrollToCaret();
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            plainText = richTextBox2.Text;
        }
        // send message --------------------------------------------------------------------------
        private void button1_Click(object sender, EventArgs e)
        {
            if (!TcpClientApp.IsConnected())
            {
                MessageBox.Show($"[Form2] [2406190127] Brak polaczania z serwerem! {serverIP}, \n lastError: {TcpClientApp.lastErrorMessage}", "Blad!",
                                                         MessageBoxButtons.OK,
                                                         MessageBoxIcon.Question);
                return; 
            }
            if (TcpClientApp.servrBreakup)
            {
                // skoro ledwo co sie podlaczylem do serwera ponownie porpzez IsConnected powyzej
                // to nie moge znowu pisac wiadomosci bo wtedy bufor dostanie z tego jednego watku: nick IsConnected + json message stad 
                // choc wazna tu jest kwesita czasu bo po tym pop upie juz bylo wszystko ok i nie musialem robic return nawet xd

                MessageBox.Show($"[TclCientApp]: Pomyslnie polaczono z serwerem, prosze wyslac wiadomosc jeszcze raz!", "Blad!",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Question);
                TcpClientApp.servrBreakup = false;
                return;
            }
            plainText = messageBox.Text;
            if (adresat == "")
            {
                return;
            }
            DiffieHellmanData diffieHellman = TcpClientApp.getMyDataAbout(adresat);

            TcpClientApp.gloabalStream.Flush();

            if (diffieHellman == null) // czat nie jest szyfrowany wiec tylko wysylam wiadomosc 
            {
                MessagePort newMessage = new MessagePort(nick, plainText, adresat, myIP);
                app.sendMessage(newMessage);
            }
            else if (diffieHellman != null)
            {
                if (diffieHellman.allowEncryptedChat)
                {
                    string key = diffieHellman.getK().ToString(); ;
                    string ecnrypted = AES.Encrypt(plainText, key);
                    MessagePort newMessage = new MessagePort(nick, ecnrypted, adresat, myIP);
                    app.sendCryptoMessage(newMessage, plainText);
                }
                else
                {
                   MessagePort newMessage = new MessagePort(nick, plainText, adresat, myIP);
                   app.sendMessage(newMessage);
                }
            }
        }

        // logout --------------------------------------------
        private void button2_Click(object sender, EventArgs e)
        {
            string message = "action";
            string adresat = "serwer";
            string action = "logout";
            if (TcpClientApp.checkClientConnectionWithoutTryToLogIn())  // IsConnected() // lepiej uzyc statycznej zmiennej, IsConnected robi tez proble polaczenia // a to tutaj jest nie potrzben
            {
                MessagePort messagePort = new MessagePort(nick, message, adresat, action, myIP);
                app.sendMessage(messagePort);
            }
            Form form1 = new Form1();
            form1.Show();
            Close(); // form2.Close();
        }

        // adresat -------------------------------------------------------
        private void richTextBox3_TextChanged(object sender, EventArgs e)
        {
            adresat = richTextBox3.Text;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        // request crypted chat -------------------------------
        private void button3_Click(object sender, EventArgs e)
        {
            if (!TcpClientApp.IsConnected())
            {
                MessageBox.Show($"[Form2] [2406190131] Brak polaczania z serwerem! {serverIP} \nlastError: {TcpClientApp.lastErrorMessage}", "Blad!",
                                                         MessageBoxButtons.OK,
                                                         MessageBoxIcon.Question);
                return;
            }

            if (TcpClientApp.servrBreakup)
            {
                MessageBox.Show($"[TclCientApp]: Pomyslnie polaczono z serwerem, prosze wyslac wiadomosc jeszcze raz!", "Blad!",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Question);
                TcpClientApp.servrBreakup = false;
                return;
            }

            if (adresat == "")
            {
                MessageBox.Show($"Prosze wprowadzic nazwe uzytkownika", "Informacja!",
                                                         MessageBoxButtons.OK,
                                                         MessageBoxIcon.Information);
                return;
            }
            string action = "requestEncryptedChat";
            int range = 10000; // 1 - 10 000 z tego range bedzie wybierana liczba pierwsza \
            BigInteger generated_prime_number = PrimeNumberGenerator.generate(range);
            string prime_number_str = generated_prime_number.ToString();
            string message = $"{nick}: Wyslales uzytkownikowi {adresat} prosbe o rozpoczecie szyfrowanego czatu, liczba {generated_prime_number}, jako string {prime_number_str}";

            MessagePort messagePort = new MessagePort(nick, message, adresat, action, prime_number_str, myIP);
            app.sendMessage(messagePort);

        }

        private void richTextBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        // informacja dla uzytkownika o ip i port serwera do jakiego jest pdolaczony ----
        private void richTextBox5_TextChanged(object sender, EventArgs e) // richTextBox5
        {

        }
    }

    public static class StopClient
    {
        public static bool stopClient = false;
    }

}
