﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab1_Form
{
    public partial class FormEnterValues : Form
    {
        public int Number=0;
        public FormEnterValues()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {           
            if(!int.TryParse(textBox1.Text,out Number))            
            MessageBox.Show("Введите число");
        }
    }
}
