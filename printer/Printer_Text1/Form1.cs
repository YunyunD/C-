using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Printer_Text1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] array_str = new string[7];

            array_str[0] = textBox1.Text;
            array_str[1] = textBox2.Text;
            array_str[2] = textBox3.Text;
            array_str[3] = textBox4.Text;
            array_str[4] = textBox5.Text;
            array_str[5] = textBox6.Text;
            array_str[6] = textBox7.Text;

            string path = @"d:\TEST\PrinterTest.txt";
            string creatText = "Printer Test" + Environment.NewLine;

            if (!File.Exists(path))
            {
                File.WriteAllText(path, creatText, Encoding.UTF8);

            }
            else
            {
                File.WriteAllText(path, creatText);
            }

            array_str[0] = "打印机:         " + array_str[0] + Environment.NewLine;
            array_str[1] = "标签宽度:       " + array_str[1] + "mm" + Environment.NewLine;
            array_str[2] = "标签高度:       " + array_str[2] + "mm" + Environment.NewLine;
            array_str[3] = "标签垂直间距:   " + array_str[3] + "mm" + Environment.NewLine;
            array_str[4] = "起始位置X:      " + array_str[4] + "P" + Environment.NewLine;
            array_str[5] = "起始位置Y:      " + array_str[5] + "p" + Environment.NewLine;
            array_str[6] = "字体高度:       " + array_str[6] + "P" + Environment.NewLine;

            for (int i = 0; i < 7; i++) {
                File.AppendAllText(path, array_str[i]);
            }

            try
            {
                Process[] processes = Process.GetProcesses();
                foreach (Process p in processes)
                {
                    if (p.MainWindowTitle == "PrinterTest.txt - 记事本")
                    {
                        p.Kill();
                    }
                }
            }
            catch (Exception)
            {
            }
            System.Diagnostics.Process.Start(path);

            textBox8.Text = "";
            for (int i = 0; i < 7; i++)
            {
                textBox8.Text=textBox8.Text+array_str[i];
            }

        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void 显示_Click(object sender, EventArgs e)
        {
           

        }
    }
}
