using DevComponents.DotNetBar.Metro;
using System;
using System.IO.Ports;
using static SK_FControl.SerialPortEnum;

namespace SK_FControl
{
    public partial class SerailPortSet : MetroForm
    {
        public string serialport = string.Empty;
        public string baudrate = string.Empty;
        public string databit = string.Empty;
        public string parity = string.Empty;
        public string stopbit = string.Empty;
        public string timeout = string.Empty;
        public string samplerate = string.Empty;

        public SerailPortSet()
        {
            InitializeComponent();
        }

        private void SerailPortSet_Load(object sender, EventArgs e)
        {
            //通讯端口、波特率、数据位、校验位、停止位
            //绑定端口号
            SetPortNameValues(cBEPortName);
            cBEPortName.SelectedIndex = 0;

            //波特率
            SetBauRateValues(cBEBaudRate);
            cBEBaudRate.SelectedText = "9600";

            //数据位
            SetDataBitsValues(cBEDataBits);
            cBEDataBits.SelectedText = "8";

            //校验位
            SetParityValues(cBEParity);
            cBEParity.SelectedIndex = 0;

            //停止位
            SetStopBitValues(cBEStopBits);
            cBEStopBits.SelectedIndex = 1;

            tBXTimeOut.Text = "1000";
            //7) SampleTime
            tBXSample.Text = "1000";

            databaseValue();

        }

        #region 常用的列表数据获取和绑定操作

        /// <summary>
        /// 封装获取串口号列表
        /// </summary>
        /// <returns></returns>
        public static string[] GetPortNames()
        {
            return SerialPort.GetPortNames();
        }

        /// <summary>
        /// 设置串口号
        /// </summary>
        /// <param name="obj"></param>
        public static void SetPortNameValues(DevComponents.DotNetBar.Controls.ComboBoxEx obj)
        {
            obj.Items.Clear();
            foreach (string str in SerialPort.GetPortNames())
            {
                obj.Items.Add(str);
            }
        }

        /// <summary>
        /// 设置波特率
        /// </summary>
        public static void SetBauRateValues(DevComponents.DotNetBar.Controls.ComboBoxEx obj)
        {
            obj.Items.Clear();
            foreach (SerialPortBaudRates rate in Enum.GetValues(typeof(SerialPortBaudRates)))
            {
                obj.Items.Add(((int)rate).ToString());
            }
        }

        /// <summary>
        /// 设置数据位
        /// </summary>
        public static void SetDataBitsValues(DevComponents.DotNetBar.Controls.ComboBoxEx obj)
        {
            obj.Items.Clear();
            foreach (SerialPortDatabits databit in Enum.GetValues(typeof(SerialPortDatabits)))
            {
                obj.Items.Add(((int)databit).ToString());
            }
        }

        /// <summary>
        /// 设置校验位列表
        /// </summary>
        public static void SetParityValues(DevComponents.DotNetBar.Controls.ComboBoxEx obj)
        {
            obj.Items.Clear();
            foreach (string str in Enum.GetNames(typeof(Parity)))
            {
                obj.Items.Add(str);
            }
        }

        /// <summary>
        /// 设置停止位
        /// </summary>
        public static void SetStopBitValues(DevComponents.DotNetBar.Controls.ComboBoxEx obj)
        {
            obj.Items.Clear();
            foreach (string str in Enum.GetNames(typeof(StopBits)))
            {
                obj.Items.Add(str);
            }
        }

        private void databaseValue()
        {
            cBEPortName.Text = serialport;
            cBEBaudRate.Text = baudrate;
            cBEDataBits.Text = databit;
            cBEParity.Text = parity;
            cBEStopBits.Text = stopbit;
            tBXTimeOut.Text = timeout;
            tBXSample.Text = samplerate;
        }
        #endregion

        private void btX_Save_Click(object sender, EventArgs e)
        {
            serialport = cBEPortName.SelectedItem.ToString();
            baudrate = cBEBaudRate.SelectedItem.ToString();
            databit = cBEDataBits.SelectedItem.ToString();
            parity = cBEParity.SelectedItem.ToString();
            stopbit = cBEStopBits.SelectedItem.ToString();
            timeout = tBXTimeOut.Text;
            samplerate = tBXSample.Text;
            this.Close();
        }

        private void btX_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
