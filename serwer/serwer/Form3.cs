using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace serwer
{
    public partial class Form3 : Form
    {
        public static RichTextBox textboxGlobal = null;

        public void startServer(RichTextBox richTextBox1)
        {
            TcpServer serwer = new TcpServer(ref richTextBox1);
            serwer.StartServer();
        }
        public Form3()
        {
            InitializeComponent();
            richTextBox1.Text = "[Serwer] Witamy \n";
            textboxGlobal = richTextBox1;

            Thread startserwer = new Thread(() => startServer(textboxGlobal));
            startserwer.Start();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        // Exit 
        private void button1_Click(object sender, EventArgs e) // https://stackoverflow.com/questions/13046019/winforms-application-exit-vs-environment-exit-vs-form-close
        {
            Environment.Exit(0); // wylacza wszyskkie watki itp
        }
    }
}
