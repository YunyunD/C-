using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace split_test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FileStream fs = new FileStream("data_test.txt", FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs, Encoding.Default);
            string str = sr.ReadToEnd();
            Console.WriteLine(str+"\r\n");
            textBox1.Text = str;
            sr.Close();
            //fileStream部分将文本内容全部读出到 str中

            string[] H_data = str.Split(new char[] { '\n', '\r' });//按行分割 后，为什么hdata的length 是行数两倍+1？ 且在器中奇数位没任何数据

            Console.WriteLine("H_data 0 is " + H_data[0] + "num0\r\n");
            Console.WriteLine("H_data 1 is " + H_data[1] + "num1\r\n");// 为什么hdata1中没任何数据？

            
            Console.WriteLine("str.length is " + str.Length + "\r\n");
            string[] data = new string[str.Length];
            //显示整体文本str.length
            

            /*
            string str1 = "2.36917E-05";
            float num1 = Convert.ToSingle(str1); //此段表明科学计数法可以直接通过convert来转成float
            Console.WriteLine(str1 + "is {0} \r\n", num1);
            */

            Console.WriteLine("H_data.length is " + H_data.Length);//H_data.Length 为数据行数
            int dataLength = H_data.Length / 2 + 1;
            float[] x = new float[dataLength];
            float[] y1 = new float[dataLength];
            float[] y2 = new float[dataLength];

            for (int i = 0; i < H_data.Length; i += 2)
            {
                // data = H_data[i].Split(new char[] { ' ' }); // 以空格为依据分割
                data = H_data[i].Split('\t');//根据TAB 进行分割 

                /* 检测data数据
                Console.WriteLine("data {0} is " + data[0] + "\r\n", i);
                Console.WriteLine("data {0} is " + data[1] + "\r\n", i);
                Console.WriteLine("data {0} is " + data[2] + "\r\n", i);
                Console.WriteLine("data length is " + data.Length + "\r\n");
                */
                
                for (int j = 0; j < data.Length; j++) {
                    switch (j) {
                        case 0:
                            x[i/2] = Convert.ToSingle(data[j]);
                            break;
                        case 1:
                            y1[i/2] = Convert.ToSingle(data[j]);
                            break;
                        case 2:
                            y2[i/2] = Convert.ToSingle(data[j]);
                            break;
                        default:
                            break;
                    }
                }
                
            }
            /*
            Console.WriteLine("x = {0} " + "\r\n", x[22]);
            Console.WriteLine("y1 = {0} " + "\r\n", y1[22]);
            Console.WriteLine("y2 = {0} " + "\r\n", y2[22]);
            */
            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            chart2.Series[0].Points.Clear();
            chart2.Series[1].Points.Clear();
            
            for (int i = 0; i < dataLength; i++) {
                chart1.Series[0].Points.AddXY(x[i], y1[i]);
                chart2.Series[1].Points.AddXY(x[i], y2[i]);
                chart1.Series[0].Color = Color.Red;
                chart2.Series[1].Color = Color.Blue;
                chart1.Series[0].BorderWidth = 1;
                chart2.Series[1].BorderWidth = 1;
            }
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {

        }
    }
}
