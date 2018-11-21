using System;
using System.Windows.Forms;

namespace RobotVT.Controller
{
    public partial class ZX_RobotInfo : UserControl
    {
        private SK_FModel.SerialPortInfo SerialPortInfo;

        public SK_FDBUtility.DB.S_D_SERIALPORT _Sport { get; set; }
        SK_FControl.SerialPortMethods _spMethods = new SK_FControl.SerialPortMethods();

        public ZX_RobotInfo()
        {
            InitializeComponent();
            topMain.MouseUp += new MouseEventHandler(this.topMainWnd_MouseUp);
            topMain.MouseDoubleClick += new MouseEventHandler(this.topMainWnd_MouseDoubleClick);
        }

        private void ZX_RobotInfo_Load(object sender, EventArgs e)
        {
            this.topMain.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.top_img;
            this.signal.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.signal_5;
            this.power1.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.power1_3;
            this.power2.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.power2_2;
            this.robotPower.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.power3_3;
            this.lamp.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.lamp_0;

            SerilaPortInfoSet();
        }

        private void SerilaPortInfoSet()
        {
            SK_FControl.StaticInfo.FirebirdDBOperator = StaticInfo.FirebirdDBOperator;
            _Sport = _spMethods.GetSerialPortSet(StaticInfo.WirelessController);
            SerialPortInfo = _spMethods.GetSerialPortInfo(StaticInfo.WirelessController);

            if (SerialPortInfo == null)
            {
                SerialPortInfo = new SK_FModel.SerialPortInfo();
            }

            SerialPortInfo.SenderOrderInterval = StaticInfo.SenderOrderInterval;
            StaticInfo.ModbusHelper.Init(SerialPortInfo);
            StaticInfo.RobotIM.Event_UpdateRealTimeData += RobotInfo_Event_UpdateRealTimeData;
            StaticInfo.RobotIM.Start();
        }


        private void topMainWnd_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                HIK_SPortSet hIK_SPort = new HIK_SPortSet();
                hIK_SPort.Event_SetFinish += HIK_SPortSet_Event_SetFinish;
                hIK_SPort.FirebirdDBOperator = StaticInfo.FirebirdDBOperator;
                hIK_SPort.SPortKey = StaticInfo.WirelessController;
                if (_Sport != null)
                {
                    hIK_SPort.SPort = _Sport;
                }
                hIK_SPort.ShowDialog();

            }
        }

        private void HIK_SPortSet_Event_SetFinish()
        {
            //sdkLoginOut();

            //Model.S_D_CameraSet _cameraSetNew = new Controller.DataAccess().GetCameraSet(_CameraSet.VT_ID);

            //string DVRIPAddress = _cameraSetNew.VT_IP; //设备IP地址或者域名 Device IP
            //Int16 DVRPortNumber = Int16.Parse(_cameraSetNew.VT_PORT);//设备服务端口号 Device Port
            //string DVRUserName = _cameraSetNew.VT_NAME;//设备登录用户名 User name to login
            //string DVRPassword = _cameraSetNew.VT_PASSWORD;//设备登录密码 Password to login

            //sdkLogin(DVRIPAddress, DVRPortNumber, DVRUserName, DVRPassword, 1, 0);
            //_CameraSet = _cameraSetNew;

        }

        private void topMainWnd_MouseDoubleClick(object sender, MouseEventArgs e)
        {
        }


        private void RobotInfo_Event_UpdateRealTimeData(object sender)
        {
            RobotInfo RobotInfo = (RobotInfo)sender;
            this.Invoke(new MethodInvoker(delegate
            {
                this.signal.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.signal_5;
                this.power1.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.power1_3;
                this.power2.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.power2_2;
                this.robotPower.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.power3_3;
                this.lamp.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.lamp_0;

                time.Text = DateTime.Now.ToShortTimeString();
                power1_number.Text = RobotInfo.BatteryPowerOfRemoteContorller.ToString() + "%";
                power2_number.Text = RobotInfo.BatteryPowerOfRemoteContorller.ToString() + "%";
                robotPower_number.Text = RobotInfo.BatteryPowerOfRobot.ToString() + "%";
                speed.Text = RobotInfo.RobotSpeed.ToString() + "m/s";
            }));
        }
    }
}
