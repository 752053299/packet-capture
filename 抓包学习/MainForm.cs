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
using System.IO;

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
        private ToolStripComboBox portName;

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
        }

        /// <summary>
        /// 开始按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartButton_Click(object sender, EventArgs e)
        {
            ToolStripButton bt = sender as ToolStripButton;
            

            if (String.IsNullOrEmpty(portName.Text))
            {
                MessageBox.Show("请选择串口");
                return;
            }

            if (!serialPort1.IsOpen) //打开串口
            {
                try
                {
                    
                    serialPort1.Open();

                    bt.Text = "停止";
                    //改变图片(未实现)
                    
                    Background.DaemonFrameType = Background.FrameType.Serial;
                    Background.SerialPortDebugEnable = true;
                    portNameBox.Enabled = false;
                    
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
            else //关闭串口
            {
                try
                {
                    Background.SerialPortDebugEnable = false;
                    bt.Text = "开始";
                    portNameBox.Enabled = true;                   
                    serialPort1.Close();
                    serialPort1.Dispose();
                    //将开始按钮绑定的事件注销
                    AnalysisProcess.ReceiveDataFrameEvent -= AnalysisProcess_ReceiveDataFrameEvent;
                }
                catch (IOException ex)
                {
                    Console.WriteLine(ex.Message);
                }


            }
        }

        //接收后的帧处理后显示
        void AnalysisProcess_ReceiveDataFrameEvent(AnalysisProcess.SerialFrameAnalysisEventArgs e)
        {

            SerialPortFrame serialPortFrame = e.SerialPortFrame;

            //判断真类型
            int FrameTypeSel = (int)serialPortFrame.FrameType;
            switch(FrameTypeSel)
            {
                case 0:
                    Log(LogMsgType.DataFrame, "数据帧");
                    DataFrameCount++;
                    break;
                case 1:
                    Log(LogMsgType.Commond, "命令帧");
                    OrderFrameCount++;
                    break;
                case 2:
                    Log(LogMsgType.Response, "应答帧");
                    ResponseFrameCount++;
                    break;
                case 3:
                    Log(LogMsgType.Warning, "告警帧");
                    WarningFrameCount++;
                    break;
            }

            //如果筛选合格，显示
            if (filtrate(serialPortFrame))
            {
                string receiveTime = e.ReceiveTime.ToString("HH:mm:ss:fff");
                string oneFrame = Character.BytesToString(serialPortFrame.OneFrame, 0, serialPortFrame.OneFrame.Length - 1, true);
                
                int FrameCommun = (int)serialPortFrame.CommunicationWay;
                string data = Character.BytesToString(serialPortFrame.DataByte, 0, serialPortFrame.DataByte.Length - 1, true);

                switch (FrameTypeSel)
                {
                    case 0:
                        Log(LogMsgType.DataFrame, "数据帧");
                        break;
                    case 1:
                        Log(LogMsgType.Commond, "命令帧");
                        break;
                    case 2:
                        Log(LogMsgType.Response, "应答帧");
                        break;
                    case 3:
                        Log(LogMsgType.Warning, "告警帧");
                        break;
                }

                switch (FrameCommun)
                {
                    case 0:
                        Log(LogMsgType.ReceiveTime, "[透  传]");
                        break;
                    case 1:
                        Log(LogMsgType.ReceiveTime, "[非透传]");
                        break;
                    case 2:
                        Log(LogMsgType.ReceiveTime, "  ");
                        break;
                    case 3:
                        Log(LogMsgType.ReceiveTime, "  ");
                        break;
                }
                Log(LogMsgType.ReceiveTime, "\t" + receiveTime);

                Log(LogMsgType.ControlCodeNote, "\t" + oneFrame + "\n");

                Log(LogMsgType.ControlCodeNote, "节点地址：  " + serialPortFrame.NodeMac + "\t" + "集中器地址：  " + serialPortFrame.ConcentratorMac + "\t" + data + "\n");
            }

        }

        /// <summary>
        /// 向窗体中写入数据
        /// </summary>
        /// <param name="msgtype">数据种类</param>
        /// <param name="msg">数据内容</param>
        public void Log(LogMsgType msgtype, string msg) //
        {
          //  richTextBox1.Invoke(new EventHandler(delegate
         //   {
           //     richTextBox1.SelectedText = string.Empty;
                richTextBox1.SelectionFont = new Font(richTextBox1.SelectionFont, FontStyle.Bold);
                richTextBox1.SelectionColor = LogMsgTypeColor[(int)msgtype];//根据logmegtype的值来显示颜色。
                richTextBox1.AppendText(msg);
                richTextBox1.ScrollToCaret();//将控件内容滚动到当前内容。
         //   }));
        }


        #region 主窗体控件初始化

        private void MainForm_Load(object sender, EventArgs e)
        {
            StringFilePath = Application.StartupPath + "\\FrameData";

            addPortName();
            Background.SerialPortDebugEnable = false;
            portName = portNameBox as ToolStripComboBox;

            if (!Directory.Exists(StringFilePath))  //不存在文件夹
            {
                Directory.CreateDirectory(StringFilePath);
            }

            serialPortInit(portName.Text, 115200);
        }

        private void addPortName()
        {
            String[] ports = SerialPort.GetPortNames();
            if (ports.Length > 0)
            {
                this.portNameBox.Items.Clear();
                this.portNameBox.Items.AddRange(ports);
                this.portNameBox.SelectedIndex = 0;
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
            if(serialPort1 == null)
            {
                serialPort1 = new SerialPort
                {
                    PortName = portName,
                    BaudRate = baudRate,
                    DataBits = 8,
                    StopBits = StopBits.One,
                    Parity = Parity.None,
                    ReadTimeout = 1000,
                    WriteTimeout = 1000
                };
            }
            else
            {
                serialPort1.PortName = portName;
            }

        }



        //串口下拉刷新端口
        private void portNameBox_DropDown(object sender, EventArgs e)
        {      
            addPortName();
        }


        //串口下拉后串口名字改变
        private void portNameBox_DropDownClosed(object sender, EventArgs e)
        {
            serialPortInit(portName.Text, 115200);
        }
        #endregion

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {

                richTextBox1.SaveFile(StringFilePath + "\\" + DtString + ".txt", RichTextBoxStreamType.PlainText);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 筛选显示
        /// </summary>
        /// <param name="internalFrame">将要筛选的帧目前只能筛选68</param>
        /// <returns></returns>
        private bool filtrate(InternalFrame internalFrame)
        {
            bool isShow = false;

            SerialPortFrame serialPortFrame = (SerialPortFrame)internalFrame;
            string nodeMac = serialPortFrame.NodeMac;
            string concentratorMac = serialPortFrame.ConcentratorMac;

            
            if(nodeTextBox.Text.Equals("") && concentratorTextBox.Text.Equals(""))
            {
                return true;
            }
            else //有一方是空
            {
                //筛选集中器
                if (nodeTextBox.Text.Equals("") && !(concentratorTextBox.Text.Equals("")))
                {
                    if (concentratorMac.Equals(concentratorTextBox.Text))
                        return true;
                }    //筛选节点
                else if (!nodeTextBox.Text.Equals("") && (concentratorTextBox.Text.Equals("")))
                {
                    if (nodeMac.Equals(nodeTextBox.Text))
                        return true;
                }
                else //筛选节点+集中器
                {
                    if (nodeMac.Equals(nodeTextBox.Text) && concentratorMac.Equals(concentratorTextBox.Text))
                        return true;
                }
            }

            return isShow;
        }


        /// <summary>
        /// 清空显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearButton_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
        }


    }
}
