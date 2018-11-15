using DevComponents.DotNetBar.Metro;
using RobotVT.Controller;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace RobotVT
{
    public partial class VisualTracking : MetroForm
    {
        public event SK_FModel.SystemDelegate.del_SystemLoadFinish Event_SystemLoadFinish;
        private SK_FVision.HIK_NetSDK.MSGCallBack m_falarmData = null;

        public VisualTracking()
        {
            InitializeComponent();
            this.SizeChanged += new System.EventHandler(this.VisualTracking_SizeChanged);
            this.FormClosing += new FormClosingEventHandler(this.VisualTracking_FormClosing);
            this.FormClosed += new FormClosedEventHandler(VisualTracking_FormClosed);
            
            Init();
            X = this.Width;//赋值初始窗体宽度
            Y = this.Height;//赋值初始窗体高度
            setTag(this);
        }

        private void Init()
        {
            this.Text = RobotVT.Controller.Methods.GetApplicationTitle();
            this.Icon = Properties.Resources.ZX32x32;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Normal;
            
            this.mainCamera.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.mainCarmer;
            this.cloudCamera.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.CloudCarmer;
            this.frontCamera.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.frontCamer;
            this.backCamera.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.backCarmer;
            this.leftCamera.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.leftCamer;
            this.rightCamera.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.rightCamer;

            this.mainWindow2.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.mainWindow2;

            mainPlayView.MouseUp = false;
            mainPlayView.Event_PlayViewMouseDoubleClick += Event_PlayViewMouseDoubleClick;

            cloudPlayView.PlayModel = "cloud";
            cloudPlayView.Event_PlayViewMouseDoubleClick += Event_PlayViewMouseDoubleClick;

            frontPlayView.PlayModel = "front";
            frontPlayView.Event_PlayViewMouseDoubleClick += Event_PlayViewMouseDoubleClick;

            backPlayView.PlayModel = "back";
            backPlayView.Event_PlayViewMouseDoubleClick += Event_PlayViewMouseDoubleClick;

            leftPlayView.PlayModel = "left";
            leftPlayView.Event_PlayViewMouseDoubleClick += Event_PlayViewMouseDoubleClick;

            rightPlayView.PlayModel = "right";
            rightPlayView.Event_PlayViewMouseDoubleClick += Event_PlayViewMouseDoubleClick;
        }

        private void VisualTracking_Load(object sender, EventArgs e)
        {
            //RobotVT.Controller.StaticInfo.QueueMessageInfo = new Queue<SK_FModel.SystemMessageInfo>();
            //RobotVT.Controller.StaticInfo.IsSaveLogInfo = true;
            //System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(Thread_SaveLogInfo));
            //thread.IsBackground = true;
            //thread.Start();

            //设置报警回调函数
            m_falarmData = new SK_FVision.HIK_NetSDK.MSGCallBack(MsgCallback);
            SK_FVision.HIK_NetSDK.NET_DVR_SetDVRMessageCallBack_V30(m_falarmData, IntPtr.Zero);

            SK_FCommon.DirFile.CreateDirectory(StaticInfo.CapturePath);

            Event_SystemLoadFinish?.Invoke();
            LoginAllDev();

            RobotVT.Controller.StaticInfo.IsLoadCaputeImage = true;
            System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(Thread_LoadCapture));
            thread.IsBackground = true;
            thread.Start();

            //mainPlayView.sdkSetAlarm();
            
            //byte[] strIP = new byte[16 * 16];
            //uint dwValidNum = 0;
            //Boolean bEnableBind = false;
            //string strListenIP="";
            ////获取本地PC网卡IP信息
            //if (SK_FVision.HIK_NetSDK.NET_DVR_GetLocalIP(strIP, ref dwValidNum, ref bEnableBind))
            //{
            //    if (dwValidNum > 0)
            //    {
            //        //取第一张网卡的IP地址为默认监听端口
            //        strListenIP = System.Text.Encoding.UTF8.GetString(strIP, 0, 16);
            //    }

            //}

            //int iListenHandle = SK_FVision.HIK_NetSDK.NET_DVR_StartListen_V30(strListenIP, 7300, m_falarmData, IntPtr.Zero);
            //if (iListenHandle < 0)
            //{
            //    uint iLastErr = SK_FVision.HIK_NetSDK.NET_DVR_GetLastError();
            //    string strErr = "启动监听失败，错误号：" + iLastErr; //撤防失败，输出错误号
            //    MessageBox.Show(strErr);
            //}
            //else
            //{
            //    MessageBox.Show("成功启动监听！");
            //}
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
                        this.Close();
                        break;
                    case Keys.Tab:
                        LeftOrRight();
                        break;
                }
            }
            return false;
        }

        #region 报警回调


        private bool m_bRecord = false;
        private uint iLastErr = 0;
        private Int32 m_lUserID = -1;
        private Int32 m_lRealHandle = -1;
        private string str1;
        private string str2;
        private Int32 i = 0;
        private Int32 m_lTree = 0;
        private string str;
        private uint dwAChanTotalNum = 0;
        private uint dwDChanTotalNum = 0;
        private Int32 m_lPort = -1;
        private IntPtr m_ptrRealHandle;
        private int[] iIPDevID = new int[96];
        private int[] iChannelNum = new int[96];
        public SK_FVision.HIK_NetSDK.NET_DVR_DEVICEINFO_V30 DeviceInfo;
        public SK_FVision.HIK_NetSDK.NET_DVR_IPPARACFG_V40 m_struIpParaCfgV40;
        public SK_FVision.HIK_NetSDK.NET_DVR_STREAM_MODE m_struStreamMode;
        public SK_FVision.HIK_NetSDK.NET_DVR_IPCHANINFO m_struChanInfo;
        public SK_FVision.HIK_NetSDK.NET_DVR_IPCHANINFO_V40 m_struChanInfoV40;


        public void InfoIPChannel()
        {
            uint dwSize = (uint)Marshal.SizeOf(m_struIpParaCfgV40);

            IntPtr ptrIpParaCfgV40 = Marshal.AllocHGlobal((Int32)dwSize);
            Marshal.StructureToPtr(m_struIpParaCfgV40, ptrIpParaCfgV40, false);

            uint dwReturn = 0;
            int iGroupNo = 0;  //该Demo仅获取第一组64个通道，如果设备IP通道大于64路，需要按组号0~i多次调用NET_DVR_GET_IPPARACFG_V40获取

            if (!SK_FVision.HIK_NetSDK.NET_DVR_GetDVRConfig(m_lUserID, SK_FVision.HIK_NetSDK.NET_DVR_GET_IPPARACFG_V40, iGroupNo, ptrIpParaCfgV40, dwSize, ref dwReturn))
            {
                iLastErr = SK_FVision.HIK_NetSDK.NET_DVR_GetLastError();
                str = "NET_DVR_GET_IPPARACFG_V40 failed, error code= " + iLastErr;
            }
            else
            {
                m_struIpParaCfgV40 = (SK_FVision.HIK_NetSDK.NET_DVR_IPPARACFG_V40)Marshal.PtrToStructure(ptrIpParaCfgV40, typeof(SK_FVision.HIK_NetSDK.NET_DVR_IPPARACFG_V40));

                for (int i = 0; i < dwAChanTotalNum; i++)
                {
                    ListAnalogChannel(i + 1, m_struIpParaCfgV40.byAnalogChanEnable[i]);
                    iChannelNum[i] = i + (int)DeviceInfo.byStartChan;
                }

                byte byStreamType = 0;
                uint iDChanNum = 64;

                if (dwDChanTotalNum < 64)
                {
                    iDChanNum = dwDChanTotalNum; //如果设备IP通道小于64路，按实际路数获取
                }

                for (int i = 0; i < iDChanNum; i++)
                {
                    iChannelNum[i + dwAChanTotalNum] = i + (int)m_struIpParaCfgV40.dwStartDChan;
                    byStreamType = m_struIpParaCfgV40.struStreamMode[i].byGetStreamType;

                    dwSize = (uint)Marshal.SizeOf(m_struIpParaCfgV40.struStreamMode[i].uGetStream);
                    switch (byStreamType)
                    {
                        //目前NVR仅支持直接从设备取流 NVR supports only the mode: get stream from device directly
                        case 0:
                            IntPtr ptrChanInfo = Marshal.AllocHGlobal((Int32)dwSize);
                            Marshal.StructureToPtr(m_struIpParaCfgV40.struStreamMode[i].uGetStream, ptrChanInfo, false);
                            m_struChanInfo = (SK_FVision.HIK_NetSDK.NET_DVR_IPCHANINFO)Marshal.PtrToStructure(ptrChanInfo, typeof(SK_FVision.HIK_NetSDK.NET_DVR_IPCHANINFO));

                            //列出IP通道 List the IP channel
                            ListIPChannel(i + 1, m_struChanInfo.byEnable, m_struChanInfo.byIPID);
                            if (m_struChanInfo.byEnable != 0)
                            {
                                if (i == 1)
                                {
                                    mainPlayView.playScreen(m_lUserID, m_bRecord, m_lRealHandle, iChannelNum[i]);
                                    mainPlayView.SetAlarm(m_lUserID);
                                }
                                if (i == 0)
                                { 
                                    cloudPlayView.playScreen(m_lUserID, m_bRecord, m_lRealHandle, iChannelNum[i]);
                                    cloudPlayView.SetAlarm(m_lUserID);
                                }
                            }

                            iIPDevID[i] = m_struChanInfo.byIPID + m_struChanInfo.byIPIDHigh * 256 - iGroupNo * 64 - 1;

                            Marshal.FreeHGlobal(ptrChanInfo);
                            break;
                        case 6:
                            IntPtr ptrChanInfoV40 = Marshal.AllocHGlobal((Int32)dwSize);
                            Marshal.StructureToPtr(m_struIpParaCfgV40.struStreamMode[i].uGetStream, ptrChanInfoV40, false);
                            m_struChanInfoV40 = (SK_FVision.HIK_NetSDK.NET_DVR_IPCHANINFO_V40)Marshal.PtrToStructure(ptrChanInfoV40, typeof(SK_FVision.HIK_NetSDK.NET_DVR_IPCHANINFO_V40));

                            //列出IP通道 List the IP channel
                            ListIPChannel(i + 1, m_struChanInfoV40.byEnable, m_struChanInfoV40.wIPID);
                            iIPDevID[i] = m_struChanInfoV40.wIPID - iGroupNo * 64 - 1;

                            Marshal.FreeHGlobal(ptrChanInfoV40);
                            break;
                        default:
                            break;
                    }
                }
            }
            Marshal.FreeHGlobal(ptrIpParaCfgV40);

        }
        public void ListIPChannel(Int32 iChanNo, byte byOnline, int byIPID)
        {
            str1 = String.Format("IPCamera {0}", iChanNo);
            m_lTree++;

            if (byIPID == 0)
            {
                str2 = "X"; //通道空闲，没有添加前端设备 the channel is idle                  
            }
            else
            {
                if (byOnline == 0)
                {
                    str2 = "offline"; //通道不在线 the channel is off-line
                }
                else
                    str2 = "online"; //通道在线 The channel is on-line
            }

            //listViewIPChannel.Items.Add(new ListViewItem(new string[] { str1, str2 }));//将通道添加到列表中 add the channel to the list
        }
        public void ListAnalogChannel(Int32 iChanNo, byte byEnable)
        {
            str1 = String.Format("Camera {0}", iChanNo);
            m_lTree++;

            if (byEnable == 0)
            {
                str2 = "Disabled"; //通道已被禁用 This channel has been disabled               
            }
            else
            {
                str2 = "Enabled"; //通道处于启用状态 This channel has been enabled
            }

            //listViewIPChannel.Items.Add(new ListViewItem(new string[] { str1, str2 }));//将通道添加到列表中 add the channel to the list
        }
        

        private void LoginAllDev()//从数据库中取出所有信息,登陆设备
        {

            for (int i = 0; i < 64; i++)
            {
                iIPDevID[i] = -1;
                iChannelNum[i] = -1;
            }
            //登录设备 Login the device
            m_lUserID = SK_FVision.HIK_NetSDK.NET_DVR_Login_V30("192.168.6.69", 8000, "admin", "zx123456", ref DeviceInfo);
            if (m_lUserID < 0)
            {
                iLastErr = SK_FVision.HIK_NetSDK.NET_DVR_GetLastError();
                str = "NET_DVR_Login_V30 failed, error code= " + iLastErr; //登录失败，输出错误号 Failed to login and output the error code

                return;
            }
            else
            {
                //登录成功
                dwAChanTotalNum = (uint)DeviceInfo.byChanNum;
                dwDChanTotalNum = (uint)DeviceInfo.byIPChanNum + 256 * (uint)DeviceInfo.byHighDChanNum;
                if (dwDChanTotalNum > 0)
                {
                    InfoIPChannel();
                }
                else
                {
                    for (int i = 0; i < dwAChanTotalNum; i++)
                    {
                        ListAnalogChannel(i + 1, 1);
                        iChannelNum[i] = i + (int)DeviceInfo.byStartChan;
                    }

                }
            }



            //List<RobotVT.Model.S_D_CameraSet> _CameraSets = new Controller.DataAccess().GetS_D_CameraSetList(0);

            //if (_CameraSets.Count <= 0)
            //{

            //}
            //else
            //{
            //    foreach (Model.S_D_CameraSet o in _CameraSets)
            //    {
            //        string DVRIPAddress = o.VT_IP; //设备IP地址或者域名 Device IP
            //        Int16 DVRPortNumber = Int16.Parse(o.VT_PORT);//设备服务端口号 Device Port
            //        string DVRUserName = o.VT_NAME;//设备登录用户名 User name to login
            //        string DVRPassword = o.VT_PASSWORD;//设备登录密码 Password to login

            //        if (o.VT_ID.ToLower() == "cloud")
            //        {

            //            cloudPlayView._CameraSet = o;
            //            //mainPlayView.sdkLogin("192.168.6.65", 8000, "admin", "zx123456", 1, 0);
            //            mainPlayView.sdkLogin(DVRIPAddress, DVRPortNumber, DVRUserName, DVRPassword, 1, 0);


            //            //cloudPlayView.sdkLogin(DVRIPAddress, DVRPortNumber, DVRUserName, DVRPassword, 1, 0);
            //            //cloudPlayView.sdkSetAlarm();

            //        }
            //        //if (o.VT_ID.ToLower() == "front")
            //        //{
            //        //    frontPlayView._CameraSet = o;
            //        //    frontPlayView.sdkLogin(DVRIPAddress, DVRPortNumber, DVRUserName, DVRPassword, 1, 0);
            //        //}
            //        //if (o.VT_ID.ToLower() == "back")
            //        //{
            //        //    backPlayView._CameraSet = o;
            //        //    backPlayView.sdkLogin(DVRIPAddress, DVRPortNumber, DVRUserName, DVRPassword, 1, 0);
            //        //}
            //        //if (o.VT_ID.ToLower() == "left")
            //        //{
            //        //    leftPlayView._CameraSet = o;
            //        //    leftPlayView.sdkLogin(DVRIPAddress, DVRPortNumber, DVRUserName, DVRPassword, 1, 0);
            //        //}
            //        //if (o.VT_ID.ToLower() == "right")
            //        //{
            //        //    rightPlayView._CameraSet = o;
            //        //    rightPlayView.sdkLogin(DVRIPAddress, DVRPortNumber, DVRUserName, DVRPassword, 1, 0);
            //        //}
            //    }
            //}



        }

        private void LoginOutAll()
        {
            //mainPlayView.sdkCloseAlarm();
            //cloudPlayView.sdkCloseAlarm();
            //mainPlayView.sdkLoginOut();
            //cloudPlayView.sdkLoginOut();
            //frontPlayView.sdkLoginOut();
            //backPlayView.sdkLoginOut();
            //leftPlayView.sdkLoginOut();
            //rightPlayView.sdkLoginOut();
        }
        
        private void LoginCloseAlarm(int m_lUserID)
        {

        }


        /// <summary>
        /// 数据处理委托
        /// </summary>
        private delegate void DealVideoDelegate(int lCommand, ref SK_FVision.HIK_NetSDK.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser);
        private void MsgCallback(int lCommand, ref SK_FVision.HIK_NetSDK.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser)
        {
            //视频数据使用委托方式处理，否则会出现内存回收异常
            DealVideoDelegate videoDlegate = new DealVideoDelegate(dealAlarm);
            videoDlegate(lCommand, ref pAlarmer, pAlarmInfo, dwBufLen, pUser);
        }


        /// <summary>
        /// 视频数据处理
        /// </summary>
        private void dealAlarm(int lCommand, ref SK_FVision.HIK_NetSDK.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser)
        {
            string ste = "";
            //通过lCommand来判断接收到的报警信息类型，不同的lCommand对应不同的pAlarmInfo内容
            switch (lCommand)
            {
                case SK_FVision.HIK_NetSDK.COMM_ALARM_FACE_DETECTION:
                    ste = "人脸侦测报警信息 " + lCommand.ToString();

                    ProcessCommAlarm_FaceDetect(ref pAlarmer, pAlarmInfo, dwBufLen, pUser);
                    break;
                case SK_FVision.HIK_NetSDK.COMM_UPLOAD_FACESNAP_RESULT:
                    ste = "人脸抓拍报警信息 " + lCommand.ToString();
                    ProcessCommAlarm_FaceSNAP(ref pAlarmer, pAlarmInfo, dwBufLen, pUser);
                    break;
                case SK_FVision.HIK_NetSDK.COMM_SNAP_MATCH_ALARM:
                    ste = "人脸比对结果信息 " + lCommand.ToString();
                    ProcessCommAlarm_SNAPMatch(ref pAlarmer, pAlarmInfo, dwBufLen, pUser);
                    break;

                default:
                    break;
            }
        }

        private void ProcessCommAlarm_FaceDetect(ref SK_FVision.HIK_NetSDK.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser)
        {

            SK_FVision.HIK_NetSDK.NET_DVR_FACEDETECT_ALARM struFaceDetectionAlarm = new SK_FVision.HIK_NetSDK.NET_DVR_FACEDETECT_ALARM();

            struFaceDetectionAlarm = (SK_FVision.HIK_NetSDK.NET_DVR_FACEDETECT_ALARM)Marshal.PtrToStructure(pAlarmInfo, typeof(SK_FVision.HIK_NetSDK.NET_DVR_FACEDETECT_ALARM));

            //保存抓拍场景图片
            if (struFaceDetectionAlarm.dwFacePicDataLen > 0 && struFaceDetectionAlarm.pFaceImage != null)
            {
                mainPlayView.sdkCaptureJpeg(struFaceDetectionAlarm);
            }

            //switch (struAlarmFACEDETECT.byAlarmPicType)// 0 - 异常人脸报警图片 1 - 人脸图片,2 - 多张人脸
            //{
            //    case 0:
            //        break;
            //    case 1:
            //        break;
            //    case 2:
            //        break;
            //    default:
            //        stringAlarm = "其他未知报警信息";
            //        break;
            //}
        }

        private void ProcessCommAlarm_FaceSNAP(ref SK_FVision.HIK_NetSDK.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser)
        {
            SK_FVision.HIK_NetSDK.NET_VCA_FACESNAP_RESULT struFaceSnap = new SK_FVision.HIK_NetSDK.NET_VCA_FACESNAP_RESULT();

            struFaceSnap = (SK_FVision.HIK_NetSDK.NET_VCA_FACESNAP_RESULT)Marshal.PtrToStructure(pAlarmInfo, typeof(SK_FVision.HIK_NetSDK.NET_VCA_FACESNAP_RESULT));

            if (struFaceSnap.dwBackgroundPicLen > 0 && struFaceSnap.pBuffer2 != null)
            {
                mainPlayView.sdkCaptureJpeg(struFaceSnap);
            }
        }

        private void ProcessCommAlarm_SNAPMatch(ref SK_FVision.HIK_NetSDK.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser)
        {
            SK_FVision.HIK_NetSDK.NET_VCA_FACESNAP_MATCH_ALARM struFaceMatchAlarm = new SK_FVision.HIK_NetSDK.NET_VCA_FACESNAP_MATCH_ALARM();

            struFaceMatchAlarm = (SK_FVision.HIK_NetSDK.NET_VCA_FACESNAP_MATCH_ALARM)Marshal.PtrToStructure(pAlarmInfo, typeof(SK_FVision.HIK_NetSDK.NET_VCA_FACESNAP_MATCH_ALARM));

            if (struFaceMatchAlarm.fSimilarity > 0 && struFaceMatchAlarm.pSnapPicBuffer != null && struFaceMatchAlarm.byPicTransType == 0)
            {
                //mainPlayView.sdkCaptureJpeg(struFaceMatchAlarm);
                try
                { 
                string str = DateTime.Now.ToString("HHmmss") + ".jpg";
                string strname = StaticInfo.CapturePath + str;

                FileStream fs = new FileStream(strname, FileMode.Create);
                int iLen = (int)struFaceMatchAlarm.dwModelDataLen;
                byte[] by = new byte[iLen];
                Marshal.Copy(struFaceMatchAlarm.pModelDataBuffer, by, 0, iLen);
                fs.Write(by, 0, iLen);
                fs.Close();

                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                //int iLen = (int)struFaceMatchAlarm.struSnapInfo.dwSnapFacePicLen;
                //byte[] by = new byte[iLen];
                //System.Runtime.InteropServices.Marshal.Copy(struFaceMatchAlarm.struSnapInfo.pBuffer1, by, 0, iLen);
                //SK_FCommon.DirFile.CreateFile(strname, by, iLen);
            }
            else
            {
                //保存黑名单人脸图片
                if (struFaceMatchAlarm.struBlackListInfo.dwBlackListPicLen > 0 && struFaceMatchAlarm.struBlackListInfo.pBuffer1 != null)
                {

                }
            }
        }


        #endregion

        #region 
        private void VisualTracking_FormClosed(object sender, FormClosedEventArgs e)
        {
            LoginOutAll();
            Dispose();
            System.Environment.Exit(0);
        }

        private void VisualTracking_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("是否退出系统", "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result == DialogResult.OK)
            {
                e.Cancel = false;  //点击OK   
            }
            else
            {
                e.Cancel = true;
            }
        }
        #endregion

        #region UI优化
        float X, Y;//X表示窗体的宽度，Y表示窗体的高度
        bool clickb = false;

        private void VisualTracking_SizeChanged(object sender, EventArgs e)
        {
            if (centerMain.Location.X < 0)
            {
                clickb = true;
            }
            float newX = this.Width / X;//获取当前宽度与初始宽度的比例
            float newY = this.Height / Y;//获取当前高度与初始高度的比例
            if ((double)Width / (double)Height > X / Y)
            {
                newX = newY;
            }
            else
            {
                newY = newX;
            }
            setControls(newX, newY, this);
            if ((double)Width / (double)Height > X / Y)
            {
                var point = zX_RobotInfo1.Location;
                point.X = (Width - zX_RobotInfo1.Width) / 2;
                zX_RobotInfo1.Location = point;
                point = centerMain.Location;
                point.X = (Width - zX_RobotInfo1.Width) / 2;
                centerMain.Location = point;
                //point = cloudCenterContorl.Location;
                //point.X = (Width - topMain.Width) / 2;
                //cloudCenterContorl.Location = point;
            }
            if (clickb)
            {
                clickb = false;
                LeftOrRight();
            }
        }
        /// <summary>
        /// 获取控件的width、height、left、top、字体大小的值
        /// </summary>
        /// <param name="cons">要获取信息的控件</param>
        private void setTag(Control cons)
        {//遍历窗体中的控件
            foreach (Control con in cons.Controls)
            {
                con.Tag = con.Width + ":" + con.Height + ":" + con.Left + ":" + con.Top + ":" + con.Font.Size;
                if (con.Controls.Count > 0)
                {
                    setTag(con);
                }
            }
        }
        /// <summary>
        /// 根据窗体大小调整控件大小
        /// </summary>
        /// <param name="newX"></param>
        /// <param name="newY"></param>
        /// <param name="cons"></param>
        private void setControls(float newX, float newY, Control cons)
        {
            //遍历窗体中的控件，重新设置控件的值
            foreach (Control con in cons.Controls)
            {
                string[] mytag = con.Tag.ToString().Split(new char[] { ':' });//获取控件的Tag属性值，并分割后存储字符串数组

                float a = Convert.ToSingle(mytag[0]) * newX;//根据窗体缩放比例确定控件的值，宽度//89*300
                con.Width = (int)(a);//宽度

                a = Convert.ToSingle(mytag[1]) * newY;//根据窗体缩放比例确定控件的值，高度//12*300
                con.Height = (int)(a);//高度

                a = Convert.ToSingle(mytag[2]) * newX;//根据窗体缩放比例确定控件的值，左边距离//
                con.Left = (int)(a);//左边距离

                a = Convert.ToSingle(mytag[3]) * newY;//根据窗体缩放比例确定控件的值，上边缘距离
                con.Top = (int)(a);//上边缘距离

                Single currentSize = Convert.ToSingle(mytag[4]) * newY;//根据窗体缩放比例确定控件的值，字体大小
                con.Font = new Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);//字体大小

                if (con.Controls.Count > 0)
                {
                    setControls(newX, newY, con);
                }

                //Remarks：
                //控件当前宽度：控件初始宽度=窗体当前宽度：窗体初始宽度
                //控件当前宽度=控件初始宽度*(窗体当前宽度/窗体初始宽度)
            }
        }
        private void LeftOrRight()
        {
            var mw1 = mainWindow.Location;
            if (mw1.X == 0)
            {
                mw1.X = mainWindow.Location.X - centerMain.Width;
                mainWindow.Location = mw1;
                mw1 = mainWindow2.Location;
                mw1.X = mainWindow2.Location.X - centerMain.Width;
                mainWindow2.Location = mw1;
            }
            else
            {
                mw1.X = mainWindow.Location.X + centerMain.Width;
                mainWindow.Location = mw1;
                mw1 = mainWindow2.Location;
                mw1.X = mainWindow2.Location.X + centerMain.Width;
                mainWindow2.Location = mw1;
            }
            //var mw1 = mainWindow.Location;
            //if (mw1.X > 0)
            //{
            //    mw1.X = mainWindow.Location.X - centerMain.Width;
            //    mainWindow.Location = mw1;
            //    mw1 = mainWindow2.Location;
            //    mw1.X = mainWindow2.Location.X + mainWindow2.Width;
            //    mainWindow2.Location = mw1;
            //}
            //else
            //{
            //    mw1.X = mainWindow.Location.X + centerMain.Width;
            //    mainWindow.Location = mw1;
            //    mw1 = mainWindow2.Location;
            //    mw1.X = mainWindow2.Location.X - mainWindow2.Width;
            //    mainWindow2.Location = mw1;
            //}
        }
        #endregion
        private void Thread_LoadCapture()
        {
            int inum = -1;
            string newfilename = "";
            while (RobotVT.Controller.StaticInfo.IsLoadCaputeImage)
            {
                try
                {
                    SK_FCommon.FileTimeInfo latest = SK_FCommon.DirFile.GetLatestFileTimeInfo(StaticInfo.CapturePath, ".jpg");

                    if (latest != null && newfilename != latest.FileName)
                    {
                        //if (pictureA1.Image == null && inum == -1)
                        //{
                        //    pictureA1.Load(latest.FileName);
                        //    inum = 1;
                        //}
                        switch (inum)
                        {
                            case 1:
                                //pictureA1.Load(latest.FileName);
                                inum++;
                                break;
                            case 2:
                                //pictureA2.Load(latest.FileName);
                                inum++;
                                break;
                            case 3:
                                //pictureA3.Load(latest.FileName);
                                inum++;
                                break;
                            case 4:
                                //pictureA4.Load(latest.FileName);
                                inum++;
                                break;
                            case 5:
                                //pictureA5.Load(latest.FileName);
                                inum++;
                                break;
                            case 6:
                                //pictureA6.Load(latest.FileName);
                                inum++;
                                break;
                            case 7:
                                //pictureA7.Load(latest.FileName);
                                inum++;
                                break;
                            case 8:
                                //pictureA8.Load(latest.FileName);
                                inum++;
                                break;
                        }
                        newfilename = latest.FileName;
                    }
                }
                catch (Exception ex)
                {
                    //throw new Exception("加载人脸比对图片失败，错误信息：" + ex.Message);
                }
                finally
                {
                    if (inum == 9)
                        inum = 1;
                    Thread.Sleep(100);
                }
            }
        }


        private void Thread_SaveLogInfo()
        {
            while (RobotVT.Controller.StaticInfo.IsSaveLogInfo)
            {
                try
                {
                    if (RobotVT.Controller.StaticInfo.QueueMessageInfo.Count > 0)
                    {
                        Methods.SaveExceptionLog(SK_FModel.SystemEnum.LogType.Runing, RobotVT.Controller.StaticInfo.QueueMessageInfo.Dequeue());
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("保存线程日志失败，错误信息：" + ex.Message);
                }
                System.Threading.Thread.Sleep(10);
            }
        }
        
        private void Event_PlayViewMouseDoubleClick(string vtid)
        {
            if (vtid == "cloud")
            {
                mainPlayView.sdkCloseAlarm();
                cloudPlayView.sdkCloseAlarm();
            }

            mainPlayView.sdkLoginOut();
            Thread.Sleep(10);
            Model.S_D_CameraSet _cameraSetNew = new Controller.DataAccess().GetCameraSet(vtid);

            string DVRIPAddress = _cameraSetNew.VT_IP; //设备IP地址或者域名 Device IP
            Int16 DVRPortNumber = Int16.Parse(_cameraSetNew.VT_PORT);//设备服务端口号 Device Port
            string DVRUserName = _cameraSetNew.VT_NAME;//设备登录用户名 User name to login
            string DVRPassword = _cameraSetNew.VT_PASSWORD;//设备登录密码 Password to login

            mainPlayView.sdkLogin(DVRIPAddress, DVRPortNumber, DVRUserName, DVRPassword, 1, 0);
        }


    }
}
 