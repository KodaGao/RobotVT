using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace SK_FModbus.SerialPort
{
    public class SerialPortMng
    {
        public SerialPortMng()
        {
            DataFormatType = SK_FModel.SystemEnum.DataFormatType.Byte;
            if (serialPort == null)
                serialPort = new System.IO.Ports.SerialPort();
            serialPort.DataReceived += SerialPort_DataReceived;
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        public event SK_FModel.SerialPortDelegate.DataReceivedEventHandler Event_DataReceived;

        /// <summary>
        /// 运行异常事件
        /// </summary>
        public event SK_FModel.SerialPortDelegate.RunExceptionEventHandler Event_RunException;

        /// <summary>
        ///发送指令事件
        /// </summary>
        public event SK_FModel.SerialPortDelegate.SenderOrderEventHandler Event_SenderOrder;

        /// <summary>
        /// 接收指令事件
        /// </summary>
        public event SK_FModel.SerialPortDelegate.ReceiveOrderEventHandler Event_ReceiveOrder;

        /// <summary>
        /// 发送指令间隔
        /// </summary>
        public int SendOrderInterval
        {
            set { sendOrderInterval = value; }
            get { return sendOrderInterval; }
        }

        /// <summary>
        /// 数据格式类型
        /// </summary>
        public SK_FModel.SystemEnum.DataFormatType DataFormatType
        {
            get { return dataFormatType; }
            set
            {
                dataFormatType = value;
                switch (dataFormatType)
                {
                    case SK_FModel.SystemEnum.DataFormatType.Byte:
                        if (SendOrderListByte == null)
                            SendOrderListByte = new Queue<byte[]>();
                        break;

                    case SK_FModel.SystemEnum.DataFormatType.String:
                        if (SendOrderListString == null)
                            SendOrderListString = new Queue<string>();
                        break;
                }
            }
        }

        private System.IO.Ports.SerialPort serialPort;
        private SK_FModel.SystemEnum.DataFormatType dataFormatType;

        /// <summary>
        /// 待发送指令队列
        /// </summary>
        private Queue<byte[]> SendOrderListByte;

        private Queue<string> SendOrderListString;

        /// <summary>
        /// 线程运行标志
        /// </summary>
        private bool ThreadRunFlag;

        /// <summary>
        /// 发送指令间隔，单位：ms
        /// </summary>
        private int sendOrderInterval = 10;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="SerialPortInfo"></param>
        public void Init(SK_FModel.SerialPortInfo SerialPortInfo)
        {
            try
            {
                serialPort.BaudRate = SerialPortInfo.BaudRate;
                serialPort.Parity = SerialPortInfo.Parity;
                serialPort.PortName = SerialPortInfo.PortName;
                serialPort.StopBits = SerialPortInfo.StopBits == StopBits.None ? StopBits.One : SerialPortInfo.StopBits;
                serialPort.ReceivedBytesThreshold = SerialPortInfo.ReceivedBytesThreshold;
                serialPort.DataBits = SerialPortInfo.DataBits;
                serialPort.RtsEnable = SerialPortInfo.RtsEnable;
                serialPort.DtrEnable = SerialPortInfo.DtrEnable;
                if (!string.IsNullOrWhiteSpace(SerialPortInfo.NewLine))
                    serialPort.NewLine = SerialPortInfo.NewLine;
            }
            catch (Exception ex)
            {
                throw new Exception("初始化串口信息失败，错误信息：" + ex.Message);
            }
        }

        public void UpdateReceivedBytesThreshold(int value)
        {
            if (serialPort == null || serialPort.ReceivedBytesThreshold == value) return;
            if(serialPort.ReceivedBytesThreshold != value)
                serialPort.ReceivedBytesThreshold = value;
        }

        public void DiscardOutBuffer()
        {
            if (serialPort != null)
                serialPort.DiscardOutBuffer();
        }

        public void DiscardInBuffer()
        {
            if (serialPort != null)
                serialPort.DiscardInBuffer();
        }

        public bool IsOpen
        {
            get { return serialPort == null ? false : serialPort.IsOpen; }
        }

        /// <summary>
        /// 打开
        /// </summary>
        public void Open()
        {
            try
            {
                if (serialPort == null)
                    throw new Exception("未初始化串口！");
                serialPort.Open();
                ThreadRunFlag = true;
                Thread _SendOrderTH = new Thread(new ThreadStart(Thread_SendOrder));
                _SendOrderTH.IsBackground = true;
                _SendOrderTH.Start();
            }
            catch (Exception ex)
            {
                throw new Exception("打开串口信息失败，错误信息：" + ex.Message);
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            try
            {
                if (serialPort == null)
                    throw new Exception("未初始化串口！");
                if (serialPort.IsOpen)
                {
                    ThreadRunFlag = false;
                    ClearSendOrder();
                    serialPort.DiscardInBuffer();
                    serialPort.DiscardOutBuffer();
                    serialPort.Close();
                    serialPort.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("关闭串口信息失败，错误信息：" + ex.Message);
            }
        }

        /// <summary>
        /// 添加一条指令到队列
        /// </summary>
        /// <param name="order"></param>
        public void AddSendOrder(object order)
        {
            switch (dataFormatType)
            {
                case SK_FModel.SystemEnum.DataFormatType.Byte:
                    if (!(order is byte[])) return;
                    byte[] _Order = (byte[])order;
                    if (_Order == null || _Order.Length <= 0) return;
                    SendOrderListByte.Enqueue(_Order);
                    break;

                case SK_FModel.SystemEnum.DataFormatType.String:
                    if (!(order is string)) return;
                    if (string.IsNullOrWhiteSpace(order.ToString())) return;
                    SendOrderListString.Enqueue(order.ToString());
                    break;
            }
        }

        /// <summary>
        /// 清空发送指令队列
        /// </summary>
        public void ClearSendOrder()
        {
            switch (dataFormatType)
            {
                case SK_FModel.SystemEnum.DataFormatType.Byte:
                    if (SendOrderListByte != null && SendOrderListByte.Count > 0)
                        SendOrderListByte.Clear();
                    break;

                case SK_FModel.SystemEnum.DataFormatType.String:
                    if (SendOrderListString != null && SendOrderListString.Count > 0)
                        SendOrderListString.Clear();
                    break;
            }
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                int _DataLen;
                switch (dataFormatType)
                {
                    case SK_FModel.SystemEnum.DataFormatType.Byte:
                        _DataLen = serialPort.BytesToRead;
                        if (_DataLen > 0)
                        {
                            byte[] _ReadDataByte = new byte[_DataLen];
                            serialPort.Read(_ReadDataByte, 0, _DataLen);
                            //if (Event_DataReceived != null)
                            //    Event_DataReceived(_ReadDataByte);
                        }
                        break;

                    case SK_FModel.SystemEnum.DataFormatType.String:
                        string _DataString = serialPort.ReadLine();
                        //if (Event_DataReceived != null)
                        //    Event_DataReceived(_DataString);
                        break;
                }
            }
            catch (Exception ex)
            {
                //if (Event_RunException != null)
                //    Event_RunException("接收数据失败，错误信息：" + ex.Message);
            }
        }

        /// <summary>
        /// 发送指令线程
        /// </summary>
        private void Thread_SendOrder()
        {
            byte[] _OrderByte = new byte[] { };
            string _OrderString;
            DateTime _LastSendOrderTime = DateTime.Now.AddDays(-1);
            TimeSpan _TS;
            while (ThreadRunFlag)
            {
                try
                {
                    _TS = DateTime.Now - _LastSendOrderTime;
                    switch (dataFormatType)
                    {
                        case SK_FModel.SystemEnum.DataFormatType.Byte:
                            if (serialPort.IsOpen && SendOrderListByte.Count > 0 && _TS.TotalMilliseconds >= sendOrderInterval)
                            {
                                _OrderByte = SendOrderListByte.Dequeue();
                                serialPort.Write(_OrderByte, 0, _OrderByte.Length);
                                _LastSendOrderTime = DateTime.Now;
                                if (Event_SenderOrder != null)
                                    Event_SenderOrder(serialPort.PortName,_OrderByte);
                            }
                            break;

                        case SK_FModel.SystemEnum.DataFormatType.String:
                            if (serialPort.IsOpen && SendOrderListString.Count > 0 && _TS.TotalMilliseconds >= sendOrderInterval)
                            {
                                _OrderString = SendOrderListString.Dequeue();
                                _OrderByte = ASCIIEncoding.Default.GetBytes(_OrderString);
                                serialPort.Write(_OrderByte, 0, _OrderByte.Length);
                                _LastSendOrderTime = DateTime.Now;
                                if (Event_SenderOrder != null)
                                    Event_SenderOrder(serialPort.PortName, _OrderByte);
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {
                    if (Event_RunException != null)
                        Event_RunException("发送指令失败，错误信息：" + ex.Message);
                }
                Thread.Sleep(10);
            }
        }
    }
}