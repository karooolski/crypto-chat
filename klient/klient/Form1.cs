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
    public partial class Form1 : Form
    {
        public String nick = "";
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (nick != "")
            {
                Form form2 = new Form2(nick);
                form2.Show();
                Hide(); // form.Hide();
            }

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            nick = richTextBox1.Text;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
