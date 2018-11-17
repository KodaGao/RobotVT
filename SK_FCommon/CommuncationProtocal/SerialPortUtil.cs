using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using SK_FModel;
using System.Threading;
using static SK_FModel.SerialPortEnum;

namespace SK_FCommon
{
    /// <summary>
    /// 串口开发辅助类
    /// </summary>
    public class SerialPortUtil
    {
        /// <summary>
        /// 接收事件是否有效 false表示有效
        /// </summary>
        public bool ReceiveEventFlag = false;
         ////<summary>
         ////结束符比特
         ////</summary>
        public byte EndByte = 0x23;//string End = "#";
        private ModbusRTUControl rtu_modbus = new ModbusRTUControl();
         ////<summary>
         ////完整协议的记录处理事件
        ////</summary>
        public event DataReceivedEventHandler DataReceived;
        public event SerialErrorReceivedEventHandler Error;

        #region 变量属性
        private string _portName = "COM1";//串口号，默认COM1
        private SerialPortBaudRates _baudRate = SerialPortBaudRates.BaudRate_9600;//波特率
        private SerialPortDatabits _dataBits = SerialPortDatabits.EightBits;//数据位
        private Parity _parity = Parity.None;//校验位
        private StopBits _stopBits = StopBits.One;//停止位
        private int _timeout = 1000;
        private SerialPort comPort = new SerialPort();

        /// <summary>
        /// 串口号
        /// </summary>
        public string PortName
        {
            get { return _portName; }
            set { _portName = value; }
        }

        /// <summary>
        /// 波特率
        /// </summary>
        public SerialPortBaudRates BaudRate
        {
            get { return _baudRate; }
            set { _baudRate = value; }
        }

        /// <summary>
        /// 奇偶校验位
        /// </summary>
        public Parity Parity
        {
            get { return _parity; }
            set { _parity = value; }
        }

        /// <summary>
        /// 数据位
        /// </summary>
        public SerialPortDatabits DataBits
        {
            get { return _dataBits; }
            set { _dataBits = value; }
        }

        /// <summary>
        /// 停止位
        /// </summary>
        public StopBits StopBits
        {
            get { return _stopBits; }
            set { _stopBits = value; }
        }

