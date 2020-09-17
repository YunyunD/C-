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
            string Lab = textBox1.Text;
            CreateQRImg(Lab);
            
            /// <summary>
            /// 生成并保存二维码图片的方法
            /// </summary>
            /// <param name="str">输入的内容</param>
             void CreateQRImg(string str)
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
                string filename = str + "-" + DateTime.Now.ToString("D");
                //保存二维码图片在photos路径下
                bt.Save(@"D:\开发\NET Framework and C#\Data\QRCoder_image\" + filename+".jpg");
                //图片控件要显示的二维码图片路径
                pictureBox1.Image = bt;
                
            }


        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
