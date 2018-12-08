using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SK_FVision
{
    public class HIK_CameraMng
    {
        #region 预览参数
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

        private string m_DevIP = "";
        /// <summary>
        /// 运行消息事件
        /// </summary>
        public delegate void HIK_RuningMessage(string MessageInfo);

        public event HIK_RuningMessage Event_RuningMessage;

        /// <summary>
        /// Debug消息事件
        /// </summary>
        public delegate void HIK_DebugMessage(string MessageInfo);

        public event HIK_DebugMessage Event_DebugMessage;

        public Int32 UserID { get; set; }
        public Int32 RealHandle { get; set; }
        
        public Int32 HIK_SdkLogin(string ip, Int16 port, string userName, string password)
        {
            if (m_lUserID < 0)
            {
                m_lUserID = HIK_NetSDK.NET_DVR_Login_V30(ip, port, userName, password, ref DeviceInfo);
                if (m_lUserID < 0)
                {
                    iLastErr = HIK_NetSDK.NET_DVR_GetLastError();
                    //登录失败，输出错误号 Failed to login and output the error code;
                    str = "设备地址:" + ip + " NET_DVR_Login_V30 failed, error code= " + iLastErr; 
                    DebugInfo(str);
                    return -1;
                }
                else
                {
                    m_DevIP = ip;
                    UserID = m_lUserID;
                    RuningInfo("设备地址:" + ip + " NET_DVR_Login_V30 succ! ");
                }
            }
            return m_lUserID;
        }

        public Int32 HIK_SdkLoginOut()
        {
            if (m_lUserID >= 0)
            {
                //注销登录 Logout the device
                if (m_lRealHandle >= 0)
                {
                    //登出前先停止预览 Stop live view before logout
                    DebugInfo("设备地址:" + m_DevIP + " Please stop live view firstly"); 
                    return -1;
                }

                if (!HIK_NetSDK.NET_DVR_Logout(m_lUserID))
                {
                    iLastErr = HIK_NetSDK.NET_DVR_GetLastError();
                    str = "设备地址:" + m_DevIP + " NET_DVR_Logout failed, error code= " + iLastErr;
                    DebugInfo(str);
                    return -1;
                }
                RuningInfo("设备地址:" + m_DevIP + " NET_DVR_Logout succ!");
                UserID = m_lUserID = -1;
            }
            return 0;
        }

        public Int32 HIK_SdkSetAlarm()
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
                str = "设备地址:" + m_DevIP + " 布防失败，错误号：" + iLastErr;
                HIK_SdkLoginOut();
                DebugInfo(str);
                return -1;
            }
            else
            {
                //布防成功，输出设备序列号  
                str = "设备地址:" + m_DevIP + " 布防成功，设备SN：" + DeviceInfo.sSerialNumber.ToString();
                RuningInfo(str);
                return 0;
            }
        }
        
        public Int32 HIK_SdkCloseAlarm()
        {
            if (m_lAlarmHandle >= 0)
            {
                if (!HIK_NetSDK.NET_DVR_CloseAlarmChan_V30(m_lAlarmHandle))
                {
                    iLastErr = HIK_NetSDK.NET_DVR_GetLastError();
                    //撤防失败，输出错误号
                    str = "设备地址:" + m_DevIP + " 撤防失败，错误号：" + iLastErr; 
                    DebugInfo(str);
                    return -1;
                }
                else
                {
                    str = "设备地址:" + m_DevIP + " 撤防成功，设备SN：" + DeviceInfo.sSerialNumber.ToString(); 
                    RuningInfo(str);
                    m_lAlarmHandle = -1;
                    return 0;
                }
            }
            else
            {
                str = "设备地址:" + m_DevIP + " 未布防，设备SN：" + DeviceInfo.sSerialNumber.ToString(); 
                DebugInfo(str);
                return -1;
            }
        }


        public void HIK_PlayScreen(Int32 lUserID,ref IntPtr RealPlayWnd, int channel, uint dwstreamType)
        {
            if (lUserID < 0)
            {
                str = "设备地址:" + m_DevIP + " Please login the device firstly!";
                DebugInfo(str);
                return;
            }
            if (m_lRealHandle < 0)
            {
                HIK_NetSDK.NET_DVR_PREVIEWINFO lpPreviewInfo = new HIK_NetSDK.NET_DVR_PREVIEWINFO();
                lpPreviewInfo.hPlayWnd = RealPlayWnd;//预览窗口 live view window
                lpPreviewInfo.lChannel = channel;//预览的设备通道 the device channel number
                lpPreviewInfo.dwStreamType = dwstreamType;//码流类型：0-主码流，1-子码流，2-码流3，3-码流4，以此类推
                lpPreviewInfo.dwLinkMode = 0;//连接方式：0- TCP方式，1- UDP方式，2- 多播方式，3- RTP方式，4-RTP/RTSP，5-RSTP/HTTP 
                lpPreviewInfo.bBlocked = true; //0- 非阻塞取流，1- 阻塞取流
                lpPreviewInfo.byPreviewMode = 0;
                lpPreviewInfo.byProtoType = 0;
                lpPreviewInfo.dwDisplayBufNum = 1; //播放库显示缓冲区最大帧数

                IntPtr pUser = IntPtr.Zero;
                lpPreviewInfo.hPlayWnd = IntPtr.Zero;
                m_ptrRealHandle = RealPlayWnd;
                RealData = new HIK_NetSDK.REALDATACALLBACK(RealDataCallBack);
                m_lRealHandle = HIK_NetSDK.NET_DVR_RealPlay_V40(lUserID, ref lpPreviewInfo, RealData, pUser);

                if (m_lRealHandle < 0)
                {
                    iLastErr = HIK_NetSDK.NET_DVR_GetLastError();
                    str = "设备地址:" + m_DevIP + " NET_DVR_RealPlay_V40 failed, error code= " + iLastErr;
                    DebugInfo(str);
                    return;
                }
                else
                {
                    RuningInfo("设备地址:" + m_DevIP + " NET_DVR_RealPlay_V40 succ!");
                    RealHandle = m_lRealHandle;
                }
            }
        }
        public Int32 HIK_StopScreen()
        {
            //停止预览 Stop live view 
            if (!HIK_NetSDK.NET_DVR_StopRealPlay(m_lRealHandle))
            {
                iLastErr = HIK_NetSDK.NET_DVR_GetLastError();
                str = "设备地址:" + m_DevIP + " NET_DVR_StopRealPlay failed, error code= " + iLastErr;
                DebugInfo(str);
                return -1;
            }
            if (!HIK_PlayCtrl.PlayM4_Stop(m_lPort))
            {
                iLastErr = HIK_PlayCtrl.PlayM4_GetLastError(m_lPort);
                str = "设备地址:" + m_DevIP + " PlayM4_Stop failed, error code= " + iLastErr;
                DebugInfo(str);
                return -1;
            }
            if (!HIK_PlayCtrl.PlayM4_CloseStream(m_lPort))
            {
                iLastErr = HIK_PlayCtrl.PlayM4_GetLastError(m_lPort);
                str = "设备地址:" + m_DevIP + " PlayM4_CloseStream failed, error code= " + iLastErr;
                DebugInfo(str);
                return -1;
            }
            if (!HIK_PlayCtrl.PlayM4_FreePort(m_lPort))
            {
                iLastErr = HIK_PlayCtrl.PlayM4_GetLastError(m_lPort);
                str = "设备地址:" + m_DevIP + " PlayM4_FreePort failed, error code= " + iLastErr;
                DebugInfo(str);
                return -1;
            }
            m_lPort = -1;

            RuningInfo("设备地址:" + m_DevIP + " NET_DVR_StopRealPlay succ!");
            m_lRealHandle = -1;
            return 0;
        }

        public void HIK_GetCameraSize()
        {
            //海康获取播放句柄 Get the port to play
            if (!HIK_PlayCtrl.PlayM4_GetPictureSize(m_lPort, ref pWidth, ref pHeight))
            {
                iLastErr = HIK_PlayCtrl.PlayM4_GetLastError(m_lPort);
                str = "PlayM4_GetPictureSize failed, error code= " + iLastErr;
                DebugInfo(str);
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
                            str = "设备地址:" + m_DevIP + " PlayM4_GetPort failed, error code= " + iLastErr;
                            DebugInfo(str);
                            break;
                        }

                        //设置流播放模式 Set the stream mode: real-time stream mode
                        if (!HIK_PlayCtrl.PlayM4_SetStreamOpenMode(m_lPort, HIK_PlayCtrl.STREAME_REALTIME))
                        {
                            iLastErr = HIK_PlayCtrl.PlayM4_GetLastError(m_lPort);
                            str = "设备地址:" + m_DevIP + " Set STREAME_REALTIME mode failed, error code= " + iLastErr;
                            DebugInfo(str);
                        }

                        //打开码流，送入头数据 Open stream
                        if (!HIK_PlayCtrl.PlayM4_OpenStream(m_lPort, pBuffer, dwBufSize, 10 * 1024 * 1024))
                        {
                            iLastErr = HIK_PlayCtrl.PlayM4_GetLastError(m_lPort);
                            str = "设备地址:" + m_DevIP + " PlayM4_OpenStream failed, error code= " + iLastErr;
                            DebugInfo(str);
                            break;
                        }
                        //设置显示缓冲区个数 Set the display buffer number
                        if (!HIK_PlayCtrl.PlayM4_SetDisplayBuf(m_lPort, 15))
                        {
                            iLastErr = HIK_PlayCtrl.PlayM4_GetLastError(m_lPort);
                            str = "设备地址:" + m_DevIP + " PlayM4_SetDisplayBuf failed, error code= " + iLastErr;
                        }
                        //设置显示模式 Set the display mode
                        if (!HIK_PlayCtrl.PlayM4_SetOverlayMode(m_lPort, 1, 0/* COLORREF(0)*/)) //play off screen 
                        {
                            iLastErr = HIK_PlayCtrl.PlayM4_GetLastError(m_lPort);
                            str = "设备地址:" + m_DevIP + " PlayM4_SetOverlayMode failed, error code= " + iLastErr;
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
                            str = "设备地址:" + m_DevIP + " PlayM4_Play failed, error code= " + iLastErr;
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
                                str = "设备地址:" + m_DevIP + " PlayM4_InputData failed, error code= " + iLastErr;
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
                                str = "设备地址:" + m_DevIP + " PlayM4_InputData failed, error code= " + iLastErr;
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


        private void DebugInfo(string str)
        {
            if (str.Length > 0)
            {
                Event_DebugMessage?.Invoke(str);
                //throw new ApplicationException(str);
            }
        }
        private void RuningInfo(string str)
        {
            if (str.Length > 0)
            {
                Event_RuningMessage?.Invoke(str);
                ////throw new ApplicationException(str);
            }
        }
    }
}
