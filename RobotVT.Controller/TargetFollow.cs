using FFmpeg.AutoGen;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;

namespace RobotVT.Controller
{
    public unsafe class TargetFollow
    {
        /// <summary>
        /// 目标跟踪组播数据接收
        /// </summary>
        /// <param name="FaceSnapAlarm"></param>
        internal delegate void MulticastEventHandler(List<byte> recvBuflist);
        /// <summary>
        /// 接收数据事件
        /// </summary>
        internal event MulticastEventHandler Event_Multicast;

        /// <summary>
        /// 目标跟踪组播数据接收
        /// </summary>
        /// <param name="FaceSnapAlarm"></param>
        internal delegate void TargetCoordinatesHandler(TargetFollowRecvInfo recvInfo);
        /// <summary>
        /// 接收数据事件
        /// </summary>
        internal event TargetCoordinatesHandler Event_TargetCoordinates;


        Socket udpReceive;
        EndPoint ep;
        UdpClient client;
        TargetFollowInfo TargetFollowInfo;
        TargetFollowRecvInfo recvInfo;
                
        /// <summary>
        /// 相关资源初始化
        /// </summary>
        public unsafe void Init()
        {
            string localip = getIPAddress();
            IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(localip), StaticInfo.MulticastGroupPort);
            udpReceive = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            ep = (EndPoint)ipe;
            udpReceive.Bind(ipe);
            udpReceive.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(IPAddress.Parse(StaticInfo.MulticastGroupIP)));

            client = new UdpClient(StaticInfo.TargetFollowIP, StaticInfo.TargetFollowPort);
            //client = new UdpClient("192.168.1.65", StaticInfo.TargetFollowPort);
            TargetFollowInfo = new TargetFollowInfo();

