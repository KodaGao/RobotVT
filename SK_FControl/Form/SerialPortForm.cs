﻿using DevComponents.DotNetBar.Metro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SK_FModel.SerialPortEnum;

namespace SK_FControl
{
    public partial class SerialPortForm : MetroForm
    {
        public virtual event SK_FModel.SystemDelegate.del_SystemSetFinish Event_SetFinish;
        public virtual SK_FDBUtility.DB.S_D_SERIALPORT SPort { get; set; }
        public virtual string SPortKey { get; set; }

        public string serialport = string.Empty;
        public string baudrate = string.Empty;
        public string databit = string.Empty;
        public string parity = string.Empty;
        public string stopbit = string.Empty;
        public string timeout = string.Empty;
        public string samplerate = string.Empty;
        
        public SerialPortForm()
        {
            InitializeComponent();
        }

        private void SerialPortForm_Load(object sender, EventArgs e)
        {
            //通讯端口、波特率、数据位、校验位、停止位
            //1)绑定端口号
            SetPortNameValues(cBEPortName);
            cBEPortName.SelectedIndex = 0;

            //2)波特率
            SetBauRateValues(cBEBaudRate);
            cBEBaudRate.SelectedText = "9600";

            //3)数据位
            SetDataBitsValues(cBEDataBits);
            this.cBEDataBits.SelectedText = "8";

            //4)校验位
            SetParityValues(cBEParity);
            this.cBEParity.SelectedIndex = 0;

            //5)停止位
            SetStopBitValues(cBEStopBits);
            this.cBEStopBits.SelectedIndex = 1;

            //6) ResponseTimeout
            tBXTimeOut.Text = "1000";
            //7) SampleTime
            tBXSample.Text = "1000";

            DatabaseValue();
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

        #endregion

        private void DatabaseValue()
        {
            cBEPortName.Text = SPort.SP_PORT;
            cBEBaudRate.Text = SPort.SP_BAUDRATE;
            cBEDataBits.Text = SPort.SP_DATABIT;
            cBEParity.Text = SPort.SP_PRATITY;
            cBEStopBits.Text = SPort.SP_STOPBIT;
            tBXTimeOut.Text = SPort.SP_TIMEOUT;
            tBXSample.Text = SPort.SP_SAMPLE;
        }

        public virtual void btX_Save_Click(object sender, EventArgs e)
        {
            if (SPort == null)
            {
                SPort = new SK_FDBUtility.DB.S_D_SERIALPORT();
                SPort.SP_KEY = SPortKey;
            }

            bool bmodify = false;

            if (SPort.SP_KEY != cBEPortName.SelectedValue.ToString())
            {
                bmodify = true;
                SPort.SP_KEY = cBEPortName.SelectedValue.ToString();
            }
            if (SPort.SP_BAUDRATE != cBEBaudRate.SelectedValue.ToString())
            {
                bmodify = true;
                SPort.SP_BAUDRATE = cBEBaudRate.SelectedValue.ToString();
            }
            if (SPort.SP_DATABIT != cBEDataBits.SelectedValue.ToString())
            {
                bmodify = true;
                SPort.SP_DATABIT = cBEDataBits.SelectedValue.ToString();
            }
            if (SPort.SP_PRATITY != cBEParity.SelectedValue.ToString())
            {
                bmodify = true;
                SPort.SP_PRATITY = cBEParity.SelectedValue.ToString();
            }
            if (SPort.SP_STOPBIT != cBEStopBits.SelectedValue.ToString())
            {
                bmodify = true;
                SPort.SP_STOPBIT = cBEStopBits.SelectedValue.ToString();
            }
            if (SPort.SP_TIMEOUT != tBXTimeOut.Text)
            {
                bmodify = true;
                SPort.SP_TIMEOUT = tBXTimeOut.Text;
            }
            if (SPort.SP_SAMPLE != tBXSample.Text)
            {
                bmodify = true;
                SPort.SP_SAMPLE = tBXSample.Text;
            }
            if (bmodify)
            {
                //new Controller.DataAccess().UpdateCameraSet(_CameraSet);
                Event_SetFinish?.Invoke();
            }
            
            this.Close();
        }

        public virtual void btX_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}