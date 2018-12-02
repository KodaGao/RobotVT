using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
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
            TargetFollowInfo = new TargetFollowInfo();

        }
        //14-22970970
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


        private void RecvThread()
        {
            try
            {
                int retlen = 0;
                List<byte> recvBuflist = new List<byte>();

                while (true)
                {
                    byte[] in_buffer = new byte[1400];
                    int in_buffer_size = udpReceive.ReceiveFrom(in_buffer, ref ep);
                    //cur_size = udpReceive.Receive(in_buffer, in_buffer_size, SocketFlags.None);
                    if (in_buffer[0] == 0xEB && in_buffer[1] == 0x90 && in_buffer[2] == 0xFF)
                    {
                        PicRecBuf(in_buffer, ref retlen, ref recvBuflist);
                    }
                    if (in_buffer[0] == 0xaa && in_buffer[1] == 0x0e)
                    {
                        TargetRecBuf(in_buffer);
                    }
                    //AForge.Video.FFMPEG.VideoCodec.H263P
                }
            }
            catch (Exception _Ex)
            {
                RobotVT.Controller.Methods.SaveExceptionLog(SK_FModel.SystemEnum.LogType.Exception, "组播数据解析异常：" + _Ex.Message + "\r\n" + _Ex.StackTrace);
                throw new Exception("组播数据解析失败，错误信息：" + _Ex.Message);
            }
        }




        FileStream fsWrite = new FileStream(StaticInfo.LogModbusPath + "test.avi", FileMode.OpenOrCreate, FileAccess.Write);

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
                    //Methods.SaveModbusLog(SK_FModel.SystemEnum.LogType.Normal, "[H264]" + "接收：" + BitConverter.ToString(recvBuflist.ToArray()).Replace("-", " "));

                    //try
                    //{
                    //    fsWrite.Write(recvBuflist.ToArray(), 0, retlen);
                    //}
                    //catch (Exception ex)
                    //{
                    //    throw ex;
                    //}

                    //cur_size = recvBuflist.Count;
                    //cur_ptr = (byte*)ffmpeg.av_malloc((ulong)cur_size + in_buffer_size);
                    //////解析数据，并还原参数
                    //DecodeFrame(recvBuflist.ToArray());
                    Event_Multicast?.Invoke(recvBuflist);
                    recvBuflist.Clear();
                    retlen = 0;
                    Thread.Sleep(1);
                }
            }
        }

        private void TargetRecBuf(byte[] recvBuf)
        {
            recvInfo = new TargetFollowRecvInfo(recvBuf);

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

                    AzimuthCoordinate = (short)((float)(MouseLocation.X  - centerX) / imageWidth * 1024);
                    PitchCoordinate = (short)((float)(centerY  - MouseLocation.Y) / imageHeight * 1024);
                }

                byte[] buf = BuildTargetFollowInfo(PitchCoordinate, AzimuthCoordinate);
                client.Send(buf, buf.Length);
                //aa:55:00:00:00:00:fe:00:00:60:00:70:cd
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
    
           
