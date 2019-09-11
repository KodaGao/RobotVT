using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;

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

        public bool MulticastThreadingIsRun = false;

        Socket udpReceiveViedo;
        Socket udpReceiveInfo;
        EndPoint epViedo;
        EndPoint epInfo;
        EndPoint epTargetSend;

        TargetFollowInfo TargetFollowInfo;
        TargetFollowRecvInfo recvInfo;

        /// <summary>
        /// 相关资源初始化
        /// </summary>
        public unsafe void Init()
        {
            string localip = GetIPAddress();
            IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(localip), StaticInfo.MulticastGroupPort);
            udpReceiveViedo = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            epViedo = (EndPoint)ipe;
            udpReceiveViedo.Bind(ipe);
            udpReceiveViedo.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(IPAddress.Parse(StaticInfo.MulticastGroupIP)));


            IPEndPoint ipe2 = new IPEndPoint(IPAddress.Parse(localip), StaticInfo.TargetFollowPort);
            udpReceiveInfo = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            epInfo = (EndPoint)ipe2;
            udpReceiveInfo.Bind(ipe2);
            udpReceiveInfo.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(IPAddress.Parse(StaticInfo.MulticastGroupIP)));


            epTargetSend = new IPEndPoint(IPAddress.Parse(StaticInfo.TargetFollowIP), StaticInfo.TargetFollowPort);

            TargetFollowInfo = new TargetFollowInfo();

        }
        //14-22970970
        /// <summary>
        /// 接收组播数据
        /// </summary>
        public void Start()
        {
            System.Threading.Thread tmultcast = new System.Threading.Thread(new System.Threading.ThreadStart(RecvThread));
            tmultcast.IsBackground = true;
            tmultcast.Start();
            MulticastThreadingIsRun = true;


            System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(RecvThread2));
            t.IsBackground = true;
            t.Start();;
        }

        /// <summary>
        /// 接收组播数据
        /// </summary>
        public void Stop()
        {
            udpReceiveViedo.Disconnect(true);
            udpReceiveViedo.Dispose();

            udpReceiveInfo.Disconnect(true);
            udpReceiveInfo.Dispose();

            MulticastThreadingIsRun = false;
        }
    
        private void RecvThread()
        {
            try
            {
                int retlen = 0;
                List<byte> recvBuflist = new List<byte>();

                while (true)
                {
                    byte[] in_buffer=new byte[1400];
                    int in_buffer_size = udpReceiveViedo.ReceiveFrom(in_buffer, ref epViedo);
                    if (in_buffer[0] == 0xEB && in_buffer[1] == 0x90 && in_buffer[2] == 0xFF)
                    {
                        PicRecBuf(in_buffer, ref retlen, ref recvBuflist);
                    }
                    //System.Threading.Thread.Sleep(1);
                }
            }
            catch (Exception _Ex)
            {
                RobotVT.Controller.Methods.SaveExceptionLog(SK_FModel.SystemEnum.LogType.Exception, "组播数据解析异常：" + _Ex.Message + "\r\n" + _Ex.StackTrace);
                throw new Exception("组播数据解析失败，错误信息：" + _Ex.Message);
            }
        }
        private void RecvThread2()
        {
            try
            {
                EndPoint epTargetRevice = new IPEndPoint(IPAddress.Parse(StaticInfo.TargetFollowIP), 0);
                while (true)
                {
                    byte[] in_buffer = new byte[12];
                    int in_buffer_size = udpReceiveInfo.ReceiveFrom(in_buffer, ref epTargetRevice);
                    if (in_buffer[0] == 0xaa && in_buffer[1] == 0x55)
                    {
                        TargetRecBuf(in_buffer);
                    }
                    System.Threading.Thread.Sleep(1);
                }
            }
            catch (Exception _Ex)
            {
                RobotVT.Controller.Methods.SaveExceptionLog(SK_FModel.SystemEnum.LogType.Exception, "组播数据解析异常：" + _Ex.Message + "\r\n" + _Ex.StackTrace);
                throw new Exception("组播数据解析失败，错误信息：" + _Ex.Message);
            }
        }


        //FileStream fs = new FileStream(Directory.GetCurrentDirectory() + "\\123.h264", FileMode.Create);
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

                ////开始写入
                //fs.Write(recvBuflist.ToArray(), 0, recvBuflist.Count);


                if (retlen == recvBuflist.Count)
                {
                    Event_Multicast?.Invoke(recvBuflist);
                    recvBuflist.Clear();
                    retlen = 0;
                }
            }
        }

        private void TargetRecBuf(byte[] recvBuf)
        {
            recvInfo = new TargetFollowRecvInfo(recvBuf);
            Event_TargetCoordinates?.Invoke(recvInfo);
        }

        /// <summary>
        /// 发送坐标数据
        /// </summary>
        public void SendingCoordinates(int tracking,int imageWidth, int imageHeight, Point MouseLocation)
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

                    AzimuthCoordinate = (short)((float)(MouseLocation.X - centerX) / imageWidth * 1080);
                    PitchCoordinate = (short)((float)(centerY - MouseLocation.Y) / imageHeight * 1080);
                }

                byte[] buf = BuildTargetFollowInfo(tracking,PitchCoordinate, AzimuthCoordinate);

                udpReceiveInfo.SendTo(buf, epTargetSend);

                //aa 55 00 00 00 00 fe 00 00 60 00 70 cd
                //aa 55 00 25 00 00 fe 00 00 60 00 70 f2
                //aa 55 00 00 00 00 fe 00 00 60 00 70 cd
                //aa 55 00 00 00 00 fe 00 00 00 00 00 00
                //aa 55 00 00 00 00 fe 00 00 60 00 70 cd

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
        
        private byte[] BuildTargetFollowInfo(int tracking, short PitchCoordinate, short AzimuthCoordinate)
        {
            TargetFollowInfo.PitchCoordinate = PitchCoordinate;
            TargetFollowInfo.AzimuthCoordinate = AzimuthCoordinate;
            switch(tracking)
            {
                case 0:
                    TargetFollowInfo.Command = TargetFollowEnum.TargetCommand.Check;
                    break;
                case 1:
                    TargetFollowInfo.Command = TargetFollowEnum.TargetCommand.Cancel;
                    break;
                case 2:
                    TargetFollowInfo.Command = TargetFollowEnum.TargetCommand.SDI;
                    break;
                default:
                    TargetFollowInfo.Command = TargetFollowEnum.TargetCommand.Check;
                    break;         
            }

            return TargetFollowInfo.TargetFollowMessage();
        }

        /// <summary>
        /// 获取本地IP
        /// </summary>
        /// <returns></returns>
        private string GetIPAddress()
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
    
           
