using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 抓包学习
{
    class AnalysisProcess
    {
        //声明分析帧的委托
        public delegate void FrameAnalysisResult(SerialFrameAnalysisEventArgs e);

        //声明分析帧的事件
        public static event FrameAnalysisResult ReceiveDataFrameEvent;

        /// <summary>
        /// 提取完成后的帧事件
        /// </summary>
        public class SerialFrameAnalysisEventArgs : EventArgs  //分析帧事件类
        {
            public readonly SerialPortFrame SerialPortFrame;//接受到的帧
            public readonly DateTime ReceiveTime;   //接受时间

            public SerialFrameAnalysisEventArgs(SerialPortFrame serialPortFrame, DateTime receiveTime)//构造函数
            {
                this.SerialPortFrame = serialPortFrame;
                this.ReceiveTime = receiveTime;
            }
        }


        #region 识别收到的帧

        //用来存储断帧
        public static byte[] interframe = new byte[0];


        /// <summary>
        ///从字节流中拆分出正确的帧，并进行处理
        /// </summary>
        /// <param name="receiveByte">接收到的字节流.</param>
        public static void CheckFrame(byte[] receiveByte)  //字节流
        {
            try
            {
               // 打印数据（测试用）
                string zframe = Character.BytesToString(receiveByte, 0, receiveByte.Length - 1, true);
                Console.WriteLine("串口收到数据： " + zframe);

                //处理断帧
                if (interframe.Length > 0)
                {
                    byte[] result = new byte[interframe.Length + receiveByte.Length];
                    interframe.CopyTo(result, 0);
                    receiveByte.CopyTo(result, interframe.Length);
                    receiveByte = new byte[result.Length];
                    receiveByte = result;
                }
                interframe = new byte[0];
                if (Background.DaemonFrameType == Background.FrameType.Serial)
                {
                    for (int i = 0; i < receiveByte.Length; i++)
                    {
                        if (receiveByte[i] == (byte)NetCommunicationFrame.FrameHeadAndTail.Head)//找到69帧头
                        {
                            /* 保证receiveByte至少包含一帧 */
                            //第1个条件保证第二个条件中的receiveByte数组不越界;第2个条件保证后文中的receiveByte数组不越界
                            if ((i + 24 < receiveByte.Length) && (i + 24 + receiveByte[23 + i] + 2 <= receiveByte.Length))
                            {
                                //数据域长度
                                int datalength = Convert.ToInt32(receiveByte[i + 22] << 8) + Convert.ToInt32(receiveByte[i + 23]);

                                if (receiveByte[i + 23 + datalength + 2] == (byte)NetCommunicationFrame.FrameHeadAndTail.Tail)//找69帧尾
                                {
                                    byte[] oneFrame = new byte[24 + datalength + 2];
                                    for (int j = 0; j < oneFrame.Length; j++)
                                    {
                                        oneFrame[j] = receiveByte[i + j];     //抽取完整的一条69帧
                                    }

                                    if (CheckSumCorrect(oneFrame))  //校验成功
                                    {
                                        //测试用
                                        string leng = Character.BytesToString(oneFrame, 0, oneFrame.Length - 1, true);
                                        Console.WriteLine("输出完整帧内容：" + leng);
                                        //处理该帧
                                        FrameAnalysis(oneFrame);

                                        Program.mainform.RxFrameCount++;//更新窗口接收到的帧计数


                                        if (i + 24 + datalength + 2 == receiveByte.Length)//receiveByte已经处理完
                                        {
                                            return;
                                        }
                                        else//receiveByte还没处理完，可能是连帧
                                        {
                                            i = i + 23 + datalength + 2;//处理下一帧数据
                                            continue;
                                        }
                                    }
                                    else //校验失败
                                    {
                                        Program.mainform.CheckFailedFrameCount++;//更新主窗口失败帧统计
                                        continue;//继续找下一个帧头
                                    }
                                }
                            }
                            else //断帧
                            {
                                interframe = new byte[receiveByte.Length - i];
                                for (int nu = 0; i < receiveByte.Length; i++)
                                {
                                    interframe[nu] = receiveByte[i];
                                    nu++;
                                }
                                Program.mainform.UnknownFrameCount++;//未知帧计数加一
                            }
                        }
                    }

                }
                else //处理网络通信帧
                {

                }
           

            }
            catch(Exception ex)
            {
                Console.WriteLine("帧接收异常，异常信息" + ex.ToString());
            }
        }

        /// <summary>
        /// 解析收到的帧
        /// </summary>
        /// <param name="receiveByte">接受到的帧字节</param>
        public static void FrameAnalysis(byte[] receiveByte)
        {
            #region 第一次提取负载  提取出69帧的负载
            byte[] receive;

            NetCommunicationFrame frame_first = new NetCommunicationFrame();
            frame_first.Receive(receiveByte);

            if (frame_first.CheckSumCorrect)
            {
                receive = frame_first.DataByte; //校验正确，提取出69帧的负载

                // 打印数据（测试用）
                string first_frame_lod = Character.BytesToString(receive, 0, receive.Length - 1, true);
                Console.WriteLine("第一次提取的负载： " + first_frame_lod);

            }
            else
            {
                Program.mainform.CheckFailedFrameCount++;//校验失败，增加窗口记录失败值
                return;
            }
            if (receive == null)//收到无负载的帧，暂不处理
            {
                Program.mainform.UnknownFrameCount++;//未知帧计数加一
                return;
            }

            #endregion

            #region 解析负载

            #region 解析串口68通信帧
            if (receive[0] == (byte)SerialPortFrame.FrameHeadAndTail.Head) //找帧头
            {
                SerialPortFrame fram68 = new SerialPortFrame();
                fram68.Receive(receive);

                if (fram68.CheckSumCorrect) //校验和正确
                {
                    // 打印数据（测试用）
                    string data = Character.BytesToString(fram68.DataByte, 0, fram68.DataByte.Length - 1, true);
                    Console.WriteLine("数据域"+ data);

                    DateTime receiveTime = DateTime.Now;
                    /* 分析一帧完成的事件 */
                    SerialFrameAnalysisEventArgs e = new SerialFrameAnalysisEventArgs(fram68, receiveTime);
                    
                    //发布事件
                    if (ReceiveDataFrameEvent != null)// 如果有对象注册
                    {
                        ReceiveDataFrameEvent(e); // 调用所有注册对象的方法
                    }

                }
                else //校验错误
                {
                    Program.mainform.CheckFailedFrameCount++;// 校验失败，增加窗口记录失败值
                    Program.mainform.UnknownFrameCount++;//未知帧计数加一
                    return;
                }
            }

            #endregion

            #endregion
        }

        #endregion

        ///// <summary>
        ///// 给控制码添加说明.
        ///// </summary>
        ///// <param name="frame">串口通信帧.</param>
        ///// <returns>添加的说明</returns>
        //static string addControlCodeNote(SerialPortFrame frame)      //Note
        //{
        //    string controlCodeNote = "";

        //    switch (frame.FrameType)  //帧类型判断
        //    {
        //        case InternalFrame.Type.DataFrame:
        //            MainForm.mainForm.FrameTypeSel = 0;
        //            break;
        //        case InternalFrame.Type.OrderFrame:
        //            MainForm.mainForm.FrameTypeSel = 1;
        //            break;
        //        case InternalFrame.Type.ReplyFrame:
        //            MainForm.mainForm.FrameTypeSel = 2;
        //            break;
        //        case InternalFrame.Type.AlarmFrame:
        //            MainForm.mainForm.FrameTypeSel = 3;
        //            break;
        //        default:
        //            break;
        //    }

        //    controlCodeNote += frame.NeedAnswer ? "[需要应答]" : "[不要应答]";

        //    switch (frame.CommunicationWay)  //通信方式
        //    {
        //        case SerialPortFrame.Communication.Transparent:
        //            MainForm.mainForm.FrameCommun = 0;
        //            break;
        //        case SerialPortFrame.Communication.Nontransparent:
        //            MainForm.mainForm.FrameCommun = 1; break;
        //        case SerialPortFrame.Communication.InnerNet:
        //            MainForm.mainForm.FrameCommun = 2; break;
        //        case SerialPortFrame.Communication.phone:
        //            MainForm.mainForm.FrameCommun = 3; break;
        //        default:
        //            break;
        //    }

        //    /************************/
        //    switch (frame.FrameDirction)
        //    {
        //        case InternalFrame.FrameDirType.GateWayToNode: MainForm.mainForm.Node2GateWay = false;
        //            break;
        //        case InternalFrame.FrameDirType.NodeToGateWay: MainForm.mainForm.Node2GateWay = true;
        //            break;
        //    }
        //    /************************/
        //    return controlCodeNote;
        //}

        ///// <summary>
        ///// 给帧内容添加说明.
        ///// </summary>
        ///// <param name="frame">串口通信帧.</param>
        ///// <returns>添加的说明</returns>
        //static string addContentNote(SerialPortFrame frame)
        //{
        //    string contentNote = String.Format("节点:{0} 集中器:{1} 帧计数器:{2} ", frame.NodeMac, frame.ConcentratorMac, frame.FrameCount);

        //    if (frame.FrameType == SerialPortFrame.Type.OrderFrame)//接收到命令帧
        //    {
        //        if (frame.DataByte.Length < 4)
        //        {
        //            contentNote += "命令标识长度不足！！"; //命令标识长度为4个字节

        //            return contentNote;
        //        }

        //        string commandIDstring = Character.BytesToString(frame.DataByte, 0, 3, true);

        //        contentNote += String.Format("命令标识:{0}", commandIDstring);
        //    }
        //    else if (frame.FrameType == SerialPortFrame.Type.AlarmFrame)//接收到告警帧
        //    {
        //        string alarmInfoString = Character.BytesToString(frame.DataByte, 0, frame.DataByte.Length - 1, true);

        //        contentNote += String.Format("告警信息:{0}", alarmInfoString);
        //    }
        //    else if (frame.FrameType == SerialPortFrame.Type.ReplyFrame)//接收到应答帧
        //    {

        //    }
        //    else if (frame.FrameType == SerialPortFrame.Type.DataFrame)//接收到数据帧
        //    {
        //        contentNote += String.Format("数据域长度{0}字节", frame.DataLength.ToString());
        //    }

        //    /*****************筛选集中器********************/
        //    //string str = MainForm.mainForm.toolStripTextBox2.Text.Replace(" ", "");
        //    //string strLast = MainForm.mainForm.toolStripTextBox2.Text.Replace(" ", "").Substring(2, 2);
        //    if (MainForm.mainForm.toolStripTextBox2.Text == "")
        //    {
        //        MainForm.mainForm.ConcentrateBool = true;
        //    }
        //    else if ((MainForm.mainForm.toolStripTextBox2.Text.Replace(" ", "") == frame.ConcentratorMac + frame.ConcentratorMac1) || (frame.ConcentratorMac == "00" && frame.ConcentratorMac1 == "00"))
        //    {
        //        MainForm.mainForm.ConcentrateBool = true;
        //    }
        //    else if (MainForm.mainForm.toolStripTextBox2.Text.Replace(" ", "").Substring(2, 2) == frame.ConcentratorMac1 || MainForm.mainForm.toolStripTextBox2.Text.Replace(" ", "").Substring(2, 2) == frame.ConcentratorMac)
        //    {
        //        MainForm.mainForm.ConcentrateBool = true;
        //    }
        //    else  //输入不对则不显示
        //    {
        //        MainForm.mainForm.ConcentrateBool = false;
        //    }
        //    /*****************筛选节点*******************/
        //    if (MainForm.mainForm.toolStripTextBox1.Text == "")
        //    {
        //        MainForm.mainForm.NodeBool = true;
        //    }
        //    else
        //    {
        //        if (frame.NodeMac == MainForm.mainForm.toolStripTextBox1.Text)
        //        {
        //            MainForm.mainForm.NodeBool = true;
        //        }
        //        else
        //        {
        //            MainForm.mainForm.NodeBool = false;
        //        }
        //    }
        //    /***************************************/
        //    return contentNote;
        //}

        /// <summary>
        /// 检查一条完整帧的校验和是否正确.
        /// </summary>
        /// <param name="oneFrame">一条完整的帧.</param>
        /// <returns>校验和正确时返回真，否则返回假</returns>
        public static bool CheckSumCorrect(byte[] oneFrame)
        {
            byte checkSum = 0;
            for (int i = 0; i < oneFrame.Length - 2; i++)
            {
                checkSum += oneFrame[i];
            }
            if (checkSum == oneFrame[oneFrame.Length - 2])
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
