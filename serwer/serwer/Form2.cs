﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace serwer
{
    public partial class Form2 : Form
    {
        string nick;
        public Form2()
        {
            InitializeComponent();
        }
        public Form2(string nickname)
        {
            InitializeComponent();
            nick = nickname;
            label1.Text = $"Witaj {nick}";
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }
    }
}
