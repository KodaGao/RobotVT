using System;
using System.Windows.Forms;

namespace RobotVT.Controller
{
    public partial class ZX_RobotInfo : UserControl
    {
        private SK_FModel.SerialPortInfo SerialPortInfo;

        private SK_FDBUtility.DB.S_D_SERIALPORT _Sport;
        SK_FControl.SerialPortMethods _spMethods = new SK_FControl.SerialPortMethods();

        public ZX_RobotInfo()
        {
            InitializeComponent();
            topMain.MouseUp += new MouseEventHandler(this.topMainWnd_MouseUp);
            topMain.MouseDoubleClick += new MouseEventHandler(this.topMainWnd_MouseDoubleClick);
            StaticInfo.RobotIM.Event_UpdateRealTimeData += RobotInfo_Event_UpdateRealTimeData;
        }

        private void ZX_RobotInfo_Load(object sender, EventArgs e)
        {
            this.topMain.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.top_img;
            this.signal.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.signal_0;
            this.power1.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.power1_0;
            this.power2.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.power2_0;
            this.robotPower.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.power3_0;
            this.lamp.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.lamp_0;

            SerilaPortInfoSet();
        }

        private void SerilaPortInfoSet()
        {
            SK_FControl.StaticInfo.FirebirdDBOperator = StaticInfo.FirebirdDBOperator;
            _Sport = _spMethods.GetSerialPortSet(StaticInfo.WirelessController);
            SerialPortInfo = _spMethods.GetSerialPortInfo(StaticInfo.WirelessController);
            
            StaticInfo.RobotIM.InitSerialPortInfo(SerialPortInfo);
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
            StaticInfo.RobotIM.Stop();
            SerialPortInfo = _spMethods.GetSerialPortInfo(StaticInfo.WirelessController);

            StaticInfo.RobotIM.InitSerialPortInfo(SerialPortInfo);
            StaticInfo.RobotIM.Start();
        }

        private void topMainWnd_MouseDoubleClick(object sender, MouseEventArgs e)
        {
        }


        private void RobotInfo_Event_UpdateRealTimeData(object sender)
        {
            RobotInfo RobotInfo = (RobotInfo)sender;
            this.Invoke(new MethodInvoker(delegate
            {
                this.signal.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.signal_0;

                if (RobotInfo.BatteryPowerOfRemoteContorller < 20)
                {
                    this.power1.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.power1_0;
                    this.power2.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.power2_0;
                }
                if (RobotInfo.BatteryPowerOfRemoteContorller >= 20 && RobotInfo.BatteryPowerOfRemoteContorller < 40)
                {
                    this.power1.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.power1_1;
                    this.power2.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.power2_1;
                }
                if (RobotInfo.BatteryPowerOfRemoteContorller >= 40 && RobotInfo.BatteryPowerOfRemoteContorller < 60)
                {
                    this.power1.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.power1_2;
                    this.power2.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.power2_2;
                }
                if (RobotInfo.BatteryPowerOfRemoteContorller >= 60 && RobotInfo.BatteryPowerOfRemoteContorller < 80)
                {
                    this.power1.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.power1_3;
                    this.power2.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.power2_3;
                }
                if (RobotInfo.BatteryPowerOfRemoteContorller >= 80)
                {
                    this.power1.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.power1_4;
                    this.power2.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.power2_4;
                }

                this.robotPower.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.power3_0;
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
