using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SK_FCommon;
using static SK_FModel.SerialPortEnum;
using System.IO.Ports;
using System.Threading;

namespace RobotVT.Controller
{
    public partial class ZX_RobotInfo : UserControl
    {

        private SK_FCommon.SerialPortUtil _spuinput;//输入端口
        private readonly ModbusRTUControl _rtuModbus = new ModbusRTUControl();

        public SK_FDBUtility.DB.S_D_SERIALPORT _Sport { get; set; }
        SK_FControl.SerialPortMethods spMethods = new SK_FControl.SerialPortMethods();



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

            string errortext = "";
            if (_spuinput == null)
            {
                SK_FControl.StaticInfo.FirebirdDBOperator = StaticInfo.FirebirdDBOperator;
                _Sport = spMethods.GetSerialPortSet(StaticInfo.WirelessController);
                if (!InitComm())
                {
                    DevComponents.DotNetBar.MessageBoxEx.Show(errortext, "错误提示");
                    CloseComm();
                    return;
                }
                _spuinput.DataReceived += _spuupload_DataReceived;
            }
            //}
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
            CloseComm();
            _spuinput.DataReceived -= _spuupload_DataReceived;
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
            //if (_CameraSet == null) return;

            //Event_PlayViewMouseDoubleClick?.Invoke(_CameraSet.VT_ID);

            //base.RealPlayWnd_MouseDoubleClick(sender, e);
        }


        public bool CloseComm()
        {
            string errortext;
            try
            {
                _spuinput.DiscardBuffer();
                System.Windows.Forms.Application.DoEvents();
                _spuinput.ClosePort();
                //LogManager.WriteLog(LogFile.Trace, "关闭" + key + "串口,端口为【" + sp.PortName.ToString() + "】");
                return true;
            }
            catch (System.Exception ex)
            {
                //LogManager.WriteLog(LogFile.Error, ex.ToString());
                errortext = ex.Message.ToString();
                return false;
            }
        }
        public bool InitComm()
        {
            string errortext;
            string sp_key = _Sport.SP_KEY;
            try
            {
                if (_spuinput != null && !_spuinput.IsOpen)
                {
                    _spuinput.OpenPort();
                }
                else if (_spuinput != null && _spuinput.IsOpen)
                {
                }
                else if (_spuinput == null)
                {
                    string serialName = _Sport.SP_PORT;
                    SerialPortBaudRates rate = (SerialPortBaudRates)Enum.Parse(typeof(SerialPortBaudRates), _Sport.SP_BAUDRATE);
                    SerialPortDatabits databit = (SerialPortDatabits)int.Parse(_Sport.SP_DATABIT);
                    Parity party = (Parity)Enum.Parse(typeof(Parity), _Sport.SP_PRATITY);
                    StopBits stopbit = (StopBits)Enum.Parse(typeof(StopBits), _Sport.SP_STOPBIT);
                    string timeout = _Sport.SP_TIMEOUT;

                    _spuinput = new SerialPortUtil(serialName, rate, party, databit, stopbit, timeout);
                    _spuinput.OpenPort();

                }
                _spuinput.DiscardBuffer();
                return true;
            }
            catch (System.Exception ex)
            {
                errortext = ex.Message.ToString();
                return false;
            }
        }
        private void _spuupload_DataReceived(DataReceivedEventArgs e)
        {
            if (e.DataReceived == "") return;
            //short[] command = _StartControl.ControlValues;
            try
            {
                //000000 - Rx:01 10 00 14 00 0A 14 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 CF BE
                //000001 - Tx:01 10 00 14 00 0A 00 0A
                //000002 - Rx:01 03 00 0B 00 05 F4 0B
                //000003 - Tx:01 03 0A 00 00 00 00 00 00 00 00 00 00 24 B6

                short[] commandwrite = new short[1 + 1 + 2 + 2 + 1 + 2 * 10 + 2];
                short[] commandread = new short[1 + 1 + 2 + 2 + 2];
                string[] received = ValueHelper.Instance.GetSplitString(e.DataReceived, " ");
                string slaveid = Convert.ToInt32(received[0], 16).ToString();
                if (slaveid != "1") return;
                if (received.Length < commandwrite.Length) return;
                if (received.Length < commandread.Length) return;
                string functioncode = received[1];
                int regstartaddress = Convert.ToInt32(received[2] + received[3], 16);
                int reglength = Convert.ToInt32(received[4] + received[5], 16);
                //if (regstartaddress + reglength > commandwrite.Length) return;
                short[] commandnew = new short[reglength];
                //for (int i = 0; i < reglength; i++)
                //{
                //    commandnew[i] = commandwrite[regstartaddress + i];
                //}
                byte[] commandresponse = _rtuModbus.ModbusResponse(Byte.Parse(slaveid), (FunctionCode)(Enum.Parse(typeof(FunctionCode), functioncode)), commandnew);
                _spuinput.WriteData(commandresponse);
                ZigbeeUi(new SK_FModel.SerialPortReceiveData());
            }
            catch (Exception ex)
            {
                throw new Exception("错误信息：" + ex.Message);
            }
        }
        private delegate void MyInvokeValue(object values);
        private void ZigbeeUi(SK_FModel.SerialPortReceiveData rd)
        {
            bool checkresponse = rd.ModbusResponse;
            string metermessage = rd.ModbusMessage;
            MyInvokeValue milbtimer = ZigbeeUiSet;
            Invoke(milbtimer, rd);
        }
        private void ZigbeeUiSet(object objvalue)
        {
            SK_FModel.SerialPortReceiveData value = (SK_FModel.SerialPortReceiveData)objvalue;

            this.signal.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.signal_5;
            this.power1.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.power1_3;
            this.power2.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.power2_2;
            this.robotPower.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.power3_3;
            this.lamp.Style.BackgroundImage = RobotVT.Resources.Properties.Resources.lamp_0;

            time.Text = DateTime.Now.ToShortTimeString();
            power1_number.Text = "78%";
            power2_number.Text = "33%";
            robotPower_number.Text = "12%";
            speed.Text = "111m/s";
        }
    }
}
