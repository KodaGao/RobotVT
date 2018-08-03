using DevComponents.DotNetBar.Metro;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace RobotVT
{
    public partial class VisualTracking : MetroForm
    {
        public event SK_FModel.SystemDelegate.del_SystemLoadFinish Event_SystemLoadFinish;

        public VisualTracking()
        {
            InitializeComponent();
            this.SizeChanged += new System.EventHandler(this.VisualTracking_SizeChanged);
            this.FormClosing += new FormClosingEventHandler(this.VisualTracking_FormClosing);
            this.FormClosed += new FormClosedEventHandler(VisualTracking_FormClosed);
            Init();
        }

        private void Init()
        {
            this.Text = RobotVT.Controller.Methods.GetApplicationTitle();
            this.Icon = Properties.Resources.ZX32x32;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Normal;

            this.topMain.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.top_img;
            this.signal.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.signal_5;
            this.power1.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.power1_3;
            this.power2.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.power2_2;
            this.robotPower.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.power3_3;
            this.lamp.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.lamp;

            this.mainCamera.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.mainCarmer;
            this.cloudCamera.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.CloudCarmer;
            this.frontCamera.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.frontCamer;
            this.backCamera.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.backCarmer;
            this.leftCamera.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.leftCamer;
            this.rightCamera.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.rightCamer;

            this.mainWindow2.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.mainWindow2;
            this.CompareBox1.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.comparebg;
            this.CompareTextPanel1.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.greenBg;
            this.CompareBox2.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.comparebg;
            this.CompareTextPanel2.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.greenBg;
            this.CompareBox3.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.comparebg;
            this.CompareTextPanel3.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.greenBg;
            this.CompareBox4.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.comparebg;
            this.CompareTextPanel4.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.greenBg;
            this.CompareBox5.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.comparebg;
            this.CompareTextPanel5.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.greenBg;
            this.CompareBox6.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.comparebg;
            this.CompareTextPanel6.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.greenBg;
            this.CompareBox7.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.comparebg;
            this.CompareTextPanel7.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.greenBg;
            this.CompareBox8.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.comparebg;
            this.CompareTextPanel8.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.greenBg;

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
            X = this.Width;//赋值初始窗体宽度
            Y = this.Height;//赋值初始窗体高度
            setTag(this);

            RobotVT.Controller.StaticInfo.QueueMessageInfo = new Queue<SK_FModel.SystemMessageInfo>();
            RobotVT.Controller.StaticInfo.IsSaveLogInfo = true;
            System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(Thread_SaveLogInfo));
            thread.IsBackground = true;
            thread.Start();

            Event_SystemLoadFinish?.Invoke();

            LoginAllDev();
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
                var point = topMain.Location;
                point.X = (Width - topMain.Width) / 2;
                topMain.Location = point;
                point = centerMain.Location;
                point.X = (Width - topMain.Width) / 2;
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
            if (mw1.X > 0)
            {
                mw1.X = mainWindow.Location.X - centerMain.Width;
                mainWindow.Location = mw1;
                mw1 = mainWindow2.Location;
                mw1.X = mainWindow2.Location.X + mainWindow2.Width;
                mainWindow2.Location = mw1;
            }
            else
            {
                mw1.X = mainWindow.Location.X + centerMain.Width;
                mainWindow.Location = mw1;
                mw1 = mainWindow2.Location;
                mw1.X = mainWindow2.Location.X - mainWindow2.Width;
                mainWindow2.Location = mw1;
            }
        }
        #endregion
        
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
                    throw new Exception("保存线程日志失败，错误信息：" + ex.Message);
                }
                System.Threading.Thread.Sleep(10);
            }
        }
        
        private void LoginAllDev()//从数据库中取出所有信息,登陆设备
        {
            List<RobotVT.Model.S_D_CameraSet> _CameraSets = new Controller.DataAccess().GetS_D_CameraSetList(0);

            if (_CameraSets.Count<=0)
            {

            }
            else
            {
                foreach(Model.S_D_CameraSet o in _CameraSets)
                {
                    string DVRIPAddress = o.VT_IP; //设备IP地址或者域名 Device IP
                    Int16 DVRPortNumber = Int16.Parse(o.VT_PORT);//设备服务端口号 Device Port
                    string DVRUserName = o.VT_NAME;//设备登录用户名 User name to login
                    string DVRPassword = o.VT_PASSWORD;//设备登录密码 Password to login

                    if (o.VT_ID.ToLower() == "cloud")
                    {
                        cloudPlayView._CameraSet = o;
                        //mainPlayView.sdkLogin("192.168.6.65", 8000, "admin", "zx123456", 1, 0);
                        mainPlayView.sdkLogin(DVRIPAddress, DVRPortNumber, DVRUserName, DVRPassword, 1, 0);
                        cloudPlayView.sdkLogin(DVRIPAddress, DVRPortNumber, DVRUserName, DVRPassword, 1, 0);
                    }
                    if (o.VT_ID.ToLower() == "front")
                    {
                        frontPlayView._CameraSet = o;
                        frontPlayView.sdkLogin(DVRIPAddress, DVRPortNumber, DVRUserName, DVRPassword, 1, 0);
                    }
                    if (o.VT_ID.ToLower() == "back")
                    {
                        backPlayView._CameraSet = o;
                        backPlayView.sdkLogin(DVRIPAddress, DVRPortNumber, DVRUserName, DVRPassword, 1, 0);
                    }
                    if (o.VT_ID.ToLower() == "left")
                    {
                        leftPlayView._CameraSet = o;
                        leftPlayView.sdkLogin(DVRIPAddress, DVRPortNumber, DVRUserName, DVRPassword, 1, 0);
                    }
                    if (o.VT_ID.ToLower() == "right")
                    {
                        rightPlayView._CameraSet = o;
                        rightPlayView.sdkLogin(DVRIPAddress, DVRPortNumber, DVRUserName, DVRPassword, 1, 0);
                    }
                }
            }



        }

        private void LoginOutAll()
        {
            mainPlayView.LoginOut();
            cloudPlayView.LoginOut();
            frontPlayView.LoginOut();
            backPlayView.LoginOut();
            leftPlayView.LoginOut();
            rightPlayView.LoginOut();
        }


        private void Event_PlayViewMouseDoubleClick(string vtid)
        {

            mainPlayView.LoginOut();

            Model.S_D_CameraSet _cameraSetNew = new Controller.DataAccess().GetCameraSet(vtid);

            string DVRIPAddress = _cameraSetNew.VT_IP; //设备IP地址或者域名 Device IP
            Int16 DVRPortNumber = Int16.Parse(_cameraSetNew.VT_PORT);//设备服务端口号 Device Port
            string DVRUserName = _cameraSetNew.VT_NAME;//设备登录用户名 User name to login
            string DVRPassword = _cameraSetNew.VT_PASSWORD;//设备登录密码 Password to login
            
            mainPlayView.sdkLogin(DVRIPAddress, DVRPortNumber, DVRUserName, DVRPassword, 1, 0);
        }


    }
}
 