        public int TimeOut
        {
            get { return _timeout; }
            set { _timeout = value; }
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 参数构造函数（使用枚举参数构造）
        /// </summary>
        /// <param name="baud">波特率</param>
        /// <param name="par">奇偶校验位</param>
        /// <param name="sBits">停止位</param>
        /// <param name="dBits">数据位</param>
        /// <param name="name">串口号</param>
        public SerialPortUtil(string name, SerialPortBaudRates baud, Parity par, SerialPortDatabits dBits, StopBits sBits)
        {
            _portName = name;
            _baudRate = baud;
            _parity = par;
            _dataBits = dBits;
            _stopBits = sBits;

            comPort.DataReceived += new SerialDataReceivedEventHandler(comPort_DataReceived);
            comPort.ErrorReceived += new SerialErrorReceivedEventHandler(comPort_ErrorReceived);
        }

        /// <summary>
        /// 参数构造函数（使用字符串参数构造）
        /// </summary>
        /// <param name="baud">波特率</param>
        /// <param name="par">奇偶校验位</param>
        /// <param name="sBits">停止位</param>
        /// <param name="dBits">数据位</param>
        /// <param name="name">串口号</param>
        public SerialPortUtil(string name, string baud, string par, string dBits, string sBits)
        {
            _portName = name;
            _baudRate = (SerialPortBaudRates)Enum.Parse(typeof(SerialPortBaudRates), baud);
            _parity = (Parity)Enum.Parse(typeof(Parity), par);
            _dataBits = (SerialPortDatabits)Enum.Parse(typeof(SerialPortDatabits), dBits);
            _stopBits = (StopBits)Enum.Parse(typeof(StopBits), sBits);

            comPort.DataReceived += new SerialDataReceivedEventHandler(comPort_DataReceived);
            comPort.ErrorReceived += new SerialErrorReceivedEventHandler(comPort_ErrorReceived);
        }

        /// <summary>
        /// 参数构造函数（使用字符串参数构造）
        /// </summary>
        /// <param name="baud">波特率</param>
        /// <param name="par">奇偶校验位</param>
        /// <param name="sBits">停止位</param>
        /// <param name="dBits">数据位</param>
        /// <param name="name">串口号</param>
        /// <param name="itimeout">超时</param>
        public SerialPortUtil(string name, SerialPortBaudRates baud, Parity par, SerialPortDatabits dBits, StopBits sBits, string itimeout)
        {
            _portName = name;
            _baudRate = baud;
            _parity = par;
            _dataBits = dBits;
            _stopBits = sBits;
            _timeout = Int32.Parse(itimeout);
            //_baudRate = (SerialPortBaudRates)Enum.Parse(typeof(SerialPortBaudRates), baud);
            //_parity = (Parity)Enum.Parse(typeof(Parity), par);
            //_dataBits = (SerialPortDatabits)Enum.Parse(typeof(SerialPortDatabits), dBits);
            //_stopBits = (StopBits)Enum.Parse(typeof(StopBits), sBits);
            //_timeout = Int32.Parse(itimeout);
            
            comPort.DataReceived += new SerialDataReceivedEventHandler(comPort_DataReceived);
            comPort.ErrorReceived += new SerialErrorReceivedEventHandler(comPort_ErrorReceived);
        }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public SerialPortUtil()
        {
            _portName = "COM1";
            _baudRate = SerialPortBaudRates.BaudRate_9600;
            _parity = Parity.None;
            _dataBits = SerialPortDatabits.EightBits;
            _stopBits = StopBits.One;

            comPort.DataReceived += new SerialDataReceivedEventHandler(comPort_DataReceived);
            comPort.ErrorReceived += new SerialErrorReceivedEventHandler(comPort_ErrorReceived);
        } 

	    #endregion

        /// <summary>
        /// 端口是否已经打开
        /// </summary>
        public bool IsOpen
        {
            get
            {
                return comPort.IsOpen;
            }
        }

        /// <summary>
        /// 打开端口
        /// </summary>
        /// <returns></returns>
        public void OpenPort()
        {
            if (comPort.IsOpen) comPort.Close();

            comPort.PortName = _portName;
            comPort.BaudRate = (int)_baudRate;
            comPort.Parity = _parity;
            comPort.DataBits = (int)_dataBits;
            comPort.StopBits = _stopBits;

            comPort.WriteTimeout = (int)_timeout;
            comPort.ReadTimeout = (int)_timeout;
            comPort.RtsEnable = true;
            comPort.DtrEnable = true;

            comPort.Open();
        }

        /// <summary>
        /// 关闭端口
        /// </summary>
        public void ClosePort()
        {
            comPort.Close();
        }

        /// <summary>
        /// 丢弃来自串行驱动程序的接收和发送缓冲区的数据
        /// </summary>
        public void DiscardBuffer()
        {
            comPort.DiscardInBuffer();
            comPort.DiscardOutBuffer();
        }

        /// <summary>
        /// 数据接收处理
        /// </summary>
        void comPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
           
            //禁止接收事件时直接退出
            if (ReceiveEventFlag) return;

            #region 根据结束字节来判断是否全部获取完成
            List<byte> _byteData = new List<byte>();


            //bool found = false;//是否检测到结束符号
            //while (comPort.BytesToRead > 0 || !found)
            //{
            byte[] readBuffer = new byte[comPort.ReadBufferSize + 1];
            int count = 0;
            try
            {
                if (comPort.IsOpen)
                    count = comPort.Read(readBuffer, 0, comPort.ReadBufferSize);
            }
            catch(Exception ex)
            {

            }

            Monitor.Enter(_byteData);
            for (int i = 0; i < count; i++)
            {
                _byteData.Add(readBuffer[i]);
            }
            Monitor.Exit(_byteData);
            //}
            #endregion

            //字符转换
            byte[] byteData = _byteData.ToArray();
            string readString = ByteToHex(byteData);
            string readtimeString = string.Format("{0:HH:mm:ss:ffff}", DateTime.Now);

            //触发整条记录的处理
            if (DataReceived != null)
            {
                DataReceived(new DataReceivedEventArgs("", "", readString, readtimeString));
            }
        }

        /// <summary>
        /// 错误处理函数
        /// </summary>
        void comPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            if (Error != null)
            {
                Error(sender, e);
            }
        }

