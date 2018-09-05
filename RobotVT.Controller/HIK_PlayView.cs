using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace RobotVT.Controller
{
    public partial class HIK_PlayView : SK_FVision.PlayView
    {
        public event SK_FModel.SystemDelegate.PlayView_SystemMouseDoubleClick Event_PlayViewMouseDoubleClick;

        new public bool MouseUp = true;

        public string PlayModel { get; set; }
        public RobotVT.Model.S_D_CameraSet _CameraSet { get; set; }

        public HIK_PlayView()
        {
            InitializeComponent();
        }

        public override void sdkCaptureJpeg(SK_FVision.HIK_NetSDK.NET_VCA_FACESNAP_RESULT struAlarm)
        {
            //报警图片保存
            if (struAlarm.dwFacePicLen > 0)
            {
                SK_FCommon.DirFile.CreateDirectory(StaticInfo.CapturePath);
                //将缓冲区里的JPEG图片数据写入文件 save the data into a file
                string str = DateTime.Now.ToString("hhMMss") + ".jpg";
                string strname = StaticInfo.CapturePath + str;
                int iLen = (int)struAlarm.dwFacePicLen;
                byte[] by = new byte[iLen];
                System.Runtime.InteropServices.Marshal.Copy(struAlarm.pBuffer1, by, 0, iLen);

                SK_FCommon.DirFile.CreateFile(strname, by, iLen);
            }
        }

        public override void sdkCaptureJpeg(SK_FVision.HIK_NetSDK.NET_DVR_FACEDETECT_ALARM struAlarm)
        {
            //报警图片保存
            if (struAlarm.dwFacePicDataLen > 0)
            {
                SK_FCommon.DirFile.CreateDirectory(StaticInfo.CapturePath);
                //将缓冲区里的JPEG图片数据写入文件 save the data into a file
                string str = DateTime.Now.ToString("hhMMss") + ".jpg";
                string strname = StaticInfo.CapturePath + str;
                //FileStream fs = new FileStream(strname, FileMode.Create);
                int iLen = (int)struAlarm.dwFacePicDataLen;
                byte[] by = new byte[iLen];
                System.Runtime.InteropServices.Marshal.Copy(struAlarm.pFaceImage, by, 0, iLen);
                SK_FCommon.DirFile.CreateFile(strname, by, iLen);

            }
        }

        public override void sdkCaptureJpeg(SK_FVision.HIK_NetSDK.NET_VCA_FACESNAP_MATCH_ALARM struAlarm)
        {
            //报警图片保存
            if (struAlarm.struSnapInfo.dwSnapFacePicLen > 0)
            {
                SK_FCommon.DirFile.CreateDirectory(StaticInfo.CapturePath);
                //将缓冲区里的JPEG图片数据写入文件 save the data into a file
                string str = DateTime.Now.ToString("hhMMss") + ".jpg";
                string strname = StaticInfo.CapturePath + str;
                //FileStream fs = new FileStream(strname, FileMode.Create);
                int iLen = (int)struAlarm.struSnapInfo.dwSnapFacePicLen;
                byte[] by = new byte[iLen];
                System.Runtime.InteropServices.Marshal.Copy(struAlarm.struSnapInfo.pBuffer1, by, 0, iLen);
                SK_FCommon.DirFile.CreateFile(strname, by, iLen);

            }
        }

        public override void PlayView_Load(object sender, EventArgs e)
        {
            base.PlayView_Load(sender, e);
        }

        public override void RealPlayWnd_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {

                if (!MouseUp && this.PlayModel == null) return;

                HIK_CameraSet hIK_CameraSet = new HIK_CameraSet();
                hIK_CameraSet.Event_SetFinish += HIK_CameraSet_Event_SetFinish;
                if (_CameraSet != null)
                {
                    hIK_CameraSet._CameraSet = _CameraSet;
                }
                hIK_CameraSet.PlayModel = this.PlayModel;
                hIK_CameraSet.ShowDialog();

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

            sdkLogin(DVRIPAddress, DVRPortNumber, DVRUserName, DVRPassword, 1, 0);
            _CameraSet = _cameraSetNew;

        }

        public override void RealPlayWnd_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (_CameraSet == null) return;

            Event_PlayViewMouseDoubleClick?.Invoke(_CameraSet.VT_ID);

            base.RealPlayWnd_MouseDoubleClick(sender, e);
        }

    }
}
