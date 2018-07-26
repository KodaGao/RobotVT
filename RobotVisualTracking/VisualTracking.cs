using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.UI;
using DevComponents.DotNetBar.Metro;
using SK_FVision;

namespace RobotVT
{
    public partial class VisualTracking : MetroForm
    {

        public event SK_FModel.SystemDelegate.del_SystemLoadFinish Event_SystemLoadFinish;

        private bool m_bInitSDK = false;
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
        //private PlayCtrl.DECCBFUN m_fDisplayFun = null;
        public delegate void MyDebugInfo(string str);
        public VisualTracking()
        {
            InitializeComponent();

            m_bInitSDK = HIK_NetSDK.NET_DVR_Init();
            if (m_bInitSDK == false)
            {
                MessageBox.Show("NET_DVR_Init error!");
                return;
            }
            else
            {
                //保存SDK日志 To save the SDK log
                HIK_NetSDK.NET_DVR_SetLogToFile(3, @"SdkLog\", true);
            }

        }

        private void VisualTracking_Load(object sender, EventArgs e)
        {
            RobotVT.Controller.StaticInfo.QueueMessageInfo = new Queue<SK_FModel.SystemMessageInfo>();
            RobotVT.Controller.StaticInfo.IsSaveLogInfo = true;
            System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(Thread_SaveLogInfo));
            thread.IsBackground = true;
            thread.Start();

            panel1.BackgroundImage = RobotVT.Resources.Properties.Resources.signal_5;

            Event_SystemLoadFinish?.Invoke();
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



    }
}
