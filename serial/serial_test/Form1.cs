using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Windows.Forms.DataVisualization.Charting;

namespace serial_test
{

    public partial class Form1 : Form
    {
        SerialPort serialPort1 = new SerialPort();//定义新的串口类
        private Queue<float> dataQueue = new Queue<float>(50); //新建一个五十位的队列，来堆栈测微机的数据
        private int numDelete = 1; //每次删除增加几个点
        private float receData ; //存放串口接受且经处理的数据，表示实际测量值
        byte[] measureDataByte = new Byte[] { 0x80, 0x03, 0x00, 0x00, 0xFF, 0xFF, 0x5A,0x6B};//预设读取指令数组
        int flagData = 0;//正负标志位
        //预设置零指令数组
        public Form1()
        {
            InitializeComponent();
            String[] portnames = SerialPort.GetPortNames();//获取当前串口名
            foreach (var item in portnames)
            {
                comboBox1.Items.Add(item);//combobox显示当前串口名
            }
            TextBox.CheckForIllegalCrossThreadCalls = false;// 禁用跨线程调用检查控件？？（正确地表述？）
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 开启串口
        /// </summary>
        private void OpenPort_Click(object sender, EventArgs e)
        {
            Opencom();
            void Opencom() {
                try
                {
                    serialPort1.BaudRate = 38400;//波特率
                    serialPort1.DataBits = 8; //数据位
                    serialPort1.PortName = comboBox1.Text;//串口名
                    serialPort1.StopBits = System.IO.Ports.StopBits.Two;//停止位
                    serialPort1.Parity = Parity.None;//无奇偶校验
                    serialPort1.ReadTimeout = 1000;//超时时间
                    serialPort1.ReceivedBytesThreshold = 1; //必须一定要加上这句话，意思是接收缓冲区当中如果有一个字节的话就出发接收函数，如果不加上这句话，那就有时候触发接收有时候都发了好多次了也没有触发接收，有时候延时现象等等，
                    serialPort1.Open();//打开串口
                    if (!serialPort1.IsOpen)
                    {
                        MessageBox.Show("端口打开失败", "error");
                        return;
                    }
                    else
                    {
                        textBoxInfo.AppendText("端口" + comboBox1.Text + "打开成功\r\n");
                    }
                    serialPort1.DataReceived += SerialPort1_DataReceived;// 表示端口接收到了数据，具体作用是？
                }
                catch (Exception ex) {
                    serialPort1.Dispose();//出错滚逼串口
                    textBoxInfo.AppendText(ex.Message);//显示错误信息
                }
            }
        }

        /// <summary>
        /// 串口接收数据并处理
        /// </summary>
        private void SerialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //throw new NotImplementedException();//抛出异常 来检测这一功能你实现了么有（使用串口类时自动会生成的）
            Thread.Sleep(75);//（毫秒）等待一定的时间，确保数据的完整性 int len
            int len = serialPort1.BytesToRead;//串口收到的字节的长度
            string receivedata = string.Empty;//接受内容的string格式
            receData = 0;//测量数值变量清零
            //receData = receData;//serial_test内的全局变量存放测微仪的数据
            if (len != 0) {
                byte[] buffer = new byte[len];//创建缓存区变量
                serialPort1.Read(buffer, 0, len);//将串口输入数据写入缓存区
                for (int i = 0; i < len; i++) {
                    receivedata = Convert.ToString(Convert.ToInt32(buffer[i]), 16);//将数据转码成字符
                    if (buffer[i] < 10) {
                        receivedata = "0" + receivedata;//小于十的前位补零
                    }
                    
                    switch (i)//截选标志位和数据位
                    {
                        case 3:
                            if ( receivedata == "00")//判别正负标志位
                            {
                                flagData = 1;
                            }
                            else {
                                flagData = -1;
                            }
                            break;
                        case 4:
                            if (receivedata != "00") {
                                receData = Convert.ToSingle ( buffer[i]);//录入数据
                                //Console.WriteLine(" {0}", buffer[i]);//检测收取到的byte数据
                            }
                            break;
                        case 5:
                            if (receivedata != "00")
                            {
                                receData = receData * 256 + Convert.ToSingle(buffer[i]);//录入数据
                                //Console.WriteLine(" {0}", buffer[i]);//检测收取到的byte数据
                            }
                            break;
                        case 6:
                            if (receivedata != "00")
                            {
                                receData = receData * 256 + Convert.ToSingle(buffer[i]);//录入数据
                                //Console.WriteLine(" {0} is the byte", buffer[i]);//检测收取到的byte数据
                            }
                            break;
                        default:
                            break;
                    }
                   
                    //textBox2.AppendText(receivedata + " ");//有待商榷
                }
                //textBox2.AppendText("The receData\r\n");
                //receData = receData * flagData / 1000; //接受数据流转化测量数据
                Console.WriteLine("The receData is {0} \r\n", receData);
            }
            //Console.WriteLine("receivedata is " + receivedata + "\r\n");
            
        }