            GetBaseNum();
        }


        Int64 GetBaseNum()
        {
            //01e2ffe0
            Int64 iTmp = Convert.ToInt64("01e2", 16);

            if (iTmp >= 0x8000000)
            {
                iTmp = ~(iTmp - 1);

            }

            return iTmp;
        }


        /// <summary>
        /// 接收组播数据
        /// </summary>
        public void Start()
        {
            System.Threading.Thread t = new Thread(new ThreadStart(RecvThread));
            t.IsBackground = true;
            t.Start();
        }

        /// <summary>
        /// 接收组播数据
        /// </summary>
        public void Stop()
        {
            udpReceive.Disconnect(true);
            udpReceive.Dispose();
            client.Dispose();
        }


        private unsafe void RecvThread()
        {
            try
            {
                int retlen = 0;
                List<byte> recvBuflist = new List<byte>();

                while (true)
                {
                    byte[] recvBuf = new byte[1400];
                    udpReceive.ReceiveFrom(recvBuf, ref ep);
                    if (recvBuf[0] == 0xEB && recvBuf[1] == 0x90 && recvBuf[2] == 0xFF)
                    {
                        PicRecBuf(recvBuf, ref retlen, ref recvBuflist);
                    }
                    if (recvBuf[0] == 0xaa && recvBuf[1] == 0x0e)
                    {
                        TargetRecBuf(recvBuf);
                    }
                }
            }
            catch (Exception _Ex)
            {
                RobotVT.Controller.Methods.SaveExceptionLog(SK_FModel.SystemEnum.LogType.Exception, "组播数据解析异常：" + _Ex.Message + "\r\n" + _Ex.StackTrace);
                throw new Exception("组播数据解析失败，错误信息：" + _Ex.Message);
            }
        }
        private void PicRecBuf(byte[] recvBuf, ref int retlen, ref List<byte> recvBuflist)
        {
            byte[] packethead = new byte[8];
            Array.Copy(recvBuf, packethead, 8);

            int iChannelNumber = packethead[3];
            int iIsEnd = packethead[4];
            byte[] packetContect;
            if (iIsEnd == 0)
            {
                packetContect = new byte[1400 - 8];
                Array.Copy(recvBuf, 8, packetContect, 0, packetContect.Length);
                retlen += 1392;
                recvBuflist.AddRange(packetContect);
            }
            else
            {
                int iPacketLength = BitConverter.ToInt16(new byte[2] { packethead[7], packethead[6] }, 0);
                packetContect = new byte[iPacketLength];
                Array.Copy(recvBuf, 8, packetContect, 0, packetContect.Length);
                retlen += iPacketLength;
                recvBuflist.AddRange(packetContect);

                if (retlen == recvBuflist.Count)
                {
                    ////解析数据，并还原参数
                    //H264(recvBuflist.ToArray());
                    Event_Multicast?.Invoke(recvBuflist);
                    recvBuflist.Clear();
                    retlen = 0;
                    Thread.Sleep(1);
                }

            }
        }
        public void ConvertVideo()
        {
            Process p = new Process();//建立外部调用线程
            p.StartInfo.FileName = @"c:\ffmpeg.exe";//要调用外部程序的绝对路径
            p.StartInfo.Arguments = "-i XXXXXXXXXXXXXX";//参数(这里就是FFMPEG的参数了)
            p.StartInfo.UseShellExecute = false;//不使用操作系统外壳程序启动线程(一定为FALSE,详细的请看MSDN)
            p.StartInfo.RedirectStandardError =true;
            //把外部程序错误输出写到StandardError流中(这个一定要注意,FFMPEG的所有输出信息,都为错误输出流,用
            //StandardOutput是捕获不到任何消息的...这是我耗费了2个多月得出来的经验...mencoder就是用standardOutput来捕获的)
            p.StartInfo.CreateNoWindow = true;//不创建进程窗口
            p.ErrorDataReceived += new DataReceivedEventHandler(Output);//外部程序(这里是FFMPEG)输出流时候产生的事件,这里是把流的处理过程转移到下面的方法中,详细请查阅MSDN
            p.Start();//启动线程
            p.BeginErrorReadLine();//开始异步读取
            //p.WaitForExit();//阻塞等待进程结束
            //p.Close();//关闭进程
            //p.Dispose();//释放资源
        }

        private void Output(object sendProcess, DataReceivedEventArgs output)
        {
            if (!String.IsNullOrEmpty(output.Data))
            {

                //处理方法... 

            }
        }



        private void TargetRecBuf(byte[] recvBuf)
        {
            recvInfo = new TargetFollowRecvInfo(recvBuf);

        }
        
        public static IntPtr BytesToIntptr(byte[] bytes)
        {
            int size = bytes.Length;
            IntPtr buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(bytes, 0, buffer, size);
                return buffer;
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }

        /// <summary>
        /// 发送坐标数据
        /// </summary>
        public void SendingCoordinates(int imageWidth, int imageHeight, Point MouseLocation)
        {
            short PitchCoordinate;
            short AzimuthCoordinate;
            try
            {
                if (imageWidth == 0 && imageHeight == 0)
                {
                    PitchCoordinate = 0;
                    AzimuthCoordinate = 0;
                }
                else
                {
                    int centerX = imageWidth / 2;
                    int centerY = imageHeight / 2;

                    AzimuthCoordinate = (short)((float)(MouseLocation.X - centerX) / imageWidth * 1024);
                    PitchCoordinate = (short)((float)(centerY - MouseLocation.Y) / imageHeight * 1024);
                }

                byte[] buf = BuildTargetFollowInfo(PitchCoordinate, AzimuthCoordinate);
                client.Send(buf, buf.Length);
            }
            catch (SocketException _Ex)
            {
                RobotVT.Controller.Methods.SaveExceptionLog(SK_FModel.SystemEnum.LogType.Exception, "UDP发送数据异常：" + _Ex.Message + "\r\n" + _Ex.StackTrace);
                throw new Exception("UDP发送数据失败，错误信息：" + _Ex.Message);
            }
            catch (Exception _Ex)
            {
                RobotVT.Controller.Methods.SaveExceptionLog(SK_FModel.SystemEnum.LogType.Exception, "UDP发送数据异常：" + _Ex.Message + "\r\n" + _Ex.StackTrace);
                throw new Exception("UDP发送数据失败，错误信息：" + _Ex.Message);
            }
        }
        
        private byte[] BuildTargetFollowInfo(short PitchCoordinate, short AzimuthCoordinate)
        {
            TargetFollowInfo.PitchCoordinate = PitchCoordinate;
            TargetFollowInfo.AzimuthCoordinate = AzimuthCoordinate;
            return TargetFollowInfo.TargetFollowMessage();
        }

        /// <summary>
        /// 获取本地IP
        /// </summary>
        /// <returns></returns>
        private string getIPAddress()
        {
            // 获得本机局域网IP地址  
            IPAddress[] AddressList = Dns.GetHostByName(Dns.GetHostName()).AddressList;
            if (AddressList.Length < 1)
            {
                return "";
            }
            return AddressList[0].ToString();
        }
    }
}
    
           
