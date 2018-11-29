using FFmpeg.AutoGen;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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

            InitDecoder(codec_id);
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

            //ffmpeg.av_packet_free(&packet);
            //ffmpeg.av_frame_free(&pFrame);
            //ffmpeg.avcodec_free_context(&pCodecCtx);
            ffmpeg.av_parser_close(pCodecParserCtx);
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
                    //DecodeFrame(recvBuflist.ToArray());
                    Event_Multicast?.Invoke(recvBuflist);
                    recvBuflist.Clear();
                    retlen = 0;
                    Thread.Sleep(1);
                }

            }
        }

        private AVCodecContext* pCodecCtx = null;
        private AVCodecParserContext* pCodecParserCtx = null;
        private AVCodec* pCodec = null;
        private AVFrame* pFrame = null;             //yuv 
        private AVPacket packet;                    //h264 
        private AVPacket* pPacket;
        readonly AVCodecID codec_id = AVCodecID.AV_CODEC_ID_H264;
        static bool codecInited = false;

        private int InitDecoder(AVCodecID codec_id)
        {
            if (codecInited != false)
            {
                return -1;
            }

            int ret;

            pFrame = ffmpeg.av_frame_alloc();
            pPacket = ffmpeg.av_packet_alloc();
            /* 初始化AVCodec */
            pCodec = ffmpeg.avcodec_find_decoder(codec_id);
            /* 初始化AVCodecParserContext */
            pCodecParserCtx = ffmpeg.av_parser_init((int)codec_id);
            if (null == pCodecParserCtx)
            {
                return -1;//终止执行
            }

            /* 初始化AVCodecContext,只是分配，还没打开 */
            pCodecCtx = ffmpeg.avcodec_alloc_context3(pCodec);

            /* 打开解码器 */
            ret = ffmpeg.avcodec_open2(pCodecCtx, pCodec, null);
            if (ret < 0)
            {
                return -1;//终止执行
            }

            ffmpeg.av_init_packet(pPacket);

            codecInited = true;
            return 0;
        }

        public System.IO.MemoryStream DecodeFrame(byte[] recv)
        {
            System.IO.MemoryStream _stream = new System.IO.MemoryStream();
            IntPtr buffer = Marshal.AllocHGlobal(recv.Length);
            try
            {
                Marshal.Copy(recv, 0, buffer, recv.Length);
                while (recv.Length > 0)
                {
                    int ret = ffmpeg.av_parser_parse2(pCodecParserCtx, pCodecCtx, (byte**)(pPacket->data), (int*)(pPacket->size),
                    (byte*)buffer, recv.Length, ffmpeg.AV_NOPTS_VALUE, ffmpeg.AV_NOPTS_VALUE, 0);
                    if (ret < 0)
                    {
                        return _stream;
                    }

                    if (pPacket->size == 0)
                        continue;

                    _stream = Decode(pCodecCtx, pFrame, pPacket);
                }
                _stream = Decode(pCodecCtx, pFrame, pPacket);
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
            return _stream;
        }

         private System.IO.MemoryStream Decode(AVCodecContext* dec_ctx, AVFrame* frame, AVPacket* pkt)
        {
            System.IO.MemoryStream _stream = new System.IO.MemoryStream();
            //var pPacket = ffmpeg.av_packet_alloc();
            try
            {
                int ret;
                ret = ffmpeg.avcodec_send_packet(pCodecCtx, pPacket);
                if (ret < 0)
                {
                    return _stream;
                }
                while (ret >= 0)
                {
                    ret = ffmpeg.avcodec_receive_frame(pCodecCtx, pFrame);
                    if (ret == ffmpeg.AVERROR(ffmpeg.EAGAIN) || ret == ffmpeg.AVERROR_EOF)
                        return _stream ;
                    else if (ret < 0)
                        break;
                }
                using (var packetStream = new UnmanagedMemoryStream((byte*)pFrame, pPacket->size)) packetStream.CopyTo(_stream);
            }
            finally
            {
                //ffmpeg.av_packet_unref(pPacket);
            }
            return _stream;
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
    
           