        #region 数据写入操作

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="msg"></param>
        public void WriteData(string msg)
        {
            if (!(comPort.IsOpen)) comPort.Open();

            //ReceiveEventFlag = true;        //关闭接收事件
            comPort.Write(msg);
            //ReceiveEventFlag = false;       //打开事件
        }

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="msg">写入端口的字节数组</param>
        public void WriteData(byte[] msg)
        {
            if (!(comPort.IsOpen)) comPort.Open();

            //ReceiveEventFlag = true;        //关闭接收事件
            comPort.Write(msg, 0, msg.Length);
            string sendString = ByteToHex(msg);
            string sendtimeString = string.Format("{0:HH:mm:ss:ffff}", DateTime.Now);

            //触发整条记录的处理
            if (DataReceived != null)
            {
                DataReceived(new DataReceivedEventArgs(sendString, sendtimeString, "", ""));
            }
            //ReceiveEventFlag = false;       //打开事件
        }

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="msg">包含要写入端口的字节数组</param>
        /// <param name="offset">参数从0字节开始的字节偏移量</param>
        /// <param name="count">要写入的字节数</param>
        public void WriteData(byte[] msg, int offset, int count)
        {
            if (!(comPort.IsOpen)) comPort.Open();

            //ReceiveEventFlag = true;        //关闭接收事件
            comPort.Write(msg, offset, count);
            //ReceiveEventFlag = false;       //打开事件
        }

        /// <summary>
        /// 发送串口命令
        /// </summary>
        /// <param name="SendData">发送数据</param>
        /// <param name="ReceiveData">接收数据</param>
        /// <param name="Overtime">重复次数</param>
        /// <returns></returns>
        public byte[] SendCommand(byte[] SendData, ref  byte[] ReceiveData, int Overtime)
        {
            if (!(comPort.IsOpen)) comPort.Open();

            ReceiveEventFlag = true;        //关闭接收事件
            comPort.DiscardInBuffer();      //清空接收缓冲区                 
            comPort.Write(SendData, 0, SendData.Length);
            Thread.Sleep(150);

            string sendString = ByteToHex(SendData);
            string sendtimeString = string.Format("{0:HH:mm:ss:ffff}", DateTime.Now);
            if (DataReceived != null)
            {
                DataReceived(new DataReceivedEventArgs(sendString, sendtimeString, "", ""));
            }
            int num = 0, ret = 0;
            while (num++ < Overtime)
            {
                if (comPort.BytesToRead >= ReceiveData.Length) break;
                System.Threading.Thread.Sleep(1);
            }
            if (comPort.BytesToRead >= ReceiveData.Length)
            {
                if (!(comPort.IsOpen))
                    return ReceiveData;

                    ret = comPort.Read(ReceiveData, 0, ReceiveData.Length);
            }

            ReceiveEventFlag = false;       //打开事件

            //字符转换
            string readString = ByteToHex(ReceiveData);
            string readtimeString = string.Format("{0:HH:mm:ss:ffff}", DateTime.Now);

            //触发整条记录的处理
            if (DataReceived != null && ret != 0)
            {
                DataReceived(new DataReceivedEventArgs("", "", readString, readtimeString));
            }

            return ReceiveData;
        }

