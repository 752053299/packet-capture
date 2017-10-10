using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace 抓包学习
{
    class Background 
    {

        #region 公共属性
        public static FrameType DaemonFrameType = FrameType.Unknown;//标识当前是什么模式下的通信（串口，未知）

        public enum FrameType
        {
            Serial,
            Unknown,
        }
        #endregion

        #region 私有属性
        private static bool _serialPortDebugEnable;
        //private static bool _socketDebugEnable;
        //private static bool _sendFileActivity;
        //private static bool _rxHoldUp = false;
        //private static int _txSerialFrameIntervalTime = 50;
        //private static int _txSocketFrameIntervalTime = 200;
        //private static int _txTempExtraIntervalTime = 0;
        #endregion

        #region 私有属性的公共存取方法
        /// <summary>
        /// Gets or sets a value indicating whether [串口调试状态标志].
        /// </summary>
        /// <value>
        /// <c>true</c> if [串口调试状态标志]; otherwise, <c>false</c>.
        /// </value>
        public static bool SerialPortDebugEnable
        {
            get { return _serialPortDebugEnable; }
            set { _serialPortDebugEnable = value; }
        }

        #endregion

        //接收到字节的委托
        delegate void DataAnalysisInvoke(byte[] receiveBytes);

        //后台线程处理
        public static void BackgroundReceive(object sender)
        {
            int len;
            while (DaemonFrameType != FrameType.Unknown)
            {
                try
                {
                     len = 0;
                     len = Program.mainform.SerialPort1.BytesToRead;
                     if (len > 0)
                     {
                         Thread.Sleep(10);
                         len = Program.mainform.SerialPort1.BytesToRead;
                         byte[] receiveByte = new byte[len];
                         Program.mainform.SerialPort1.Read(receiveByte, 0, len);

                         Program.mainform.Invoke(new DataAnalysisInvoke(AnalysisProcess.CheckFrame),receiveByte);
                     }

                     Thread.Sleep(5);


                }
                catch (Exception ex)//此异常可能由用户主动断开串口或Socket连接引发
                {
                    Console.WriteLine(ex.Message);
                    return;
                }

            }
        }

    }
}
