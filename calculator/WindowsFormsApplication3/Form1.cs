using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication3
{
    public partial class Form1 : Form
    {
        Double save1 = 0;
        
        string Pheptoan  = "";
        bool kiemtraso0 = true;
        bool kiemtradaucham = true;
        bool kiemtrapheptoan = true;
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_Click(object sender, EventArgs e)
        {
            if (kiemtraso0 || textBox1.Text=="0")
                textBox1.Clear();
            kiemtraso0=false;
            Button button = (Button)sender;
            textBox1.Text = textBox1.Text + button.Text;
        }

        private void operator_Click(object sender, EventArgs e)
        {
            if (kiemtrapheptoan)
            {
                Button button = (Button)sender;
                save1 = Double.Parse(textBox1.Text);
                label1.Text = (textBox1.Text) + " " + button.Text;
                Pheptoan = button.Text;
                kiemtraso0 = true;
               
                kiemtradaucham = true;
                kiemtrapheptoan = false;
            }
            else
            {
                Button button = (Button)sender;
                Pheptoan = button.Text;
                button15_Click(sender,e);
                label1.Text = (textBox1.Text) + " " + button.Text;
                save1 = Double.Parse(textBox1.Text);
                kiemtraso0 = true;

                kiemtrapheptoan = false;
                kiemtradaucham = true;
            }
        }

        private void btnce_Click(object sender, EventArgs e)
        {
            textBox1.Text = "0";
            kiemtraso0 = true;
            kiemtradaucham = true;
            kiemtrapheptoan = true;
        }

        private void btnC_Click(object sender, EventArgs e)
        {
            label1.Text = "";
            textBox1.Text = "0";
            save1 = 0;
            kiemtraso0 = true;
            kiemtradaucham = true;
            
        }

        private void button15_Click(object sender, EventArgs e)
        {
            label1.Text = "";
            switch (Pheptoan)
            { 
                case "+":
                   textBox1.Text = (save1 + Double.Parse(textBox1.Text)).ToString();
                    break;
                case "-":
                   textBox1.Text = (save1 - Double.Parse(textBox1.Text)).ToString();
                    break;
                case "X":
                    textBox1.Text = (Double.Parse(textBox1.Text) * save1).ToString();
                    break;
                case "/":
                    textBox1.Text = (save1 /Double.Parse(textBox1.Text)).ToString();
                    break;
                default: textBox1.Text = textBox1.Text; break;
            }
            kiemtraso0 = true;
            kiemtradaucham = true;
            kiemtrapheptoan = true;
            save1 = 0;
        }

        private void button17_Click(object sender, EventArgs e)
        {
            if (kiemtradaucham)
            {
                kiemtradaucham = false;
                kiemtraso0 = false;
                Button button = (Button)sender;
                textBox1.Text = textBox1.Text + button.Text;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button15_Click(sender, e);
            textBox1.Text=(1/Double.Parse(textBox1.Text)).ToString();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            button15_Click(sender, e);
            textBox1.Text=(Math.Sqrt(Double.Parse(textBox1.Text))).ToString();
        }

        
        private void button18_Click(object sender, EventArgs e)
        {

        }

        private void button20_Click(object sender, EventArgs e)
        {
            textBox1.Text = (-1*(Double.Parse(textBox1.Text))).ToString();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            textBox1.Text = (textBox1.Text).Substring(0, textBox1.Text.Length-1);
            if (textBox1.Text == "")
                textBox1.Text = "0";

        }

        
    }
}
