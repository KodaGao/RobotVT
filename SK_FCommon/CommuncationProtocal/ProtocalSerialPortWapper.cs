using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using SK_FModel;
using System.Threading;

namespace SK_FCommon
{
    internal class ProtocalSerialPortWapper : ProtocalWapper, IDisposable
    {
        /// <summary>
        /// 接收事件是否有效 false表示有效
        /// </summary>
        public bool ReceiveEventFlag = false;
        ////<summary>
        ////完整协议的记录处理事件
        ////</summary>
        public override event DataReceivedEventHandler DataReceived;
        public override event SerialErrorReceivedEventHandler Error;

        private SerialPort comPort = new SerialPort();
        private ModbusRTUControl rtu_modbus = new ModbusRTUControl();

        public override void Connect()
        {
            if (comPort.IsOpen) comPort.Close();

            comPort.PortName = ((SerialModel)ConnectObject).serialport;
            comPort.BaudRate = (int)((SerialModel)ConnectObject).baudrate;
            comPort.Parity = ((SerialModel)ConnectObject).parity;
            comPort.DataBits = (int)((SerialModel)ConnectObject).databit;
            comPort.StopBits = ((SerialModel)ConnectObject).stopbit;
            comPort.ReadTimeout = ((SerialModel)ConnectObject).timeout;
            comPort.WriteTimeout = ((SerialModel)ConnectObject).timeout;


            comPort.Open();
            comPort.RtsEnable = true;
            comPort.DtrEnable = true;


            //comPort.DataReceived += new SerialDataReceivedEventHandler(Connect_DataReceived);
            //comPort.ErrorReceived += new SerialErrorReceivedEventHandler(Connect_ErrorReceived);
        }
        public override void DisConnect()
        {
            if (comPort.IsOpen) comPort.Close();
        }
        public override int WriteData(byte[] SendData, ref  byte[] ReceiveData, int Overtime)
        {
            if (!(comPort.IsOpen)) comPort.Open();

            //ReceiveEventFlag = true;        //关闭接收事件
            //comPort.DiscardInBuffer();      //清空接收缓冲区                 
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
                    return ret;
                ret = comPort.Read(ReceiveData, 0, ReceiveData.Length);
            }

            //ReceiveEventFlag = false;       //打开事件

            //字符转换
            string readString = ByteToHex(ReceiveData);
            string readtimeString = string.Format("{0:HH:mm:ss:ffff}", DateTime.Now);

            //触发整条记录的处理
            if (DataReceived != null)
            {
                DataReceived(new DataReceivedEventArgs("", "", readString, readtimeString));
            }

            return ret;
        }
        public override object WriteData(byte[] SendData, byte[] ReceiveData, int Overtime)
        {
            if (!(comPort.IsOpen)) comPort.Open();

            SerialPortReceiveData rd = new SerialPortReceiveData();

            try
            {
                //ReceiveEventFlag = true;        //关闭接收事件
                comPort.DiscardInBuffer();      //清空接收缓冲区  
                comPort.Write(SendData, 0, SendData.Length);
                Thread.Sleep(670);

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
                if (!rd.ModbusResponse)
                    rd.SlaveID = SendData[0];
                //ReceiveEventFlag = false;       //打开事件

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

        public override object WriteData(byte[] SendData, byte[] ReceiveData, int Overtime,int Delay)
        {
            if (!(comPort.IsOpen)) comPort.Open();

            SerialPortReceiveData rd = new SerialPortReceiveData();

            try
            {
                //ReceiveEventFlag = true;        //关闭接收事件
                comPort.DiscardInBuffer();      //清空接收缓冲区  
                comPort.Write(SendData, 0, SendData.Length);
                Thread.Sleep(Delay);

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
                if (!rd.ModbusResponse)
                    rd.SlaveID = SendData[0];
                //ReceiveEventFlag = false;       //打开事件

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
        public override void DiscardBuffer()
        {
            comPort.DiscardInBuffer();
            comPort.DiscardOutBuffer();
        }
        public override bool IsConnect()
        {
            return comPort.IsOpen;
        }


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

        public override void Connect_DataReceived(object sender, SerialDataReceivedEventArgs e)
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
            catch (Exception ex)
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

            ////字符转换
            //byte[] byteData = _byteData.ToArray();
            //string readString = ByteToHex(byteData);
            //string readtimeString = string.Format("{0:HH:mm:ss:ffff}", DateTime.Now);

            ////触发整条记录的处理
            //if (DataReceived != null)
            //{
            //    DataReceived(new DataReceivedEventArgs("", "", readString, readtimeString));
            //}
        }

        public override void Connect_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            //throw new NotImplementedException();
        }
    }
}

