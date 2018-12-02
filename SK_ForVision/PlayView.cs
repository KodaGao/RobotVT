using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SK_FVision
{
    [System.Security.SecuritySafeCritical]
    public partial class PlayView : UserControl
    {
        #region 预览参数
        private bool m_bRecord = false;
        private uint iLastErr = 0;
        private Int32 m_lUserID = -1;
        private Int32 m_lRealHandle = -1;
        private string str;
        private Int32 m_lPort = -1;
        private IntPtr m_ptrRealHandle;
        private Int32 m_lAlarmHandle = -1;
        public int pWidth = 0, pHeight = 0;

        private HIK_NetSDK.REALDATACALLBACK RealData = null;
        private HIK_NetSDK.NET_DVR_DEVICEINFO_V30 DeviceInfo;
        #endregion

        #region 解码器相关变量声明
        /// <summary>
        /// 数据的句柄
        /// </summary>
        /// <summary>
        /// 这是解码器属性信息
        /// </summary>
        public HI_H264DEC.hiH264_DEC_ATTR_S decAttr;
        /// <summary>
        /// 这是解码器输出图像信息
        /// </summary>
        public HI_H264DEC.hiH264_DEC_FRAME_S _decodeFrame = new HI_H264DEC.hiH264_DEC_FRAME_S();
        /// <summary>
        /// 解码器句柄
        /// </summary>
        public IntPtr _decHandle;
        static double[,] YUV2RGB_CONVERT_MATRIX = new double[3, 3] { { 1, 0, 1.4022 }, { 1, -0.3456, -0.7145 }, { 1, 1.771, 0 } };
        #endregion

        public PlayView()
        {
            InitializeComponent();
            this.Load += new System.EventHandler(this.PlayView_Load);
            RealPlayWnd.MouseUp += new MouseEventHandler(this.RealPlayWnd_MouseUp);
            RealPlayWnd.MouseDoubleClick += new MouseEventHandler(this.RealPlayWnd_MouseDoubleClick);
            RealPlayWnd.MouseMove += new MouseEventHandler(this.RealPlayWnd_MouseMove);
        }

        public virtual void RealPlayWnd_MouseMove(object sender, MouseEventArgs e)
        {
        }

        public virtual void RealPlayWnd_MouseDoubleClick(object sender, MouseEventArgs e)
        {
        }

        public virtual void RealPlayWnd_MouseUp(object sender, MouseEventArgs e)
        { }

        public void GetPictureSize()
        {
            //获取播放句柄 Get the port to play
            if (!HIK_PlayCtrl.PlayM4_GetPictureSize(m_lPort, ref pWidth, ref pHeight))
            {
                iLastErr = HIK_PlayCtrl.PlayM4_GetLastError(m_lPort);
                str = "PlayM4_GetPictureSize failed, error code= " + iLastErr;
                DebugInfo(str);
            }
        }

        public virtual void PlayView_Load(object sender, EventArgs e)
        {
            decAttr = new HI_H264DEC.hiH264_DEC_ATTR_S();
            decAttr.uPictureFormat = 0;
            decAttr.uStreamInType = 0;
            decAttr.uPicWidthInMB = (uint)596;
            decAttr.uPicHeightInMB = (uint)762;
            decAttr.uBufNum = 8;
            decAttr.uWorkMode = 16;
            //创建、初始化解码器句柄
            _decHandle = HI_H264DEC.Hi264DecCreate(ref decAttr);
        }

        public virtual void sdkLogin(string ip, Int16 port, string userName, string password, int channel, uint dwstreamType)
        {
            //登录设备 Login the device
            int userID = HIK_NetSDK.NET_DVR_Login_V30(ip, port, userName, password, ref DeviceInfo);

            if (m_lUserID < 0)
            {
                //登录设备 Login the device
                m_lUserID = HIK_NetSDK.NET_DVR_Login_V30(ip, port, userName, password, ref DeviceInfo);
                if (m_lUserID < 0)
                {
                    iLastErr = HIK_NetSDK.NET_DVR_GetLastError();
                    str = "NET_DVR_Login_V30 failed, error code= " + iLastErr; //登录失败，输出错误号 Failed to login and output the error code;
                    DebugInfo(str);
                    return;
                }
                else
                {
                    //登录成功
                    DebugInfo("NET_DVR_Login_V30 succ!");
                    playScreen();
                }
            }
            return;
        }
        
        public void playScreen(byte[] in_buffer)
        {
            try
            {
                IntPtr pData = IntPtr.Zero;
                pData = SK_FCommon.ValueHelper.Instance.GetIntptr(in_buffer);
                //解码
                //pData 为需要解码的 H264 nalu 数据，length 为该数据的长度
                if (HI_H264DEC.Hi264DecAU(_decHandle, pData, (uint)in_buffer.Length, 0, ref _decodeFrame, 0) == 0)
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
                        //这类代码网上比较常见，我就不贴了
                    }
                }

                //当所有解码操作完成后需要释放解码库，可以放在 FormClosing 事件里做
                //HI_H264DEC.Hi264DecDestroy(_decHandle);
                //IntPtr pUser = IntPtr.Zero;

                //lpPreviewInfo.hPlayWnd = IntPtr.Zero;
                //m_ptrRealHandle = RealPlayWnd.Handle;
                //m_lRealHandle = HIK_NetSDK.NET_DVR_RealPlay_V40(0, ref lpPreviewInfo, RealData, pUser);
                //if (m_lRealHandle < 0)
                //{
                //    iLastErr = HIK_NetSDK.NET_DVR_GetLastError();
                //    str = "NET_DVR_RealPlay_V40 failed, error code= " + iLastErr;
                //    return;
                //}
                //RealPlayWnd.Invalidate();
            }
            catch (Exception e)
            {
            }
        }

        private void playScreen()
        {
            if (m_lUserID < 0)
            {
                MessageBox.Show("Please login the device firstly!");
                return;
            }

            if (m_bRecord)
            {
                MessageBox.Show("Please stop recording firstly!");
                return;
            }

            try
            {
                if (m_lRealHandle < 0)
                {
                    HIK_NetSDK.NET_DVR_PREVIEWINFO lpPreviewInfo = new HIK_NetSDK.NET_DVR_PREVIEWINFO();
                    //lpPreviewInfo.hPlayWnd = RealPlayWnd.Handle;//预览窗口 live view window

                    lpPreviewInfo.hPlayWnd = RealPlayWnd.Handle;
                    lpPreviewInfo.lChannel = 1;//预览的设备通道 the device channel number
                    lpPreviewInfo.dwStreamType = 1;//码流类型：0-主码流，1-子码流，2-码流3，3-码流4，以此类推
                    lpPreviewInfo.dwLinkMode = 0;//连接方式：0- TCP方式，1- UDP方式，2- 多播方式，3- RTP方式，4-RTP/RTSP，5-RSTP/HTTP 
                    lpPreviewInfo.bBlocked = true; //0- 非阻塞取流，1- 阻塞取流
                    lpPreviewInfo.byPreviewMode = 0;
                    lpPreviewInfo.byProtoType = 0;
                    lpPreviewInfo.dwDisplayBufNum = 1; //播放库显示缓冲区最大帧数

                    IntPtr pUser = IntPtr.Zero;
                    //int user_ID = handle.m_userID;

                    //if (playModel == "0")
                    //{

                    //    m_lRealHandle = HIK_NetSDK.NET_DVR_RealPlay_V40(m_lUserID, ref lpPreviewInfo, null, pUser);
                    //}
                    //else
                    //{
                        lpPreviewInfo.hPlayWnd = IntPtr.Zero;
                        //m_ptrRealHandle = RealPlayWnd.Handle;
                        m_ptrRealHandle = RealPlayWnd.Handle;
                    RealData = new HIK_NetSDK.REALDATACALLBACK(RealDataCallBack);
                        m_lRealHandle = HIK_NetSDK.NET_DVR_RealPlay_V40(m_lUserID, ref lpPreviewInfo, RealData, pUser);
                    //}

                    if (m_lRealHandle < 0)
                    {
                        iLastErr = HIK_NetSDK.NET_DVR_GetLastError();
                        str = "NET_DVR_RealPlay_V40 failed, error code= " + iLastErr;
                        return;
                    }
                    //map.Add(index,handle);
                    RealPlayWnd.Invalidate();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        
        public void playScreen(Int32 m_lUserID, bool m_bRecord, Int32 m_lRealHandle, Int32 m_lChannel)
        {
            if (m_lUserID < 0)
            {
                MessageBox.Show("Please login the device firstly!");
                return;
            }

            if (m_bRecord)
            {
                MessageBox.Show("Please stop recording firstly!");
                return;
            }

            try
            {
                if (m_lRealHandle < 0)
                {
                    HIK_NetSDK.NET_DVR_PREVIEWINFO lpPreviewInfo = new HIK_NetSDK.NET_DVR_PREVIEWINFO();
                    //lpPreviewInfo.hPlayWnd = RealPlayWnd.Handle;//预览窗口 live view window

                    lpPreviewInfo.hPlayWnd = RealPlayWnd.Handle;
                    lpPreviewInfo.lChannel = m_lChannel;//预览的设备通道 the device channel number
                    lpPreviewInfo.dwStreamType = 1;//码流类型：0-主码流，1-子码流，2-码流3，3-码流4，以此类推
                    lpPreviewInfo.dwLinkMode = 0;//连接方式：0- TCP方式，1- UDP方式，2- 多播方式，3- RTP方式，4-RTP/RTSP，5-RSTP/HTTP 
                    lpPreviewInfo.bBlocked = true; //0- 非阻塞取流，1- 阻塞取流
                    lpPreviewInfo.byPreviewMode = 0;
                    lpPreviewInfo.byProtoType = 0;
                    lpPreviewInfo.dwDisplayBufNum = 3; //播放库显示缓冲区最大帧数

                    IntPtr pUser = IntPtr.Zero;
                    //int user_ID = handle.m_userID;

                    //if (playModel == "0")
                    //{

                    //    m_lRealHandle = HIK_NetSDK.NET_DVR_RealPlay_V40(m_lUserID, ref lpPreviewInfo, null, pUser);
                    //}
                    //else
                    //{
                    lpPreviewInfo.hPlayWnd = IntPtr.Zero;
                    //m_ptrRealHandle = RealPlayWnd.Handle;

                    m_ptrRealHandle = RealPlayWnd.Handle;

                    RealData = new HIK_NetSDK.REALDATACALLBACK(RealDataCallBack);
                    m_lRealHandle = HIK_NetSDK.NET_DVR_RealPlay_V40(m_lUserID, ref lpPreviewInfo, RealData, pUser);
                    //}

                    if (m_lRealHandle < 0)
                    {
                        iLastErr = HIK_NetSDK.NET_DVR_GetLastError();
                        str = "NET_DVR_RealPlay_V40 failed, error code= " + iLastErr;
                        return;
                    }
                    //map.Add(index,handle);
                    RealPlayWnd.Invalidate();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        
        public void sdkLoginOut()
        {
            stopScreen();

            if (m_lUserID >= 0)
            {
                //注销登录 Logout the device
                if (m_lRealHandle >= 0)
                {
                    DebugInfo("Please stop live view firstly"); //登出前先停止预览 Stop live view before logout
                    return;
                }

                if (!HIK_NetSDK.NET_DVR_Logout(m_lUserID))
                {
                    iLastErr = HIK_NetSDK.NET_DVR_GetLastError();
                    str = "NET_DVR_Logout failed, error code= " + iLastErr;
                    DebugInfo(str);
                    return;
                }
                DebugInfo("NET_DVR_Logout succ!");
                m_lUserID = -1;
            }
            return;
        }
        private void stopScreen()
        {
            //停止预览 Stop live view 
            if (!HIK_NetSDK.NET_DVR_StopRealPlay(m_lRealHandle))
            {
                iLastErr = HIK_NetSDK.NET_DVR_GetLastError();
                str = "NET_DVR_StopRealPlay failed, error code= " + iLastErr;
                DebugInfo(str);
                return;
            }
            if (!HIK_PlayCtrl.PlayM4_Stop(m_lPort))
            {
                iLastErr = HIK_PlayCtrl.PlayM4_GetLastError(m_lPort);
                str = "PlayM4_Stop failed, error code= " + iLastErr;
                DebugInfo(str);
            }
            if (!HIK_PlayCtrl.PlayM4_CloseStream(m_lPort))
            {
                iLastErr = HIK_PlayCtrl.PlayM4_GetLastError(m_lPort);
                str = "PlayM4_CloseStream failed, error code= " + iLastErr;
                DebugInfo(str);
            }
            if (!HIK_PlayCtrl.PlayM4_FreePort(m_lPort))
            {
                iLastErr = HIK_PlayCtrl.PlayM4_GetLastError(m_lPort);
                str = "PlayM4_FreePort failed, error code= " + iLastErr;
                DebugInfo(str);
            }
            m_lPort = -1;

            DebugInfo("NET_DVR_StopRealPlay succ!");
            m_lRealHandle = -1;
            RealPlayWnd.Invalidate();//刷新窗口 refresh the window
        }

        public virtual void sdkSetAlarm()
        {
            HIK_NetSDK.NET_DVR_SETUPALARM_PARAM struAlarmParam = new HIK_NetSDK.NET_DVR_SETUPALARM_PARAM();
            struAlarmParam.dwSize = (uint)Marshal.SizeOf(struAlarmParam);
            //struAlarmParam.byLevel = 0; //0- 一级布防,1- 二级布防
            //struAlarmParam.byAlarmInfoType = 1;
            struAlarmParam.byFaceAlarmDetection = 0;

            m_lAlarmHandle = SK_FVision.HIK_NetSDK.NET_DVR_SetupAlarmChan_V41(m_lUserID, ref struAlarmParam);
            if (m_lAlarmHandle < 0)
            {
                iLastErr = SK_FVision.HIK_NetSDK.NET_DVR_GetLastError();
                str = "布防失败，错误号：" + iLastErr;
                sdkLoginOut();
            }
            else
            {
                str = "布防成功，设备SN：" + System.Text.Encoding.UTF8.GetString(DeviceInfo.sSerialNumber).TrimEnd('\0'); //布防成功，输出设备序列号                
            }
            DebugInfo(str);

            //System.Text.Encoding.UTF8.GetString(DeviceInfo.sSerialNumber);
        }

        public void SetAlarm(int m_lUserID)
        {
            HIK_NetSDK.NET_DVR_SETUPALARM_PARAM struAlarmParam = new HIK_NetSDK.NET_DVR_SETUPALARM_PARAM();
            struAlarmParam.dwSize = (uint)Marshal.SizeOf(struAlarmParam);
            //struAlarmParam.byLevel = 0; //0- 一级布防,1- 二级布防
            //struAlarmParam.byAlarmInfoType = 1;
            struAlarmParam.byFaceAlarmDetection = 1;

            m_lAlarmHandle = SK_FVision.HIK_NetSDK.NET_DVR_SetupAlarmChan_V41(m_lUserID, ref struAlarmParam);
            if (m_lAlarmHandle < 0)
            {
                iLastErr = SK_FVision.HIK_NetSDK.NET_DVR_GetLastError();
                str = "布防失败，错误号：" + iLastErr;
                sdkLoginOut();
            }
            else
            {
                str = "布防成功，设备SN："; //布防成功，输出设备序列号                
            }
            DebugInfo(str);
        }

        public virtual void sdkCloseAlarm()
        {
            if (m_lAlarmHandle >= 0)
            {
                if (!HIK_NetSDK.NET_DVR_CloseAlarmChan_V30(m_lAlarmHandle))
                {
                    iLastErr = HIK_NetSDK.NET_DVR_GetLastError();
                    str = "撤防失败，错误号：" + iLastErr; //撤防失败，输出错误号
                }
                else
                {
                    str = "撤防成功，设备SN：" + DeviceInfo.sSerialNumber.ToString(); //布防成功，输出设备序列号
                    m_lAlarmHandle = -1;
                }
            }
            else
            {
                str = "未布防，设备SN：" + DeviceInfo.sSerialNumber.ToString(); //布防成功，输出设备序列号
            }
            DebugInfo(str);
        }
        
        private void DebugInfo(string str)
        {
            if (str.Length > 0)
            {
                //Exception_LabeX.Text = str;
            }
        }

        /// <summary>
        /// 数据处理委托
        /// </summary>
        private delegate void DealVideoDelegate(Int32 lRealHandle, UInt32 dwDataType, IntPtr pBuffer, UInt32 dwBufSize, IntPtr pUser);
        /// <summary>
        /// 码流获取后触发回调函数
        /// </summary>
        public void RealDataCallBack(Int32 lRealHandle, UInt32 dwDataType, IntPtr pBuffer, UInt32 dwBufSize, IntPtr pUser)
        {
            //视频数据使用委托方式处理，否则会出现内存回收异常
            DealVideoDelegate videoDlegate = new DealVideoDelegate(dealVideo);
            videoDlegate(lRealHandle, dwDataType, pBuffer, dwBufSize, pUser);
        }
        /// <summary>
        /// 视频数据处理
        /// </summary>
        private void dealVideo(Int32 lRealHandle, UInt32 dwDataType, IntPtr pBuffer, UInt32 dwBufSize, IntPtr pUser)
        {
            switch (dwDataType)
            {
                case HIK_NetSDK.NET_DVR_SYSHEAD:     // sys head
                    if (dwBufSize > 0)
                    {
                        if (m_lPort >= 0)
                        {
                            return; //同一路码流不需要多次调用开流接口
                        }

                        //获取播放句柄 Get the port to play
                        if (!HIK_PlayCtrl.PlayM4_GetPort(ref m_lPort))
                        {
                            iLastErr = HIK_PlayCtrl.PlayM4_GetLastError(m_lPort);
                            str = "PlayM4_GetPort failed, error code= " + iLastErr;
                            DebugInfo(str);
                            break;
                        }

                        //设置流播放模式 Set the stream mode: real-time stream mode
                        if (!HIK_PlayCtrl.PlayM4_SetStreamOpenMode(m_lPort, HIK_PlayCtrl.STREAME_REALTIME))
                        {
                            iLastErr = HIK_PlayCtrl.PlayM4_GetLastError(m_lPort);
                            str = "Set STREAME_REALTIME mode failed, error code= " + iLastErr;
                            DebugInfo(str);
                        }

                        //打开码流，送入头数据 Open stream
                        if (!HIK_PlayCtrl.PlayM4_OpenStream(m_lPort, pBuffer, dwBufSize, 10 * 1024 * 1024))
                        {
                            iLastErr = HIK_PlayCtrl.PlayM4_GetLastError(m_lPort);
                            str = "PlayM4_OpenStream failed, error code= " + iLastErr;
                            DebugInfo(str);
                            break;
                        }
                        //设置显示缓冲区个数 Set the display buffer number
                        if (!HIK_PlayCtrl.PlayM4_SetDisplayBuf(m_lPort, 15))
                        {
                            iLastErr = HIK_PlayCtrl.PlayM4_GetLastError(m_lPort);
                            str = "PlayM4_SetDisplayBuf failed, error code= " + iLastErr;
                        }
                        //设置显示模式 Set the display mode
                        if (!HIK_PlayCtrl.PlayM4_SetOverlayMode(m_lPort, 1, 0/* COLORREF(0)*/)) //play off screen 
                        {
                            iLastErr = HIK_PlayCtrl.PlayM4_GetLastError(m_lPort);
                            str = "PlayM4_SetOverlayMode failed, error code= " + iLastErr;
                            DebugInfo(str);
                        }

                        ////设置硬解码
                        //if (!HIK_PlayCtrl.PlayM4_SetDecodeEngineEx(m_lPort, 1))
                        //{
                        //    iLastErr = HIK_PlayCtrl.PlayM4_GetLastError(m_lPort);
                        //    str = "PlayM4_SetDecodeEngineEx failed, error code= " + iLastErr;
                        //    Console.WriteLine(str);
                        //    break;
                        //}

                        //开始解码 Start to play                       
                        if (!HIK_PlayCtrl.PlayM4_Play(m_lPort, m_ptrRealHandle))
                        {
                            iLastErr = HIK_PlayCtrl.PlayM4_GetLastError(m_lPort);
                            str = "PlayM4_Play failed, error code= " + iLastErr;
                            DebugInfo(str);
                            break;
                        }
                    }
                    break;
                case HIK_NetSDK.NET_DVR_STREAMDATA:     // video stream data
                    if (dwBufSize > 0 && m_lPort != -1)
                    {
                        for (int i = 0; i < 999; i++)
                        {
                            if (!HIK_PlayCtrl.PlayM4_InputData(m_lPort, pBuffer, dwBufSize))
                            {
                                iLastErr = HIK_PlayCtrl.PlayM4_GetLastError(m_lPort);
                                str = "PlayM4_InputData failed, error code= " + iLastErr;
                                DebugInfo(str);
                                System.Threading.Thread.Sleep(2);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    break;
                default:
                    if (dwBufSize > 0 && m_lPort != -1)
                    {
                        //送入其他数据 Input the other data
                        for (int i = 0; i < 999; i++)
                        {
                            if (!HIK_PlayCtrl.PlayM4_InputData(m_lPort, pBuffer, dwBufSize))
                            {
                                iLastErr = HIK_PlayCtrl.PlayM4_GetLastError(m_lPort);
                                str = "PlayM4_InputData failed, error code= " + iLastErr;
                                DebugInfo(str);
                                System.Threading.Thread.Sleep(2);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    break;
            }
        }

    }
}