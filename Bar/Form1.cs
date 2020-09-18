using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ThoughtWorks.QRCode.Codec;


namespace Bar
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string lab = textBox1.Text;
            string cipherT = PlainTtoCipherT(lab);
            CreateQRImg(cipherT, lab);
            textBox2.Text = cipherT;

            /// <summary>
            /// 简单加密
            /// </summary>
            /// <param Lab="str">输入的内容</param>
            string PlainTtoCipherT (string Lab) 
            {
                byte[] plaintext = Encoding.Unicode.GetBytes(Lab);//明文转码byte
                byte[] ciphertext_data = new byte[plaintext.Length];//新建密文byte数组
                //Console.WriteLine("Plainttext length is " + plaintext.Length + "\r\n");\\显示转码后数组的长度

                for (int i = 0; i < plaintext.Length; i += 2)//加密
                {
                    int p = i / 2 + 6;
                    if (p > (plaintext.Length / 2))
                    {
                        p = p - plaintext.Length / 2;
                    }
                    ciphertext_data[p - 1] = Convert.ToByte(plaintext[i] + i / 2);
                    if (ciphertext_data[p - 1] > 126)
                    {
                        ciphertext_data[p - 1] = Convert.ToByte(ciphertext_data[p - 1] - 127 + 33);
                    }
                }

                /*调试输出 明文，密文及其长度
                for (int i = 0; i < plaintext.Length; i += 2)
                {
                    Console.WriteLine("plainttext_data is {0} . \r\n", plaintext[i]);
                }
                for (int i = 0; i < ciphertext_data.Length / 2; i++)
                {
                    Console.WriteLine("ciphertext_data is {0} . \r\n", ciphertext_data[i]);
                }
                Console.WriteLine("ciphertext_data length is " + ciphertext_data.Length + "\r\n");
                */

                string cipherText = Encoding.ASCII.GetString(ciphertext_data, 0, ciphertext_data.Length / 2);//转码成文字
                //Console.WriteLine("Ciphertext is " + cipherText + "\r\n");//输出密文内容
                return cipherText;
            }

            /// <summary>
            /// 生成并保存二维码图片的方法
            /// </summary>
            /// <param name="str">输入的内容</param>
            void CreateQRImg(string str,string strName)
            {
                Bitmap bt;
                string enCodeString = str;
                //生成设置编码实例
                QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
                //设置二维码的规模 默认 4
                qrCodeEncoder.QRCodeScale = 4;
                //设置二维码的版本 默认 7
                qrCodeEncoder.QRCodeVersion = 2;
                //设置错误检验级别 默认为中等
                qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
                //生成二维码图片
                bt = qrCodeEncoder.Encode(enCodeString, Encoding.UTF8);
                //二维码图片名称
                string filename =  strName + "-" + DateTime.Now.ToString("D");
                //保存二维码图片在photos路径下
                bt.Save(@"D:\开发\NET Framework and C#\Data\QRCoder_image\" + filename+".jpg");
                //图片控件显示的二维码
                pictureBox1.Image = bt;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string lab = textBox2.Text;
            string plainT = CipherTtoPlainT(lab);
            textBox1.Text = plainT;

            /// <summary>
            /// 简单加密
            /// </summary>
            /// <param cipherT="str">输入的内容</param>
            string CipherTtoPlainT(string cipherT)
            {
                byte[] cipherT_Data = Encoding.Unicode.GetBytes(cipherT);//转码byte
                byte[] plainT_Data = new byte[cipherT_Data.Length];//新建明文byte数组
                //Console.WriteLine("Plainttext length is " + cipherT_Data.Length + "\r\n");//显示数组长度

                for (int i = 0; i < cipherT_Data.Length; i += 2)//解密
                {
                    int p = i / 2 + 14;
                    if (p > cipherT_Data.Length / 2)
                    {
                        p = p - cipherT_Data.Length / 2;
                    }
                    plainT_Data[p - 1] = Convert.ToByte(cipherT_Data[i] - (p - 1));
                    if (plainT_Data[p - 1] < 33)
                    {
                        plainT_Data[p - 1] = Convert.ToByte(plainT_Data[p - 1] + 126 - 32);
                    }
                }

                string plainText = Encoding.ASCII.GetString(plainT_Data, 0, plainT_Data.Length / 2);//转码成文字
                return plainText;
            }

           


        }
    }
}
