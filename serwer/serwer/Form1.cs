using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace serwer
{
    public partial class Form1 : Form
    {

        String nick; 

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
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            nick = richTextBox1.Text;
        }
    }
}
