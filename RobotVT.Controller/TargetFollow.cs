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


        public Bitmap H264(byte[] recvbuf)
        {
            AVCodecContext* pCodecCtx = null;
            AVCodecParserContext* pCodecParserCtx = null;
            AVCodec* pCodec = null;
            AVFrame* pFrame = null;             //yuv 
            AVPacket packet;                    //h264 
            AVPacket* _packet;
            AVPicture picture;                  //储存rgb格式图片 
            SwsContext* pSwsCtx = null;
            AVCodecID codec_id = AVCodecID.AV_CODEC_ID_H264;

            int ret;
            /* 初始化AVCodec */
            pCodec = ffmpeg.avcodec_find_decoder(codec_id);

            /* 初始化AVCodecContext,只是分配，还没打开 */
            pCodecCtx = ffmpeg.avcodec_alloc_context3(pCodec);

            /* 初始化AVCodecParserContext */
            pCodecParserCtx = ffmpeg.av_parser_init((int)AVCodecID.AV_CODEC_ID_H264);
            if (null == pCodecParserCtx)
            {
                return null;//终止执行
            }

            /* we do not send complete frames,什么意思？ */
            if (pCodec->capabilities > 0 && ffmpeg.AV_CODEC_CAP_TRUNCATED > 0)
                pCodecCtx->flags |= ffmpeg.AV_CODEC_FLAG_TRUNCATED;

            /* 打开解码器 */
            ret = ffmpeg.avcodec_open2(pCodecCtx, pCodec, null);
            if (ret < 0)
            {
                return null;//终止执行
            }
            pFrame = ffmpeg.av_frame_alloc();
            ffmpeg.av_init_packet(&packet);
            packet.size = recvbuf.Length;//这个填入H264数据帧的大小  
            packet.data = (byte*)BytesToIntptr(recvbuf);//这里填入一个指向完整H264数据帧的指针

            _packet = ffmpeg.av_packet_alloc();

            int got;
            bool is_first_time = true;

            //下面开始真正的解码  
            ret = ffmpeg.avcodec_decode_video2(pCodecCtx, pFrame, &got, &packet);
            if (ret < 0)
            {
                return null;//终止执行
            }
            if (got > 0)
            {
                if (is_first_time)  //分配格式转换存储空间 
                {
                    // C AV_PIX_FMT_RGB32 统一改为 AVPixelFormat.AV_PIX_FMT_RGB24
                    pSwsCtx = ffmpeg.sws_getContext(pCodecCtx->width, pCodecCtx->height, pCodecCtx->pix_fmt,
                        pCodecCtx->width, pCodecCtx->height, AVPixelFormat.AV_PIX_FMT_RGB24, ffmpeg.SWS_BICUBIC, null, null, null);

                    ffmpeg.avpicture_alloc(&picture, AVPixelFormat.AV_PIX_FMT_RGB24, pCodecCtx->width, pCodecCtx->height);

                    is_first_time = false;
                }

                /* YUV转RGB */
                ffmpeg.sws_scale(pSwsCtx, pFrame->data, pFrame->linesize,
                    0, pCodecCtx->height,
                    picture.data, picture.linesize);

                #region 构造图片
                var dstData = new byte_ptrArray4();// 声明形参
                var dstLinesize = new int_array4();// 声明形参
                                                   // 目标媒体格式需要的字节长度
                var convertedFrameBufferSize = ffmpeg.av_image_get_buffer_size(AVPixelFormat.AV_PIX_FMT_RGB24, pCodecCtx->width, pCodecCtx->height, 1);
                // 分配目标媒体格式内存使用
                var convertedFrameBufferPtr = Marshal.AllocHGlobal(convertedFrameBufferSize);
                // 设置图像填充参数
                ffmpeg.av_image_fill_arrays(ref dstData, ref dstLinesize, (byte*)convertedFrameBufferPtr, AVPixelFormat.AV_PIX_FMT_RGB24, pCodecCtx->width, pCodecCtx->height, 1);

                // 封装Bitmap图片
                Bitmap bitmap = new Bitmap(pCodecCtx->width, pCodecCtx->height, dstLinesize[0], PixelFormat.Format24bppRgb, convertedFrameBufferPtr);
                #endregion

                ffmpeg.av_free_packet(&packet);
                ffmpeg.av_frame_free(&pFrame);
                ffmpeg.avpicture_free(&picture);
                ffmpeg.sws_freeContext(pSwsCtx);
                ffmpeg.avcodec_free_context(&pCodecCtx);
                ffmpeg.av_parser_close(pCodecParserCtx);
                return bitmap;
            }
            ffmpeg.av_free_packet(&packet);
            ffmpeg.av_frame_free(&pFrame);
            ffmpeg.avpicture_free(&picture);
            ffmpeg.sws_freeContext(pSwsCtx);
            ffmpeg.avcodec_free_context(&pCodecCtx);
            ffmpeg.av_parser_close(pCodecParserCtx);
            return null;
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
    
           
