using System;
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
    public partial class FormEnterVules : Form
    {
        private string varName;
        public int result;
        public bool check;
        public FormEnterVules(string varName)
        {
            InitializeComponent();
            this.varName = varName;
            label1.Text = "Введите значение переменной " + varName + " :";
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try { result = int.Parse(textBox1.Text); }
            catch(Exception ex)
            {
                MessageBox.Show("Something wrong"+ex.ToString());
            }
            Close();
        }
    }
}
