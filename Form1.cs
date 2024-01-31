using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laba_auto
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox1.Text = "⊥";
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public void button1_Click(object sender, EventArgs e)
        {
            string s = textBox1.Text;
            try
            {
                string[] resultParse = Analyzer.Parse(s);
                textBox3.Text = "Идентификаторы" + Environment.NewLine + resultParse[0];
                textBox4.Text = "Тип" + Environment.NewLine + resultParse[1];
                textBox5.Text = "Память" + Environment.NewLine + resultParse[2];
                textBox2.Clear();
            }
            catch (ExceptionWithPosition err)
            {
                textBox2.Text = err.Message;
                textBox1.Text = err.Stext;
                textBox1.SelectionStart = err.Position;
                textBox1.Focus();
                textBox1.SelectionStart = err.Position;
            }
            catch (Exception err)
            {
                textBox2.Text = err.Message;
                textBox1.Text = s;
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint_1(object sender, PaintEventArgs e)
        {

        }
    }
}
