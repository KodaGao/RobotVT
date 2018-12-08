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
    public partial class HIK_CloudControl : UserControl
    {
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

        public int m_lUserID = -1;
        public int m_lRealHandle = -1;
        public int m_lChannel = -1;
        private bool bAuto = false;

        public HIK_CloudControl()
        {
            InitializeComponent();
            this.MouseDown += HIK_CloudControl_MouseDown;
            this.MouseUp += HIK_CloudControl_MouseUp;
        }

        private void HIK_CloudControl_Load(object sender, EventArgs e)
        {
            //对云台实施的每一个动作都需要调用该接口两次，分别是开始和停止控制，由接口中的最后一个参数（dwStop）决定。
            //在调用此接口之前需要先开启预览。与设备之间的云台各项操作的命令都对应于设备与云台之间的控制码，设备会根据目前设置的解码器种类和解码器地址向云台发送控制码。
            //如果目前设备上设置的解码器与云台设备的不匹配，需要重新配置设备的解码器。
            //如果云台设备所需的解码器设备不支持，则无法用该接口控制。
            //云台默认以最大速度动作。
        }

        private void HIK_CloudControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (m_lUserID == -1)
            {
                throw new ApplicationException("摄像机未登陆！");
            }

            if (m_lRealHandle > -1)
            {
                bAuto = true;
            }
            else
            {
                bAuto = false;
            }

            PTZControl(e.X, e.Y, 0);
        }

        private void HIK_CloudControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (m_lUserID == -1)
            {
                string str = "摄像机未登陆！";
                DebugInfo(str);   
            }
            if (m_lRealHandle > -1)
            {
                bAuto = true;
            }
            else
            {
                bAuto = false;
                string str = "摄像机预览！";
                DebugInfo(str);
            }

            PTZControl(e.X, e.Y, 1);
        }


        #region 云台控制操作(需先启动图象预览)。
        private void PTZControl(int x,int y,uint stop)
        {
            if (y >= 90)//焦距变小
            {
                PTZControl_ZoomOut(stop);
            }
            else if (y < 90 && y >= 40)//判断焦点调整
            {
                if (x < 40)//焦点前调
                {
                    PTZControl_FocusNear(stop);
                }
                else if (x > 90)//焦点后调
                {
                    PTZControl_FocusFar(stop);
                }
            }
            else if (y < 40)//焦距变大
            {
                PTZControl_ZoomIn(stop);
            }
        }
        private void PTZControl_ZoomIn(uint dwstop)
        {
            if (bAuto)
            {
                HIK_NetSDK.NET_DVR_PTZControl(m_lRealHandle, HIK_NetSDK.ZOOM_IN, dwstop);
            }
            else
            {
                HIK_NetSDK.NET_DVR_PTZControl_Other(m_lUserID, m_lChannel, HIK_NetSDK.ZOOM_IN, dwstop);
            }
        }
        private void PTZControl_ZoomOut(uint dwstop)
        {
            if (bAuto)
            {
                HIK_NetSDK.NET_DVR_PTZControl(m_lRealHandle, HIK_NetSDK.ZOOM_OUT, dwstop);
            }
            else
            {
                HIK_NetSDK.NET_DVR_PTZControl_Other(m_lUserID, m_lChannel, HIK_NetSDK.ZOOM_OUT, dwstop);
            }
        }
        private void PTZControl_FocusNear(uint dwstop)
        {
            if (bAuto)
            {
                HIK_NetSDK.NET_DVR_PTZControl(m_lRealHandle, HIK_NetSDK.FOCUS_NEAR, dwstop);
            }
            else
            {
                HIK_NetSDK.NET_DVR_PTZControl_Other(m_lUserID, m_lChannel, HIK_NetSDK.FOCUS_NEAR, dwstop);
            }
        }
        private void PTZControl_FocusFar(uint dwstop)
        {
            if (bAuto)
            {
                HIK_NetSDK.NET_DVR_PTZControl(m_lRealHandle, HIK_NetSDK.FOCUS_FAR, dwstop);
            }
            else
            {
                HIK_NetSDK.NET_DVR_PTZControl_Other(m_lUserID, m_lChannel, HIK_NetSDK.FOCUS_FAR, dwstop);
            }
        }
        #endregion

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // HIK_CloudControl
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.Name = "HIK_CloudControl";
            this.ResumeLayout(false);

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
