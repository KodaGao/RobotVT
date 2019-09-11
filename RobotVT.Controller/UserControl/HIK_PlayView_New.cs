using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RobotVT.Controller
{
    public partial class HIK_PlayView_New : SK_FVision.ViedoPlayer
    {
        public delegate void PlayView_SystemMouseDoubleClick(string vtID, Int32 userID,Int32 realHandle);
        public event PlayView_SystemMouseDoubleClick Event_PlayViewMouseDoubleClick;
        public string PlayModel { get; set; }
        public bool ShowTarget { get; set; }

        public Int32 UserID
        {
            get
            {
                if (HIK_CameraMng != null)
                { 
                    return HIK_CameraMng.UserID;
                }
                else
                {
                    return -1;
                }
            }
        }
        public Int32 RealHandle { get
            {
                if (HIK_CameraMng != null)
                {
                    return HIK_CameraMng.RealHandle;
                }
                else
                {
                    return -1;
                }
            } }
        new public Int32 Width { get; set; }
        new public Int32 Height { get; set; }


        private int Hik_Channel = 1;
        private uint Hik_StreamType = 0;

        private SK_FVision.HIK_CameraMng HIK_CameraMng;
        private HIK_CameraSet hIK_CameraSet;

        public HIK_PlayView_New()
        {
            InitializeComponent();
            this.Disposed += HIK_PlayView_New_Disposed;
            if (StaticInfo.TargetFollow != null)
                StaticInfo.TargetFollow.Event_Multicast += TargetFollow_Event_Multicast;

            hIK_CameraSet = new HIK_CameraSet();
            hIK_CameraSet.Event_SetFinish += HIK_CameraSet_Event_SetFinish;

            PlayModel = "";
            ShowTarget = false;

        }

        private void HIK_PlayView_New_Disposed(object sender, EventArgs e)
        {
            HIK_CameraMng.HIK_SdkCloseAlarm();
            HIK_CameraMng.HIK_StopScreen();
            HIK_CameraMng.HIK_SdkLoginOut();

            hIK_CameraSet.Dispose();
        }

        public void InitHIKCamera()
        {
            HIK_CameraMng = new SK_FVision.HIK_CameraMng();
            HIK_CameraMng.Event_DebugMessage += HIK_CameraMng_Event_DebugMessage;
            HIK_CameraMng.Event_RuningMessage += HIK_CameraMng_Event_RuningMessage;

        }
        public void InitH264Decode()
        {
            this.ImagePlaySpan = 10;
            InitH264Thread();
        }

        public void Stop264Decode()
        {
            StopH264Thread();
        }

        public override void RealPlayWnd_MouseUp(object sender, MouseEventArgs e)
        {
            ////主窗口右键进行设备登陆设置
            //if (e.Button == MouseButtons.Right && this.PlayModel.ToLower() != StaticInfo.MainView)
            //{
            //    Model.S_D_CameraSet _cameraSet = new Controller.DataAccess().GetCameraSet(PlayModel.ToUpper());
            //    if (_cameraSet != null)
            //    {
            //        hIK_CameraSet._CameraSet = _cameraSet;
            //    }
            //    hIK_CameraSet.PlayModel = PlayModel.ToUpper();
            //    hIK_CameraSet.ShowDialog();
            //}

            //主窗口右键取消目标跟踪
            if (e.Button == MouseButtons.Right && this.PlayModel.ToLower() == StaticInfo.MainView)
            {
                Point _mousePoint = e.Location;
                base.GetPictureSize();
                StaticInfo.TargetFollow.SendingCoordinates(1, RealPlayWnd.Width, RealPlayWnd.Height, _mousePoint);
            }

            //主窗口左键单击目标跟踪
            if (!gbMove && e.Button == MouseButtons.Left && this.PlayModel.ToLower() == StaticInfo.MainView)
            {
                Point _mousePoint = e.Location;
                base.GetPictureSize();
                //GetHIKSize();
                //_mousePoint = new Point(0, pHeight / 2);

                //StaticInfo.TargetFollow.SendingCoordinates(0, pWidth, pHeight, _mousePoint);
                StaticInfo.TargetFollow.SendingCoordinates(0, RealPlayWnd.Width, RealPlayWnd.Height, _mousePoint);
            }

            if (gbMove && e.Button == MouseButtons.Left && this.PlayModel.ToLower() == StaticInfo.MainView)
            {
                PTZControl_FocusFar(0, 0, true, RealHandle, 1);
                PTZControl_FocusNear(0, 0, true, RealHandle, 1);
                PTZControl_ZoomOut(0, 0, true, RealHandle, 1);
                PTZControl_ZoomIn(0, 0, true, RealHandle, 1);
                gbMove = false;
            }
        }

        public override void RealPlayWnd_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //主窗口双击进行超脑设备登陆设置
            //其他窗口进行切换
            if (this.PlayModel.ToLower() == StaticInfo.MainView)
            {
                Model.S_D_CameraSet _cameraSet = new Controller.DataAccess().GetCameraSet(PlayModel.ToUpper());
                if (_cameraSet != null)
                {
                    hIK_CameraSet._CameraSet = _cameraSet;
                }
                hIK_CameraSet.PlayModel = PlayModel.ToUpper();
                hIK_CameraSet.ShowDialog();
            }
            else
            {
                Event_PlayViewMouseDoubleClick?.Invoke(PlayModel, HIK_CameraMng.UserID, HIK_CameraMng.RealHandle);
            }
        }

        bool gbMove = false;
        Point _LastPoint = new Point(0, 0);

        public override void RealPlayWnd_MouseDown(object sender, MouseEventArgs e)
        {
            //主窗口左键记录起始位置
            if (e.Button == MouseButtons.Left && gbMove == false && this.PlayModel.ToLower() == StaticInfo.MainView)
            {
                _LastPoint = e.Location;
            }
        }

        public override void RealPlayWnd_MouseMove(object sender, MouseEventArgs e)
        {
            //判断鼠标移动方向，进行摄像头缩放控制
            //松开鼠标后停止缩放
            if (this.PlayModel.ToLower() == StaticInfo.MainView && gbMove == false)
            {
                gbMove = true;
                NotifyMouseMove(e.Location);
            }
        }
        private void NotifyMouseMove(Point newPoint)
        {
            if (newPoint.X > _LastPoint.X)
            {
                PTZControl_FocusFar(0, 0, true, RealHandle, 0);
            }
            else
            {
               PTZControl_FocusNear(0, 0, true, RealHandle, 0);
            }

            if (newPoint.Y > _LastPoint.Y)
            {
                PTZControl_ZoomOut(0, 0, true, RealHandle, 0);
            }
            else
            {
                PTZControl_ZoomIn(0, 0, true, RealHandle, 0);
            }
        }

        private void HIK_CameraSet_Event_SetFinish()
        {
            Model.S_D_CameraSet _cameraSet = new Controller.DataAccess().GetCameraSet(PlayModel.ToUpper());
            if (_cameraSet != null)
            {
                string ip = _cameraSet.VT_IP; //设备IP地址或者域名 Device IP
                Int16 port = Int16.Parse(_cameraSet.VT_PORT);//设备服务端口号 Device Port
                string userName = _cameraSet.VT_NAME;//设备登录用户名 User name to login
                string password = _cameraSet.VT_PASSWORD;//设备登录密码 Password to login
                

                switch (PlayModel.ToLower())
                {
                    case StaticInfo.MainView:
                    case StaticInfo.CloudView:
                        int ret = HIK_CameraMng.HIK_StopScreen();
                        ret = HIK_CameraMng.HIK_SdkLoginOut();
                        ret = HIK_CameraMng.HIK_SdkLogin(ip, port, userName, password);
                        break;
                    case StaticInfo.FrontView:
                    case StaticInfo.BackView:
                    case StaticInfo.LeftView:
                    case StaticInfo.RightView:
                        ret = HIK_CameraMng.HIK_StopScreen();
                        ret = HIK_CameraMng.HIK_SdkLoginOut();

                        ret = HIK_CameraMng.HIK_SdkLogin(ip, port, userName, password);
                        var RealHandle = RealPlayWnd.Handle;
                        HIK_CameraMng.HIK_PlayScreen(HIK_CameraMng.UserID, ref RealHandle, Hik_Channel, Hik_StreamType);
                        break;
                }
            }

        }


        private void TargetFollow_Event_Multicast(List<byte> recvBuflist)
        {
            if (PlayModel == null || !(PlayModel.ToLower() == StaticInfo.MainView || PlayModel.ToLower() == StaticInfo.CloudView)) return;
            if (!ShowTarget) return;
            DecodeH264(recvBuflist.ToArray());
        }

        private void HIK_CameraMng_Event_RuningMessage(string MessageInfo)
        {
            //throw new NotImplementedException();
        }

        private void HIK_CameraMng_Event_DebugMessage(string MessageInfo)
        {
            //throw new NotImplementedException();
        }

        #region HIK
        public void LoginHIKCamera(string ip, Int16 port, string userName, string password)
        {
            int ret = HIK_CameraMng.HIK_SdkLogin(ip, port, userName, password);
            if (ret < 0)
            {

            }
        }
        public void LoginOutHIKCamera()
        {
            int ret = HIK_CameraMng.HIK_SdkLoginOut();
        }

        public void PlayHIKScreen()
        {
            var RealHandle = RealPlayWnd.Handle;
            HIK_CameraMng.HIK_PlayScreen(HIK_CameraMng.UserID,ref RealHandle, Hik_Channel, Hik_StreamType);
            RealPlayWnd.Invalidate();
        }
        public void PlayHIKScreen(Int32 UserID)
        {
            var RealHandle = RealPlayWnd.Handle;
            HIK_CameraMng.HIK_PlayScreen(UserID, ref RealHandle, Hik_Channel, Hik_StreamType);
            RealPlayWnd.Invalidate();
        }
        public void StopHIKScreen()
        {
            int ret = HIK_CameraMng.HIK_StopScreen();
            RealPlayWnd.Invalidate();
        }

        public void SetHIKAlarm()
        {
            int ret = HIK_CameraMng.HIK_SdkSetAlarm();
        }
        public void StopHIKAlarm()
        {
            int ret = HIK_CameraMng.HIK_SdkCloseAlarm();
        }

        public void GetHIKSize()
        {
            HIK_CameraMng.HIK_GetCameraSize();
            Width = HIK_CameraMng.pWidth;
            Height = HIK_CameraMng.pHeight;
        }
        #endregion
    }
}
