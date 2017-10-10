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
using System.Threading;

namespace 抓包学习
{
    public partial class MainForm : Form
    {

        public enum LogMsgType { DataFrame, Commond, Response, Warning, Error, Statistics, controlFalutColor, ReceiveTime, ControlCodeNote };

        //颜色
        private Color[] LogMsgTypeColor = { Color.Brown, Color.DarkOliveGreen, Color.Black, Color.Red, Color.Red, Color.Green, Color.OrangeRed, Color.Blue, Color.BlueViolet };
        //
        public string StringFilePath = "";
        public string DtString = "";

        private int _rxFrameCount; //记录接收到的帧个数
        private int _checkFailedFrameCount;//校验失败的帧
        private int _dataFrameCount;//记录数据帧
        private int _orderFrameCount;//记录命令帧
        private int _responseFrameCount;//记录应答帧
        private int _warningFrameCount;//记录告警帧
        private int _unknownFrameCount;//记录未知帧

        public int RxFrameCount 
        {
            get 
            { 
                return _rxFrameCount;
            }
            set
            {
                _rxFrameCount = value;
                LabelReceiveNum.Text = "收到的帧：" + _rxFrameCount.ToString();
            }
        }
        public int CheckFailedFrameCount
        {
            get
            {
                return _checkFailedFrameCount;
            }
            set
            {
                _checkFailedFrameCount = value;
            }
        }

        public int DataFrameCount
        {
            get
            {
                return _dataFrameCount;
            }
            set
            {
                _dataFrameCount = value;
                LabelDataNum.Text = "数据帧：" + _dataFrameCount.ToString();
            }
        }

        public int OrderFrameCount
        {
            get
            {
                return _orderFrameCount;
            }
            set
            {
                _orderFrameCount = value;
                LabelOrderNum.Text = "命令帧：" + _orderFrameCount.ToString();
            }
        }

        public int ResponseFrameCount
        {
            get
            {
                return _responseFrameCount;
            }
            set
            {
                _responseFrameCount = value;
                LabelResponseNum.Text = "应答帧：" + _responseFrameCount.ToString();
            }
        }

        public int WarningFrameCount
        {
            get
            {
                return _warningFrameCount;
            }
            set
            {
                _warningFrameCount = value;
                LabelWarningNum.Text = "告警帧：" + _warningFrameCount.ToString();
            }
        }

        public int UnknownFrameCount
        {
            get
            {
                return _unknownFrameCount;
            }
            set
            {
                _unknownFrameCount = value;
                LabelUnknowNum.Text = "未知帧：" + _unknownFrameCount.ToString();
            }
        }


        public MainForm()
        {
            InitializeComponent();
            StringFilePath = Application.StartupPath + "\\FrameData";

            addPortName();
            Background.SerialPortDebugEnable = false;


        }

        /// <summary>
        /// 开始按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartButton_Click(object sender, EventArgs e)
        {
            ToolStripButton bt = sender as ToolStripButton;
            ToolStripComboBox portName = portNameBox as ToolStripComboBox;

            if (String.IsNullOrEmpty(portName.Text))
            {
                MessageBox.Show("请选择串口");
                return;
            }

            if (Background.SerialPortDebugEnable == false)
            {
                try
                {
                    serialPortInit(portName.Text, 115200);
                    serialPort1.Open();

                    bt.Text = "停止";
                    //改变图片(未实现)
                    
                    Background.DaemonFrameType = Background.FrameType.Serial;
                    Background.SerialPortDebugEnable = true;
                    portNameBox.Enabled = false;
                    System.IO.Directory.CreateDirectory(StringFilePath);
                    DtString = DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString() + "-" + DateTime.Now.Second.ToString();

                    ThreadPool.QueueUserWorkItem(new WaitCallback(Background.BackgroundReceive), null);

                    //后台接收到帧之后发布事件的接收
                    AnalysisProcess.ReceiveDataFrameEvent += AnalysisProcess_ReceiveDataFrameEvent;
                }
                catch(Exception ex)
                {
                    MessageBox.Show("串口连接失败" + ex.Message);
                }

            }
        }

        //接收后的帧处理后显示
        void AnalysisProcess_ReceiveDataFrameEvent(AnalysisProcess.SerialFrameAnalysisEventArgs e)
        {
            string oneFrame = Character.BytesToString(e.SerialPortFrame.OneFrame, 0, e.SerialPortFrame.OneFrame.Length - 1, true);
            Log(LogMsgType.Commond, oneFrame);
        }

        /// <summary>
        /// 向窗体中写入数据
        /// </summary>
        /// <param name="msgtype">数据种类</param>
        /// <param name="msg">数据内容</param>
        public void Log(LogMsgType msgtype, string msg) //
        {
            richTextBox1.Invoke(new EventHandler(delegate
            {
                richTextBox1.SelectedText = string.Empty;
                richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont, FontStyle.Bold);
                richTextBox1.SelectionColor = LogMsgTypeColor[(int)msgtype];//根据logmegtype的值来显示颜色。
                richTextBox1.AppendText(msg+"\n");
                richTextBox1.ScrollToCaret();//将控件内容滚动到当前内容。
            }));
        }



        #region 主窗体控件初始化


        private void addPortName()
        {
            String[] ports = SerialPort.GetPortNames();
            if (ports.Length > 0)
            {
                this.portNameBox.Items.Clear();
                this.portNameBox.Items.AddRange(ports);
                this.portNameBox.SelectedIndex = this.portNameBox.Items.Count - 1;
                this.portStateLabel.Text = "发现串口";
            }
            else
            {
                this.portStateLabel.Text = "未发现串口";
                this.StartButton.Enabled = false;
            }
        }

        /// <summary>
        /// 串口初始化.
        /// </summary>
        /// <param name="portName">端口名.</param>
        /// <param name="baudRate">波特率.</param>
        private void serialPortInit(string portName, int baudRate)
        {
            serialPort1 = new SerialPort();
            serialPort1.PortName = portName;
            serialPort1.BaudRate = baudRate;
            serialPort1.DataBits = 8;
            serialPort1.StopBits = StopBits.One;
            serialPort1.Parity = Parity.None;
            serialPort1.ReadTimeout = 1000;
            serialPort1.WriteTimeout = 1000;
        }



        #endregion

        private void portNameBox_DropDown(object sender, EventArgs e)
        {
            addPortName();
        }

       
    }
}
