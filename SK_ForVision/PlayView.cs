using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SK_FVision
{
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

        private HIK_NetSDK.REALDATACALLBACK RealData = null;
        private HIK_NetSDK.NET_DVR_DEVICEINFO_V30 DeviceInfo;
        //private HIK_NetSDK.NET_DVR_IPPARACFG_V40 m_struIpParaCfgV40;
        //private HIK_NetSDK.NET_DVR_STREAM_MODE m_struStreamMode;
        //private HIK_NetSDK.NET_DVR_IPCHANINFO m_struChanInfo;
        //private HIK_NetSDK.NET_DVR_IPCHANINFO_V40 m_struChanInfoV40;
        //private HIK_NetSDK.NET_DVR_MATRIX_DECCHAN_CONTROL m_struMatrixDecchan;
        //private HIK_PlayCtrl.DECCBFUN m_fDisplayFun = null;
        #endregion


        public PlayView()
        {
            InitializeComponent();
            RealPlayWnd.MouseUp += new MouseEventHandler(this.RealPlayWnd_MouseUp);
        }

        public virtual void RealPlayWnd_MouseUp(object sender, MouseEventArgs e)
        { }

        private void PlayView_Load(object sender, EventArgs e)
        {
        }

        public virtual void sdkLogin(string ip, Int16 port, string userName, string password, int channel, uint dwstreamType)
        {
            ////设置异常回调
            //HIK_NetSDK.NET_DVR_SetExceptionCallBack_V30(0, IntPtr.Zero, g_ExceptionCallBack, IntPtr.Zero);

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

            ////handle为空则不播放
            //if (!map.ContainsKey(index))
            //{
            //    return;
            //}


            //ViewHandle handle = map[index];
            try
            {
                if (m_lRealHandle < 0)
                {
                    HIK_NetSDK.NET_DVR_PREVIEWINFO lpPreviewInfo = new HIK_NetSDK.NET_DVR_PREVIEWINFO();
                    lpPreviewInfo.hPlayWnd = RealPlayWnd.Handle;//预览窗口 live view window
                    lpPreviewInfo.lChannel = 1;//预览的设备通道 the device channel number
                    lpPreviewInfo.dwStreamType = 0;//码流类型：0-主码流，1-子码流，2-码流3，3-码流4，以此类推
                    lpPreviewInfo.dwLinkMode = 0;//连接方式：0- TCP方式，1- UDP方式，2- 多播方式，3- RTP方式，4-RTP/RTSP，5-RSTP/HTTP 
                    lpPreviewInfo.bBlocked = true; //0- 非阻塞取流，1- 阻塞取流
                    lpPreviewInfo.byPreviewMode = 0;
                    lpPreviewInfo.byProtoType = 0;
                    lpPreviewInfo.dwDisplayBufNum = 1; //播放库显示缓冲区最大帧数

                    IntPtr pUser = IntPtr.Zero;
                    //int user_ID = handle.m_userID;

                    //if (playModel == "0")
                    //{

                    //    m_lRealHandle = HIK_NetSDK.NET_DVR_RealPlay_V40(user_ID, ref lpPreviewInfo, null, pUser);
                    //}
                    //else
                    //{
                    lpPreviewInfo.hPlayWnd = IntPtr.Zero;
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
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void LoginOut()
        {
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