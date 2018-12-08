using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace RobotVT.Controller
{
    public partial class HIK_PlayView : SK_FVision.PlayView
    { 
        public delegate void PlayView_SystemMouseDoubleClick(string vtID,Int32 userID);
        public event PlayView_SystemMouseDoubleClick Event_PlayViewMouseDoubleClick;
        public Int32 m_lUserID = -1;

        private HIK_CameraSet hIK_CameraSet;
        public bool ShowTarget = false;


        public string PlayModel { get; set; }
        public RobotVT.Model.S_D_CameraSet _CameraSet { get; set; }

        public HIK_PlayView()
        {
            InitializeComponent();
            StaticInfo.TargetFollow.Event_Multicast += TargetFollow_Event_Multicast;
            PlayModel = "ind";
        }

        private void TargetFollow_Event_Multicast(List<byte> recvBuflist)
        {
            if (PlayModel == null || !(PlayModel.ToLower() == StaticInfo.MainView || PlayModel.ToLower() == StaticInfo.CloudView)) return;
            if (!ShowTarget) return;
            PlayRealScreen(-1, recvBuflist.ToArray());
        }

        public override void PlayView_Load(object sender, EventArgs e)
        {
            base.PlayView_Load(sender, e);

            hIK_CameraSet = new HIK_CameraSet();
            hIK_CameraSet.Event_SetFinish += HIK_CameraSet_Event_SetFinish;
        }

        public override void RealPlayWnd_MouseMove(object sender, MouseEventArgs e)
        {
            ////if (PlayModel == null || PlayModel.ToLower() != StaticInfo.MainView) return;
            ////base.GetPictureSize();
            ////Point _mousePoint = e.Location;
            ////StaticInfo.TargetFollow.SendingCoordinates(2, pWidth, pHeight, _mousePoint);
        }

        public override void RealPlayWnd_MouseUp(object sender, MouseEventArgs e)
        {
            //右键进行设备登陆设置
            if (e.Button == MouseButtons.Right && this.PlayModel.ToLower() != StaticInfo.CloudView)
            {
                //HIK_CameraSet hIK_CameraSet = new HIK_CameraSet();
                //hIK_CameraSet.Event_SetFinish += HIK_CameraSet_Event_SetFinish;
                if (_CameraSet != null)
                {
                    hIK_CameraSet._CameraSet = _CameraSet;
                }
                hIK_CameraSet.PlayModel = this.PlayModel;
                hIK_CameraSet.ShowDialog();
            }

            //主窗口左键单击目标跟踪
            if (e.Button == MouseButtons.Left && this.PlayModel.ToLower() == StaticInfo.MainView)
            {
                Point _mousePoint = e.Location;
                base.GetPictureSize();
                StaticInfo.TargetFollow.SendingCoordinates(0, pWidth, pHeight, _mousePoint);
            }
            base.RealPlayWnd_MouseUp(sender, e);
        }
        
        private void HIK_CameraSet_Event_SetFinish()
        {
            sdkLoginOut();
            
            Model.S_D_CameraSet _cameraSetNew = new Controller.DataAccess().GetCameraSet(_CameraSet.VT_ID);

            string DVRIPAddress = _cameraSetNew.VT_IP; //设备IP地址或者域名 Device IP
            Int16 DVRPortNumber = Int16.Parse(_cameraSetNew.VT_PORT);//设备服务端口号 Device Port
            string DVRUserName = _cameraSetNew.VT_NAME;//设备登录用户名 User name to login
            string DVRPassword = _cameraSetNew.VT_PASSWORD;//设备登录密码 Password to login

            m_lUserID = sdkLogin(DVRIPAddress, DVRPortNumber, DVRUserName, DVRPassword, 1, 0);
            this.PlayRealScreen(m_lUserID, null);
            _CameraSet = _cameraSetNew;

        }

        public override void RealPlayWnd_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.PlayModel.ToLower() == StaticInfo.CloudView)
            {
                Event_PlayViewMouseDoubleClick?.Invoke(StaticInfo.CloudView, m_lUserID);
            }
            if (_CameraSet == null) return;

            //双击取消目标跟踪
            if (this.PlayModel.ToLower() == StaticInfo.MainView)
            {
                Point _mousePoint = e.Location;
                base.GetPictureSize();
                StaticInfo.TargetFollow.SendingCoordinates(1, pWidth, pHeight, _mousePoint);
            }
            else
            {
                Event_PlayViewMouseDoubleClick?.Invoke(_CameraSet.VT_ID, m_lUserID);
            }
            base.RealPlayWnd_MouseDoubleClick(sender, e);
        }
    }
}
