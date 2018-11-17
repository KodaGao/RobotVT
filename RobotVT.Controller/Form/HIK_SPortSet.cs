using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RobotVT.Controller
{
    public partial class HIK_SPortSet : SK_FControl.SerialPortForm
    {

        public HIK_SPortSet()
        {
            InitializeComponent();
        }

        //public override void DatabaseValue()
        //{
        //    //SPort = spMethods.GetSerialPortSet(SPortKey);
        //    //if (SPort == null)
        //    //{
        //    //    SPort = new SK_FDBUtility.DB.S_D_SERIALPORT();

        //    //    SPort.SP_KEY = SPortKey;
        //    //    SPort.SP_PORT = "COM1";
        //    //    SPort.SP_BAUDRATE = "9600";
        //    //    cBEDataBits.Text = SPort.SP_DATABIT = "8";
        //    //    cBEParity.Text = SPort.SP_PRATITY = "None";
        //    //    cBEStopBits.Text = SPort.SP_STOPBIT = "One";
        //    //    tBXTimeOut.Text = SPort.SP_TIMEOUT = "1000";
        //    //    tBXSample.Text = SPort.SP_SAMPLE = "1000";
        //    //    spMethods.InsertSerialPortSet(SPort);
        //    //}
        //    //cBEPortName.Text = SPort.SP_PORT;
        //    //cBEBaudRate.Text = SPort.SP_BAUDRATE;
        //    //cBEDataBits.Text = SPort.SP_DATABIT;
        //    //cBEParity.Text = SPort.SP_PRATITY;
        //    //cBEStopBits.Text = SPort.SP_STOPBIT;
        //    //tBXTimeOut.Text = SPort.SP_TIMEOUT;
        //    //tBXSample.Text = SPort.SP_SAMPLE;
        //}

    }
}