        /// <summary>
        /// 关闭串口
        /// </summary>
        private void ClostPort_Click(object sender, EventArgs e)
        {
            serialPort1.Dispose();
        }

        /// <summary>
        /// 串口输出数据流（十六进制数 以空格分隔）
        /// </summary>
        private void SendContent_Click(object sender, EventArgs e)
        {
            string str1 = textBoxSend.Text;//读取文本框字符
            string[] textSend = str1.Split(new char[] {' '});//以空格分割
            Console.WriteLine("testSend2 is " + textSend + "\r\n");//调试输出字符
            //Console.WriteLine("testSend2.length is " + textSend.Length + "\r\n");
            byte[] dataSend = new byte[textSend.Length];
            for (int i = 0; i < textSend.Length; i++) {
                dataSend[i] =Convert.ToByte( Convert.ToInt32(textSend[i],16));
                //Console.WriteLine("dataSend{0} is " + dataSend[i] + "\r\n",i);
            }
            serialPort1.Write(dataSend,0,dataSend.Length);
            //Console.WriteLine("sending text is " + " " + "\r\n");//调试输出
        }

        /// <summary>
        /// 清空文本框及初始化
        /// </summary>
        private void buttonClearText_Click(object sender, EventArgs e)
        {
            textBoxSend.Text = "";
            textBox2.Text = "";
            textBoxInfo.Text = "";
            InitChart();// 初始化图表
            String[] portnames = SerialPort.GetPortNames();//重新获取当前串口名
            comboBox1.Items.Clear();
            foreach (var item in portnames)
            {
                comboBox1.Items.Add(item);//combobox显示当前串口名
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 开启定时器并实时出图
        /// </summary>
        private void buttonMeasure_Click(object sender, EventArgs e)
        {
            this.timer1.Start();//开始绘图

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 关闭定时器
        /// </summary>
        private void buttonStop_Click(object sender, EventArgs e)
        {
            this.timer1.Stop();//停止计时器绘图
        }

        /// <summary>
        /// 定时器 控制串口发送数据流，并处理返回数据输出在图表中
        /// </summary>
        private void timer1_Tick(object sender, EventArgs e)
        {
            serialPort1.Write(measureDataByte,0,measureDataByte.Length);//串口输出读取数据指令
            //if (Math.Abs(receData) >4)//接收的数据大于0.0004mm才更新数据
            //{
                UpdataQueueValue();//数据队列更新
            //}
            this.chart1.Series[0].Points.Clear();//清空图表点
            for (int i = 0; i < dataQueue.Count; i++) {
                this.chart1.Series[0].Points.AddXY((i + 1), dataQueue.ElementAt(i));//更新图表
                //Console.WriteLine("queue Element is {0}\r\n", dataQueue.ElementAt(i));//监视队列中的数据输出
            }
        }

        /// <summary>
        /// 初始化图表
        /// </summary>
        private void InitChart()
        {
            //定义图表区域
            this.chart1.ChartAreas.Clear();
            ChartArea chartArea1 = new ChartArea("C1");
            this.chart1.ChartAreas.Add(chartArea1);
            //定义储存和显示点的容器
            this.chart1.Series.Clear();
            Series series1 = new Series("S1");
            series1.ChartArea = "C1";
            this.chart1.Series.Add(series1);
            //设置图标显示样式
            this.chart1.ChartAreas[0].AxisX.Interval = 1;
            this.chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = System.Drawing.Color.Silver;
            this.chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = System.Drawing.Color.Silver;
            //设置标题
            this.chart1.Titles.Clear();
            this.chart1.Titles.Add("S01");
            this.chart1.Titles[0].Text = "Serial Port";
            this.chart1.Titles[0].ForeColor = Color.RoyalBlue;
            this.chart1.Titles[0].Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            //图表样式
            this.chart1.Series[0].Color = Color.MidnightBlue;
            this.chart1.Titles[0].Text = string.Format("测微仪 显示");
            this.chart1.Series[0].ChartType = SeriesChartType.Line;
            //claer
            this.chart1.Series[0].Points.Clear();
        }

        /// <summary>
        /// 更新队列中的值
        /// </summary>
        private void UpdataQueueValue() {
            if (dataQueue.Count > 50) {
                for (int i = 0; i < numDelete; i++) { //先出列
                    dataQueue.Dequeue();
                }
            }
            for (int i = 0; i < numDelete; i++) {//输入值
                //if (receData > 100) {
                    receData = receData * flagData / 10000;//receData = receData / 1000; //单位转化
                //}
                if (dataQueue.Count>49) { 
                    if (receData != 0 && ( Math.Abs(receData)<0.0005 ) &&(Math.Abs( Convert.ToSingle( dataQueue.ElementAt(49))) > 0.0004) || (receData == 0 && (Math.Abs( Convert.ToSingle( dataQueue.ElementAt(49))))>0.001) || (receData>1000)) {
                        receData = Convert.ToSingle( dataQueue.ElementAt(49));
                    }
                }
                
                dataQueue.Enqueue(receData);
                Console.WriteLine("Queue receData is {0} \r\n", receData);//队列数据输出
            }
        }
    }
}
