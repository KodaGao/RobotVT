using FFmpeg.AutoGen;
using SK_FVision.FFmpeg.AutoGen;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
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


            //初始化解码器
            var decAttr = new HI_H264Dec.hiH264_DEC_ATTR_S();
            decAttr.uPictureFormat = 0;
            decAttr.uStreamInType = 0;
            decAttr.uPicWidthInMB = 1024 >> 4;
            decAttr.uPicHeightInMB = 1024 >> 4;
            decAttr.uBufNum = 8;
            decAttr.uWorkMode = 16;
            _decHandle = HI_H264Dec.Hi264DecCreate(ref decAttr);


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

            //释放解码库
            HI_H264Dec.Hi264DecDestroy(_decHandle);
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
                    //H264_1(recvBuflist.ToArray());
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

        #region 解码器相关变量声明
        /// <summary>
        /// 数据的句柄
        /// </summary>
        /// <summary>
        /// 这是解码器属性信息
        /// </summary>
        public HI_H264Dec.hiH264_DEC_ATTR_S decAttr;
        /// <summary>
        /// 这是解码器输出图像信息
        /// </summary>
        public HI_H264Dec.hiH264_DEC_FRAME_S _decodeFrame = new HI_H264Dec.hiH264_DEC_FRAME_S();
        /// <summary>
        /// 解码器句柄
        /// </summary>
        public IntPtr _decHandle;
        static double[,] YUV2RGB_CONVERT_MATRIX = new double[3, 3] { { 1, 0, 1.4022 }, { 1, -0.3456, -0.7145 }, { 1, 1.771, 0 } };
        #endregion

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

        public unsafe void H264(byte[] recvBuf)
        {
            HI_H264Dec.hiH264_DEC_FRAME_S _decodeFrame = new HI_H264Dec.hiH264_DEC_FRAME_S();
            IntPtr pData = BytesToIntptr(recvBuf);
            uint length = (uint)pData;
            //解码
            //pData 为需要解码的 H264 nalu 数据，length 为该数据的长度
            if (HI_H264Dec.Hi264DecAU(_decHandle, pData, (uint)length, 0, ref _decodeFrame, 0) == 0)
            {
                if (_decodeFrame.bError == 0)
                {
                    //计算 y u v 的长度
                    var yLength = _decodeFrame.uHeight * _decodeFrame.uYStride;
                    var uLength = _decodeFrame.uHeight * _decodeFrame.uUVStride / 2;
                    var vLength = uLength;
                    var yBytes = new byte[yLength];
                    var uBytes = new byte[uLength];
                    var vBytes = new byte[vLength];
                    var decodedBytes = new byte[yLength + uLength + vLength];
                    //_decodeFrame 是解码后的数据对象，里面包含 YUV 数据、宽度、高度等信息
                    Marshal.Copy(_decodeFrame.pY, yBytes, 0, (int)yLength);
                    Marshal.Copy(_decodeFrame.pU, uBytes, 0, (int)uLength);
                    Marshal.Copy(_decodeFrame.pV, vBytes, 0, (int)vLength);
                    //将从 _decodeFrame 中取出的 YUV 数据放入 decodedBytes 中
                    Array.Copy(yBytes, decodedBytes, yLength);
                    Array.Copy(uBytes, 0, decodedBytes, yLength, uLength);
                    Array.Copy(vBytes, 0, decodedBytes, yLength + uLength, vLength);

                    //decodedBytes 为yuv数据，可以将其转换为 RGB 数据后再转换为 BitMap 然后通过 PictureBox 控件即可显示
                    
                }
            }

            //当所有解码操作完成后需要释放解码库，可以放在 FormClosing 事件里做
            HI_H264Dec.Hi264DecDestroy(_decHandle);
        }

        public unsafe IntPtr H264_1(byte[] recvBuf)
        {

            int size = recvBuf.Length;
            IntPtr buffer = Marshal.AllocHGlobal(size);

            #region ffmpeg.autogen

            AVCodecContext* pCodecCtx = null;
            AVCodecParserContext* pCodecParserCtx = null;
            AVCodec* pCodec = null;
            AVFrame* pFrame = null;             //yuv 
            AVPacket packet;                    //h264 
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
                return buffer;//终止执行
            }

            /* we do not send complete frames,什么意思？ */
            if (pCodec->capabilities > 0 && ffmpeg.AV_CODEC_CAP_TRUNCATED > 0)
                pCodecCtx->flags |= ffmpeg.AV_CODEC_FLAG_TRUNCATED;

            /* 打开解码器 */
            ret = ffmpeg.avcodec_open2(pCodecCtx, pCodec, null);
            if (ret < 0)
            {
                return buffer;//终止执行
            }
            #endregion
            pFrame = ffmpeg.av_frame_alloc();
            ffmpeg.av_init_packet(&packet);
            packet.size = 0;
            packet.data = null;

            const int in_buffer_size = 4096;
            //uint in_buffer[in_buffer_size + FF_INPUT_BUFFER_PADDING_SIZE] = { 0 };
            byte[] in_buffer = new byte[in_buffer_size];
            byte* cur_ptr;
            int cur_size;
            int got;
            bool is_first_time = true;
            
            cur_size = recvBuf.Length;
            //cur_ptr = in_buffer;//指针转换问题
            cur_ptr = (byte*)ffmpeg.av_malloc(in_buffer_size);
            Marshal.Copy(recvBuf, 0, (IntPtr)cur_ptr, cur_size);
            while (cur_size > 0)
            {
                /* 返回解析了的字节数 */
                int len = ffmpeg.av_parser_parse2(pCodecParserCtx, pCodecCtx,
                    &packet.data, &packet.size, (byte*)cur_ptr, cur_size,
                    ffmpeg.AV_NOPTS_VALUE, ffmpeg.AV_NOPTS_VALUE, ffmpeg.AV_NOPTS_VALUE);
                cur_ptr += len;
                cur_size -= len;
                if (packet.size == 0)
                    continue;

                //ret = ffmpeg.avcodec_decode_video2(pCodecCtx, pFrame, &got, &packet);
                ret = ffmpeg.avcodec_send_packet(pCodecCtx, &packet);
                if (ret < 0)
                {
                    return buffer;//终止执行
                }

                got = ffmpeg.avcodec_receive_frame(pCodecCtx, pFrame);
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
                    var bitmap = new Bitmap(pCodecCtx->width, pCodecCtx->height, dstLinesize[0], PixelFormat.Format24bppRgb, convertedFrameBufferPtr);

                    #endregion
                }
            }
            ffmpeg.av_packet_unref(&packet);
            ffmpeg.av_frame_free(&pFrame);
            ffmpeg.avpicture_free(&picture);
            ffmpeg.sws_freeContext(pSwsCtx);
            ffmpeg.avcodec_free_context(&pCodecCtx);
            ffmpeg.av_parser_close(pCodecParserCtx);
            return buffer;
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
    
           
