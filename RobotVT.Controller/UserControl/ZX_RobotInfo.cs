using System;
using System.Windows.Forms;
using System.Reflection;

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
            StaticInfo.HIKAnalysis.Event_Alarm += HIKAnalysis_Event_Alarm;
        }

        private void HIKAnalysis_Event_Alarm(string message)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                if (message == "")
                {
                    lamp.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.lamp_0;
                }
                else
                {
                    lamp.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.lamp_1;
                    lamp.Tag = message;
                }
            }));
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


            System.Threading.Thread tmultcast = new System.Threading.Thread(new System.Threading.ThreadStart(SystemInfoThread));
            tmultcast.IsBackground = true;
            tmultcast.Start();
        }



        public void GetPowerStatus()
        {
            string a;
            float b,c;
            Type t = typeof(System.Windows.Forms.PowerStatus);
            PropertyInfo[] pi = t.GetProperties();

            a = pi[3].GetValue(SystemInformation.PowerStatus, null).ToString();
            //if (a == "1") power2_number.Text = a + "00" + "%";
            //if (a != "1" && a.Remove(0, 3) != "0.0") power2_number.Text = a.Remove(0, 2) + "0%";
            //if (a != "1" && a.Remove(0, 3) == "0.0") power2_number.Text = a.Remove(0, 3) + "%";

            b = float.Parse(a);
            c = 100 * b;
            power2_number.Text = c.ToString() + "%";
            //System.DateTime currentTime = new System.DateTime();
            //string strT = currentTime.ToString("t");
            //time.Text = currentTime.Hour+":"+ currentTime.Minute;
            time.Text = DateTime.Now.ToShortTimeString().ToString();
            if (b< 0.2)
            {

                this.power2.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.power2_0;
            }
            if (b >= 0.2 && b < 0.4)
            {
                this.power2.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.power2_1;
            }
            if (b >= 0.4 && b < 0.6)
            {
                this.power2.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.power2_2;
            }
            if (b >= 0.6 && b < 0.8)
            {
                this.power2.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.power2_3;
            }
            if (b >= 0.8)
            {
                this.power2.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.power2_4;
            }
            //c = b * 100;
            //power2_number.Text = Convert.ToString(c)+"%";
        }
        private void SystemInfoThread()
        {
            try
            {
                while (true)
                {

                    System.Threading.Thread.Sleep(1000);
                    this.Invoke(new MethodInvoker(delegate
                    {
                        
                        time.Text = DateTime.Now.ToShortTimeString();
                        GetPowerStatus();
                        //power1_number.Text =  + "%";
                    }));
                    if (bSerialIsStart)
                        StaticInfo.RobotIM.SendOrder();
                }
            }
            catch (Exception _Ex)
            {
                RobotVT.Controller.Methods.SaveExceptionLog(SK_FModel.SystemEnum.LogType.Exception, "组播数据解析异常：" + _Ex.Message + "\r\n" + _Ex.StackTrace);
                throw new Exception("获取遥控器电量失败，错误信息：" + _Ex.Message);
            }
        }

        bool bSerialIsStart = false;

        private void SerilaPortInfoSet()
        {
            SK_FControl.StaticInfo.FirebirdDBOperator = StaticInfo.FirebirdDBOperator;
            _Sport = _spMethods.GetSerialPortSet(StaticInfo.WirelessController);
            SerialPortInfo = _spMethods.GetSerialPortInfo(StaticInfo.WirelessController);
            
            StaticInfo.RobotIM.InitSerialPortInfo(SerialPortInfo);
            GetPowerStatus();
            bSerialIsStart = StaticInfo.RobotIM.Start();
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
            bSerialIsStart = StaticInfo.RobotIM.Start();
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
                    this.robotPower.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.power3_0;
                }
                if (RobotInfo.BatteryPowerOfRemoteContorller >= 20 && RobotInfo.BatteryPowerOfRemoteContorller < 40)
                {
                    this.power1.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.power1_1;
                    this.robotPower.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.power3_1;
                }
                if (RobotInfo.BatteryPowerOfRemoteContorller >= 40 && RobotInfo.BatteryPowerOfRemoteContorller < 60)
                {
                    this.power1.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.power1_2;
                    this.robotPower.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.power3_2;
                }
                if (RobotInfo.BatteryPowerOfRemoteContorller >= 60 && RobotInfo.BatteryPowerOfRemoteContorller < 80)
                {
                    this.power1.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.power1_3;
                    this.robotPower.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.power3_3;
                }
                if (RobotInfo.BatteryPowerOfRemoteContorller >= 80)
                {
                    this.power1.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.power1_4;
                    this.robotPower.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.power3_4;
                }
                

                //time.Text = DateTime.Now.ToShortTimeString();
                power1_number.Text = RobotInfo.BatteryPowerOfRemoteContorller.ToString() + "%";
                //power2_number.Text = RobotInfo.BatteryPowerOfRemoteContorller.ToString() + "%";
                robotPower_number.Text = RobotInfo.BatteryPowerOfRobot.ToString() + "%";
                speed.Text = RobotInfo.RobotSpeed.ToString() + "m/s";
            }));
        }
    }
}
