using DevComponents.DotNetBar.Metro;
using SK_FVision;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace RobotVT
{
    public partial class VisualTracking : MetroForm
    {
        public event SK_FModel.SystemDelegate.del_SystemLoadFinish Event_SystemLoadFinish;

        private bool m_bRecord = false;
        private uint iLastErr = 0;
        private Int32 m_lUserID = -1;
        private Int32 m_lRealHandle = -1;
        private string str1;
        private string str2;
        private Int32 i = 0;
        private Int32 m_lTree = 0;
        private string str;
        private long iSelIndex = 0;
        private uint dwAChanTotalNum = 0;
        private uint dwDChanTotalNum = 0;
        private Int32 m_lPort = -1;
        private IntPtr m_ptrRealHandle;
        private int[] iIPDevID = new int[96];
        private int[] iChannelNum = new int[96];

        private HIK_NetSDK.REALDATACALLBACK RealData = null;
        public HIK_NetSDK.NET_DVR_DEVICEINFO_V30 DeviceInfo;
        public HIK_NetSDK.NET_DVR_IPPARACFG_V40 m_struIpParaCfgV40;
        public HIK_NetSDK.NET_DVR_STREAM_MODE m_struStreamMode;
        public HIK_NetSDK.NET_DVR_IPCHANINFO m_struChanInfo;
        public HIK_NetSDK.NET_DVR_IPCHANINFO_V40 m_struChanInfoV40;
        public HIK_NetSDK.NET_DVR_MATRIX_DECCHAN_CONTROL m_struMatrixDecchan;
        private HIK_PlayCtrl.DECCBFUN m_fDisplayFun = null;
        public delegate void MyDebugInfo(string str);
        public VisualTracking()
        {
            InitializeComponent();
            Init();
        }


        private void Init()
        {
            this.Text = RobotVT.Controller.Methods.GetApplicationTitle();
            this.Icon = Properties.Resources.ZX32x32;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Normal;

        }
        private void VisualTracking_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip.Show(this, e.Location);
            }
        }

        private void VisualTracking_Load(object sender, EventArgs e)
        {
            //RobotVT.Controller.StaticInfo.QueueMessageInfo = new Queue<SK_FModel.SystemMessageInfo>();
            //RobotVT.Controller.StaticInfo.IsSaveLogInfo = true;
            //System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(Thread_SaveLogInfo));
            //thread.IsBackground = true;
            //thread.Start();
            
            //Event_SystemLoadFinish?.Invoke();

            //Login();
            //Preview();
        }
        
        private void AddQueue(SK_FModel.SystemMessageInfo messageInfo)
        {
            RobotVT.Controller.StaticInfo.QueueMessageInfo.Enqueue(messageInfo);
        }

        private void Thread_SaveLogInfo()
        {
            while (RobotVT.Controller.StaticInfo.IsSaveLogInfo)
            {
                try
                {
                    if (RobotVT.Controller.StaticInfo.QueueMessageInfo.Count > 0)
                    {
                        //SK_FCommon.LogHelper.SaveLog(RobotVT.Controller.StaticInfo.QueueMessageInfo.Dequeue());
                    }
                }
                catch (Exception ex)
                {
                }
                System.Threading.Thread.Sleep(10);
            }
        }

        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {

            int WM_KEYDOWN = 256;

            int WM_SYSKEYDOWN = 260;

            if (msg.Msg == WM_KEYDOWN | msg.Msg == WM_SYSKEYDOWN)

            {

                switch (keyData)

                {

                    case Keys.Escape:

                        this.Close();//esc关闭窗体

                        break;

                }
            }

            return false;

        }
        
        private void Login()
        {
            if (m_lUserID < 0)
            {
                string DVRIPAddress = "192.168.6.64"; //设备IP地址或者域名 Device IP
                Int16 DVRPortNumber = Int16.Parse("8000");//设备服务端口号 Device Port
                string DVRUserName = "admin";//设备登录用户名 User name to login
                string DVRPassword = "zx123456";//设备登录密码 Password to login

                //登录设备 Login the device
                m_lUserID = HIK_NetSDK.NET_DVR_Login_V30(DVRIPAddress, DVRPortNumber, DVRUserName, DVRPassword, ref DeviceInfo);
                if (m_lUserID < 0)
                {
                    iLastErr = HIK_NetSDK.NET_DVR_GetLastError();
                    str = "NET_DVR_Login_V30 failed, error code= " + iLastErr; //登录失败，输出错误号 Failed to login and output the error code
                    DebugInfo(str);
                    return;
                }
                else
                {
                    //登录成功
                    DebugInfo("NET_DVR_Login_V30 succ!");
                }

            }
            return;
        }

        private void LoginOut()
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
                //listViewIPChannel.Items.Clear();//清空通道列表 Clean up the channel list
                m_lUserID = -1;
                //btnLogin.Text = "Login";
            }
            return;
        }

        public void DebugInfo(string str)
        {
            if (str.Length > 0)
            {
                str += "\n";
                //TextBoxInfo.AppendText(str);
            }
        }

        private void Preview()
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

            if (m_lRealHandle < 0)
            {
                HIK_NetSDK.NET_DVR_PREVIEWINFO lpPreviewInfo = new HIK_NetSDK.NET_DVR_PREVIEWINFO();
                //lpPreviewInfo.hPlayWnd = RealPlayWnd.Handle;//预览窗口 live view window
                lpPreviewInfo.lChannel = 1;//预览的设备通道 the device channel number
                lpPreviewInfo.dwStreamType = 2;//码流类型：0-主码流，1-子码流，2-码流3，3-码流4，以此类推
                lpPreviewInfo.dwLinkMode = 0;//连接方式：0- TCP方式，1- UDP方式，2- 多播方式，3- RTP方式，4-RTP/RTSP，5-RSTP/HTTP 
                lpPreviewInfo.bBlocked = false; //0- 非阻塞取流，1- 阻塞取流
                lpPreviewInfo.byPreviewMode = 0;
                lpPreviewInfo.byProtoType = 0;
                lpPreviewInfo.dwDisplayBufNum = 1; //播放库显示缓冲区最大帧数

                IntPtr pUser = IntPtr.Zero;//用户数据 user data 

                //if (comboBoxView.SelectedIndex == 0)
                //{
                //    //打开预览 Start live view 
                //    m_lRealHandle = HIK_NetSDK.NET_DVR_RealPlay_V40(m_lUserID, ref lpPreviewInfo, null/*RealData*/, pUser);
                //}
                //else
                //{
                lpPreviewInfo.hPlayWnd = IntPtr.Zero;//预览窗口 live view window
                //m_ptrRealHandle = RealPlayWnd.Handle;
                RealData = new HIK_NetSDK.REALDATACALLBACK(RealDataCallBack);//预览实时流回调函数 real-time stream callback function 
                m_lRealHandle = HIK_NetSDK.NET_DVR_RealPlay_V40(m_lUserID, ref lpPreviewInfo, RealData, pUser);
                //}

                if (m_lRealHandle < 0)
                {
                    iLastErr = HIK_NetSDK.NET_DVR_GetLastError();
                    str = "NET_DVR_RealPlay_V40 failed, error code= " + iLastErr; //预览失败，输出错误号 failed to start live view, and output the error code.
                    DebugInfo(str);
                    return;
                }
                else
                {
                    //预览成功
                    DebugInfo("NET_DVR_RealPlay_V40 succ!");
                    //btnPreview.Text = "Stop View";
                }
            }
            else
            {
                //停止预览 Stop live view 
                if (!HIK_NetSDK.NET_DVR_StopRealPlay(m_lRealHandle))
                {
                    iLastErr = HIK_NetSDK.NET_DVR_GetLastError();
                    str = "NET_DVR_StopRealPlay failed, error code= " + iLastErr;
                    DebugInfo(str);
                    return;
                }

                //if ((comboBoxView.SelectedIndex == 1) && (m_lPort >= 0))
                //{
                //    if (!HIK_PlayCtrl.PlayM4_Stop(m_lPort))
                //    {
                //        iLastErr = HIK_PlayCtrl.PlayM4_GetLastError(m_lPort);
                //        str = "PlayM4_Stop failed, error code= " + iLastErr;
                //        DebugInfo(str);
                //    }
                //    if (!HIK_PlayCtrl.PlayM4_CloseStream(m_lPort))
                //    {
                //        iLastErr = HIK_PlayCtrl.PlayM4_GetLastError(m_lPort);
                //        str = "PlayM4_CloseStream failed, error code= " + iLastErr;
                //        DebugInfo(str);
                //    }
                //    if (!HIK_PlayCtrl.PlayM4_FreePort(m_lPort))
                //    {
                //        iLastErr = HIK_PlayCtrl.PlayM4_GetLastError(m_lPort);
                //        str = "PlayM4_FreePort failed, error code= " + iLastErr;
                //        DebugInfo(str);
                //    }
                //    m_lPort = -1;
                //}

                DebugInfo("NET_DVR_StopRealPlay succ!");
                m_lRealHandle = -1;
                //btnPreview.Text = "Live View";
                //RealPlayWnd.Invalidate();//刷新窗口 refresh the window
            }
            return;
        }

        public void RealDataCallBack(Int32 lRealHandle, UInt32 dwDataType, IntPtr pBuffer, UInt32 dwBufSize, IntPtr pUser)
        {
            //下面数据处理建议使用委托的方式
            MyDebugInfo AlarmInfo = new MyDebugInfo(DebugInfo);
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
                            this.BeginInvoke(AlarmInfo, str);
                            break;
                        }

                        //设置流播放模式 Set the stream mode: real-time stream mode
                        if (!HIK_PlayCtrl.PlayM4_SetStreamOpenMode(m_lPort, HIK_PlayCtrl.STREAME_REALTIME))
                        {
                            iLastErr = HIK_PlayCtrl.PlayM4_GetLastError(m_lPort);
                            str = "Set STREAME_REALTIME mode failed, error code= " + iLastErr;
                            this.BeginInvoke(AlarmInfo, str);
                        }

                        //打开码流，送入头数据 Open stream
                        if (!HIK_PlayCtrl.PlayM4_OpenStream(m_lPort, pBuffer, dwBufSize, 2 * 1024 * 1024))
                        {
                            iLastErr = HIK_PlayCtrl.PlayM4_GetLastError(m_lPort);
                            str = "PlayM4_OpenStream failed, error code= " + iLastErr;
                            this.BeginInvoke(AlarmInfo, str);
                            break;
                        }


                        //设置显示缓冲区个数 Set the display buffer number
                        if (!HIK_PlayCtrl.PlayM4_SetDisplayBuf(m_lPort, 15))
                        {
                            iLastErr = HIK_PlayCtrl.PlayM4_GetLastError(m_lPort);
                            str = "PlayM4_SetDisplayBuf failed, error code= " + iLastErr;
                            this.BeginInvoke(AlarmInfo, str);
                        }

                        //设置显示模式 Set the display mode
                        if (!HIK_PlayCtrl.PlayM4_SetOverlayMode(m_lPort, 0, 0/* COLORREF(0)*/)) //play off screen 
                        {
                            iLastErr = HIK_PlayCtrl.PlayM4_GetLastError(m_lPort);
                            str = "PlayM4_SetOverlayMode failed, error code= " + iLastErr;
                            this.BeginInvoke(AlarmInfo, str);
                        }

                        //设置解码回调函数，获取解码后音视频原始数据 Set callback function of decoded data
                        m_fDisplayFun = new HIK_PlayCtrl.DECCBFUN(DecCallbackFUN);
                        if (!HIK_PlayCtrl.PlayM4_SetDecCallBackEx(m_lPort, m_fDisplayFun, IntPtr.Zero, 0))
                        {
                            this.BeginInvoke(AlarmInfo, "PlayM4_SetDisplayCallBack fail");
                        }

                        //开始解码 Start to play                       
                        if (!HIK_PlayCtrl.PlayM4_Play(m_lPort, m_ptrRealHandle))
                        {
                            iLastErr = HIK_PlayCtrl.PlayM4_GetLastError(m_lPort);
                            str = "PlayM4_Play failed, error code= " + iLastErr;
                            this.BeginInvoke(AlarmInfo, str);
                            break;
                        }
                    }
                    break;
                case HIK_NetSDK.NET_DVR_STREAMDATA:     // video stream data
                    if (dwBufSize > 0 && m_lPort != -1)
                    {
                        for (int i = 0; i < 999; i++)
                        {
                            //送入码流数据进行解码 Input the stream data to decode
                            if (!HIK_PlayCtrl.PlayM4_InputData(m_lPort, pBuffer, dwBufSize))
                            {
                                iLastErr = HIK_PlayCtrl.PlayM4_GetLastError(m_lPort);
                                str = "PlayM4_InputData failed, error code= " + iLastErr;
                                Thread.Sleep(2);
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
                                Thread.Sleep(2);
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
        //解码回调函数
        private void DecCallbackFUN(int nPort, IntPtr pBuf, int nSize, ref HIK_PlayCtrl.FRAME_INFO pFrameInfo, int nReserved1, int nReserved2)
        {
            // 将pBuf解码后视频输入写入文件中（解码后YUV数据量极大，尤其是高清码流，不建议在回调函数中处理）
            if (pFrameInfo.nType == 3) //#define T_YV12	3
            {
                //    FileStream fs = null;
                //    BinaryWriter bw = null;
                //    try
                //    {
                //        fs = new FileStream("DecodedVideo.yuv", FileMode.Append);
                //        bw = new BinaryWriter(fs);
                //        byte[] byteBuf = new byte[nSize];
                //        Marshal.Copy(pBuf, byteBuf, 0, nSize);
                //        bw.Write(byteBuf);
                //        bw.Flush();
                //    }
                //    catch (System.Exception ex)
                //    {
                //        MessageBox.Show(ex.ToString());
                //    }
                //    finally
                //    {
                //        bw.Close();
                //        fs.Close();
                //    }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
        }


        private void toolStripMenuItem_Set_Click(object sender, EventArgs e)
        {
            SK_FVision.HIK_CameraSet _CameraSet = new HIK_CameraSet();
            _CameraSet.ShowDialog();
        }
    }
}