        /// <summary>
        /// 发送串口命令
        /// </summary>
        /// <param name="SendData">发送数据</param>
        /// <param name="ReceiveData">接收数据</param>
        /// <param name="Overtime">重复次数</param>
        /// <returns></returns>
        public SerialPortReceiveData SendCommand(byte[] SendData, byte[] ReceiveData, int Overtime)
        {
            if (!(comPort.IsOpen)) comPort.Open();

            SerialPortReceiveData rd = new SerialPortReceiveData();

            try
            {
                ReceiveEventFlag = true;        //关闭接收事件
                comPort.DiscardInBuffer();      //清空接收缓冲区                 
                comPort.Write(SendData, 0, SendData.Length);
                Thread.Sleep(150);

                string sendString = ByteToHex(SendData);
                string sendtimeString = string.Format("{0:HH:mm:ss:ffff}", DateTime.Now);
                if (DataReceived != null)
                {
                    DataReceived(new DataReceivedEventArgs(sendString, sendtimeString, "", ""));
                }
                int num = 0, ret = 0;
                while (num++ < Overtime)
                {
                    if (comPort.BytesToRead >= ReceiveData.Length) break;
                    System.Threading.Thread.Sleep(1);
                }
                if (comPort.BytesToRead >= ReceiveData.Length)
                {
                    if (!(comPort.IsOpen))
                        return rd;
                    ret = comPort.Read(ReceiveData, 0, ReceiveData.Length);
                }

                rd.ReceiveData = ReceiveData;
                rtu_modbus.ModbusReturn(comPort.BytesToRead, ref rd);
                rd.SlaveID = SendData[0];
                ReceiveEventFlag = false;       //打开事件

                //字符转换
                string readString = ByteToHex(ReceiveData);
                string readtimeString = string.Format("{0:HH:mm:ss:ffff}", DateTime.Now);

                //触发整条记录的处理
                if (DataReceived != null && rd.ModbusResponse)
                {
                    DataReceived(new DataReceivedEventArgs("", "", readString, readtimeString));
                }
            }
            catch (Exception ex)
            {

            }

            return rd;
        }
        public RLSReceiveData RLSSendCommand(byte[] SendData, int Overtime)
        {
            if (!(comPort.IsOpen)) comPort.Open();

            RLSReceiveData rd = new RLSReceiveData();
            byte[] ReceiveData = new byte[9];
            try
            {

                ReceiveEventFlag = true;        //关闭接收事件
                comPort.DiscardInBuffer();      //清空接收缓冲区                 
                comPort.Write(SendData, 0, SendData.Length);

                string sendString = ByteToHex(SendData);
                string sendtimeString = string.Format("{0:HH:mm:ss:ffff}", DateTime.Now);
                if (DataReceived != null)
                {
                    DataReceived(new DataReceivedEventArgs(sendString, sendtimeString, "", ""));
                }
                int num = 0, ret = 0;
                while (num++ < Overtime)
                {
                    if (comPort.BytesToRead >= ReceiveData.Length) break;
                    System.Threading.Thread.Sleep(1);
                }
                Thread.Sleep(500);
                if (comPort.BytesToRead >= ReceiveData.Length)
                {
                    if (!(comPort.IsOpen))
                        return rd;

                    ret = comPort.Read(ReceiveData, 0, ReceiveData.Length);
                }

                ReceiveEventFlag = false;       //打开事件

                //字符转换
                string readString = ByteToHex(ReceiveData);
                string readtimeString = string.Format("{0:HH:mm:ss:ffff}", DateTime.Now);

                //触发整条记录的处理
                if (DataReceived != null && ret != 0)
                {
                    DataReceived(new DataReceivedEventArgs("", "", readString, readtimeString));
                }
                rd.SlaveID = SendData[1];
                rd.ReceiveData = ReceiveData;
            }
            catch (Exception ex)
            {

            }

            return rd;
        }
        #endregion
         
        #region 格式转换
        /// <summary>
        /// 转换十六进制字符串到字节数组
        /// </summary>
        /// <param name="msg">待转换字符串</param>
        /// <returns>字节数组</returns>
        public static byte[] HexToByte(string msg)
        {
            msg = msg.Replace(" ", "");//移除空格

            //create a byte array the length of the
            //divided by 2 (Hex is 2 characters in length)
            byte[] comBuffer = new byte[msg.Length / 2];
            for (int i = 0; i < msg.Length; i += 2)
            {
                //convert each set of 2 characters to a byte and add to the array
                comBuffer[i / 2] = (byte)Convert.ToByte(msg.Substring(i, 2), 16);
            }

            return comBuffer;
        }

        /// <summary>
        /// 转换字节数组到十六进制字符串
        /// </summary>
        /// <param name="comByte">待转换字节数组</param>
        /// <returns>十六进制字符串</returns>
        public static string ByteToHex(byte[] comByte)
        {
            StringBuilder builder = new StringBuilder(comByte.Length * 3);
            foreach (byte data in comByte)
            {
                builder.Append(Convert.ToString(data, 16).PadLeft(2, '0').PadRight(3, ' '));
            }

            return builder.ToString().ToUpper();
        }
        #endregion

        /// <summary>
        /// 检查端口名称是否存在
        /// </summary>
        /// <param name="port_name"></param>
        /// <returns></returns>
        public static bool Exists(string port_name)
        {
            foreach (string port in SerialPort.GetPortNames()) if (port == port_name) return true;
            return false;
        }

        /// <summary>
        /// 格式化端口相关属性
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static string Format(SerialPort port)
        {
            return String.Format("{0} ({1},{2},{3},{4},{5})", 
                port.PortName, port.BaudRate, port.DataBits, port.StopBits, port.Parity, port.Handshake);
        }
    }
    
}
