using System;
using System.Collections.Generic;
using System.Threading;

namespace SK_FModbus
{
    public class ModbusMng
    {
        public ModbusMng()
        {
            cRC16CodeCheckType = SK_FModel.SerialPortEnum.CRC16CodeCheckType.CRCTable;
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        public event SK_FModel.SerialPortDelegate.del_DataReceived Event_DataReceived;

        /// <summary>
        /// 异常运行事件
        /// </summary>
        public event SK_FModel.SerialPortDelegate.del_RunException Event_RunException;

        /// <summary>
        ///发送指令事件
        /// </summary>
        public event SK_FModel.SerialPortDelegate.del_SenderOrder Event_SenderOrder;

        /// <summary>
        /// 接收指令事件
        /// </summary>
        public event SK_FModel.SerialPortDelegate.del_ReceiveOrder Event_ReceiveOrder;

        private List<byte> ReceiveDataList;
        private System.IO.Ports.SerialPort SerialPortMng;
        private DateTime ReceiveDataTime;
        private bool ThreadRunFlag;
        private SK_FModel.SerialPortEnum.CRC16CodeCheckType cRC16CodeCheckType;
        object ReceiveDataListLock;
        private SK_FModel.SerialPortInfo serialPortInfo;
        int ReceiveOrderLen;

        /// <summary>
        /// 待发送指令队列
        /// </summary>
        private Queue<byte[]> SendOrderListByte;

        /// <summary>
        /// 指令间隔，单位：ms
        /// </summary>
        public int ReceiveOrderInterval
        {
            set { orderInterval = value; }
        }

        public SK_FModel.SerialPortEnum.CRC16CodeCheckType CRC16CodeCheckType
        {
            get { return cRC16CodeCheckType; }
            set { cRC16CodeCheckType = value; }
        }

        /// <summary>
        /// 指令间隔，单位：ms
        /// </summary>
        private int orderInterval;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="SerialPortInfo"></param>
        public void Init(SK_FModel.SerialPortInfo SerialPortInfo)
        {
            try
            {
                serialPortInfo = SerialPortInfo;
                if (SendOrderListByte == null)
                    SendOrderListByte = new Queue<byte[]>();
                if (ReceiveDataList == null)
                    ReceiveDataList = new List<byte>();
                ReceiveDataList.Clear();
                if (SerialPortMng == null)
                {
                    SerialPortMng = new System.IO.Ports.SerialPort();
                    SerialPortMng.BaudRate = SerialPortInfo.BaudRate;
                    SerialPortMng.Parity = SerialPortInfo.Parity;
                    SerialPortMng.PortName = SerialPortInfo.PortName;
                    SerialPortMng.StopBits = SerialPortInfo.StopBits == System.IO.Ports.StopBits.None ? System.IO.Ports.StopBits.One : SerialPortInfo.StopBits;
                    SerialPortMng.ReceivedBytesThreshold = SerialPortInfo.ReceivedBytesThreshold;
                    SerialPortMng.DataBits = SerialPortInfo.DataBits;
                    SerialPortMng.RtsEnable = SerialPortInfo.RtsEnable;
                    SerialPortMng.DtrEnable = SerialPortInfo.DtrEnable;
                    SerialPortMng.DataReceived += SerialPortMng_DataReceived;
                }
                ReceiveDataTime = DateTime.Now.AddDays(1);
                ReceiveDataListLock = "ReceiveDataListLock";
            }
            catch (Exception ex)
            {
                throw new Exception("初始化串口信息失败，错误信息：" + ex.Message);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="DeviceAddressId">设备地址码 默认：0x01</param>
        /// <param name="OrderType">指令类型</param>
        /// <param name="ReadWriteType">读写类型</param>
        /// <param name="RegisterNum">寄存器数量</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public byte[] CreateOrder(byte AddressId,byte FunctionCode, byte[] RegisterAddress, short RegisterNum, byte[] value)
        {
            try
            {
                if (RegisterNum <= 0) throw new Exception("寄存器数量不能小于1！");
                byte[] _Result = null, _TempByte, _CRC16Byte = new byte[2];
                CRC16 _CRC16 = new CRC16();
                switch (FunctionCode)
                {
                    case 03:
                    case 04:

                        #region 读

                        _Result = new byte[8];
                        _Result[0] = AddressId;
                        _Result[1] = FunctionCode;                     
                        _Result[2] = RegisterAddress[0];
                        _Result[3] = RegisterAddress[1];
                        _TempByte = BitConverter.GetBytes(RegisterNum);
                        Array.Reverse(_TempByte);
                        _Result[4] = _TempByte[0];
                        _Result[5] = _TempByte[1];
                        _TempByte = new byte[_Result.Length - 2];
                        Array.Copy(_Result, _TempByte, _TempByte.Length);
                        _CRC16.GetCRC(_TempByte, ref _CRC16Byte);
                        _Result[6] = _CRC16Byte[0];
                        _Result[7] = _CRC16Byte[1];

                        #endregion 读

                        break;

                    case 05:
                    case 06:
                    case 15:
                    case 16:

                        _Result = new byte[8];
                        _Result[0] = AddressId;
                        _Result[1] = FunctionCode;
                        _Result[2] = RegisterAddress[0];
                        _Result[3] = RegisterAddress[1];
                        _Result[4] = value[0];
                        _Result[5] = value[1];
                        _TempByte = new byte[_Result.Length - 2];
                        Array.Copy(_Result, _TempByte, _TempByte.Length);
                        _CRC16.GetCRC(_TempByte, ref _CRC16Byte);
                        _Result[6] = _CRC16Byte[0];
                        _Result[7] = _CRC16Byte[1];

                        break;
                }
                return _Result;
            }
            catch (Exception ex)
            {
                throw new Exception("生成指令数据失败，错误信息：" + ex.Message);
            }
        }

        private void SerialPortMng_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            try
            {
                int _DataLen = SerialPortMng.BytesToRead;
                if (_DataLen > 0)
                {
                    byte[] _ReadDataByte = new byte[_DataLen];
                    SerialPortMng.Read(_ReadDataByte, 0, _DataLen);
                    lock (ReceiveDataListLock)
                    {
                        ReceiveDataList.AddRange(_ReadDataByte);
                    }
                    ReceiveDataTime = DateTime.Now;
                    if (Event_ReceiveOrder != null)
                        Event_ReceiveOrder(serialPortInfo.PortName,_ReadDataByte);
                }               
            }
            catch (Exception ex)
            {
                if (Event_RunException != null)
                    Event_RunException("[" + (serialPortInfo == null ? "" : serialPortInfo.PortName) + "]Modbus接收数据失败，错误信息：" + ex.Message);
            }
        }

        public void UpdateReceivedBytesThreshold(int value)
        {
            if (SerialPortMng == null || serialPortInfo == null || serialPortInfo.ReceivedBytesThreshold == value) return;
            serialPortInfo.ReceivedBytesThreshold = value;
            SerialPortMng.ReceivedBytesThreshold = serialPortInfo.ReceivedBytesThreshold;
        }

        /// <summary>
        /// 清除指令队列
        /// </summary>
        public void ClearSendOrder()
        {
            if (SendOrderListByte != null)
                SendOrderListByte.Clear();
        }

        private void SerialPortMng_Event_RunException(object ErrorInfo)
        {
            if (Event_RunException != null)
                Event_RunException(ErrorInfo);
        }

        private void SerialPortMng_Event_SenderOrder(string PortName, byte[] Message)
        {
            if (Event_SenderOrder != null)
                Event_SenderOrder(PortName, Message);
        }

        /// <summary>
        /// 发送指令线程
        /// </summary>
        private void Thread_SendOrder()
        {
            byte[] _OrderByte = new byte[] { };
            DateTime _LastSendOrderTime = DateTime.Now.AddDays(-1);
            TimeSpan _TS;
            while (ThreadRunFlag)
            {
                try
                {
                    _TS = DateTime.Now - _LastSendOrderTime;
                    if (SerialPortMng != null && SerialPortMng.IsOpen && SendOrderListByte.Count > 0 && _TS.TotalMilliseconds >= serialPortInfo.SenderOrderInterval)
                    {
                        //SerialPortMng.DiscardInBuffer();
                        //SerialPortMng.DiscardOutBuffer();
                        _OrderByte = SendOrderListByte.Dequeue();
                        ReceiveOrderLen = GetOrderLen(_OrderByte);
                        UpdateReceivedBytesThreshold(1);
                        SerialPortMng.Write(_OrderByte, 0, _OrderByte.Length);
                        _LastSendOrderTime = DateTime.Now;
                        if (Event_SenderOrder != null)
                            Event_SenderOrder(serialPortInfo.PortName, _OrderByte);
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

        int GetOrderLen(byte[] _OrderByte)
        {
            int _ReceiveOrderLen;
            if (_OrderByte.Length < 6)
                _ReceiveOrderLen = 0;
            else
            {
                switch (_OrderByte[1])
                {
                    default:
                    case 0x01:
                    case 0x03:
                    case 0x04:
                        _ReceiveOrderLen = ModbusHelper.ModbusToTnt16(new byte[] { _OrderByte[4], _OrderByte[5] }) * 2 + 5;
                        break;
                    case 0x06:
                    case 0x16:
                    case 16:
                    case 15:
                        _ReceiveOrderLen = 8;
                        break;
                }
            }
            return _ReceiveOrderLen;
        }

        public void AddSendOrder(byte[] OrderItem)
        {
            if (SerialPortMng == null || !SerialPortMng.IsOpen) return;
            SendOrderListByte.Enqueue(OrderItem);
        }

        public int SenderOrderInterval
        {
            get
            {
                return serialPortInfo != null ? serialPortInfo.SenderOrderInterval : -1;
            }
            set
            {
                if (serialPortInfo != null)
                    serialPortInfo.SenderOrderInterval = value;
            }
        }

        public bool IsOpen
        {
            get
            {
                if (SerialPortMng == null)
                    return false;
                else
                    return SerialPortMng.IsOpen;
            }
        }

        public void Start()
        {
            try
            {
                SerialPortMng.Open();
                ThreadRunFlag = true;
                Thread _SendOrderTH = new Thread(new ThreadStart(Thread_SendOrder));
                _SendOrderTH.IsBackground = true;
                _SendOrderTH.Start();

                Thread _AnalysisDataTH = new Thread(new ThreadStart(Thread_AnalysisData));
                _AnalysisDataTH.IsBackground = true;
                _AnalysisDataTH.Start();
            }
            catch (Exception ex)
            {
                throw new Exception("启动Modbus失败，错误信息：" + ex.Message);
            }
        }

        public void Stop()
        {
            try
            {
                ThreadRunFlag = false;
                if (SerialPortMng != null)
                {
                    SerialPortMng.DiscardInBuffer();
                    SerialPortMng.DiscardOutBuffer();
                    SerialPortMng.Close();
                    SerialPortMng.Dispose();
                }
                ReceiveDataList.Clear();
            }
            catch (Exception ex)
            {
                throw new Exception("停止Modbus失败，错误信息：" + ex.Message);
            }
        }

        /// <summary>
        /// 清除传输缓冲区中的数据
        /// </summary>
        public void DiscardOutBuffer()
        {
            if (SerialPortMng != null && SerialPortMng.IsOpen)
                SerialPortMng.DiscardOutBuffer();
        }

        /// <summary>
        /// 清除接收缓冲区中的数据
        /// </summary>
        public void DiscardInBuffer()
        {
            if (SerialPortMng != null && SerialPortMng.IsOpen)
                SerialPortMng.DiscardInBuffer();
        }

        private void Thread_AnalysisData()
        {
            byte[] _DataItem, _CRCResult;
            CRC16 _CRC16 = new CRC16();
            CRC16Table _CRC16Table = new CRC16Table();
            bool _Result = false;
            _CRCResult = new byte[2];
            int _DataLen;
            byte _FunctionCode;
            while (ThreadRunFlag)
            {
                try
                {
                    if (ReceiveDataList.Count >= 1 && ReceiveDataList.Count >= ReceiveOrderLen)// serialPortInfo.ReceivedBytesThreshold)
                    {
                        _FunctionCode = ReceiveDataList[1];
                        switch (_FunctionCode)
                        {
                            case 0x01:
                            case 0x03:
                            case 0x04:
                                _DataItem = new byte[ReceiveOrderLen];
                                ReceiveDataList.CopyTo(0, _DataItem, 0, _DataItem.Length);
                                ReceiveDataList.RemoveRange(0, _DataItem.Length);
                                if (cRC16CodeCheckType == SK_FModel.SerialPortEnum.CRC16CodeCheckType.CRC16)
                                    _Result = _CRC16.CheckResponse(_DataItem, ref _CRCResult);
                                else
                                    _Result = _CRC16Table.CheckResponse(_DataItem, ref _CRCResult);
                                if (_Result && Event_DataReceived != null)
                                    Event_DataReceived(_DataItem);
                                else if (!_Result && Event_RunException != null)
                                {
                                    ReceiveDataList.Clear();
                                    DiscardInBuffer();
                                    Event_RunException("[" + (serialPortInfo == null ? "" : serialPortInfo.PortName) + "]Modbus接收数据失败，错误信息：校验失败！" + BitConverter.ToString(_DataItem).Replace("-", " ") + " 结果：" + BitConverter.ToString(_CRCResult).Replace("-", " "));
                                }
                                break;

                            case 0x06:
                            case 16:
                            case 15:
                            case 0x16:
                                _DataLen = 8;
                                if (ReceiveDataList.Count >= _DataLen)
                                {
                                    _DataItem = new byte[_DataLen];
                                    lock (ReceiveDataListLock)
                                    {
                                        ReceiveDataList.CopyTo(0, _DataItem, 0, _DataItem.Length);
                                        ReceiveDataList.RemoveRange(0, _DataItem.Length);
                                    }
                                    if (cRC16CodeCheckType == SK_FModel.SerialPortEnum.CRC16CodeCheckType.CRC16)
                                        _Result = _CRC16.CheckResponse(_DataItem, ref _CRCResult);
                                    else
                                        _Result = _CRC16Table.CheckResponse(_DataItem, ref _CRCResult);

                                    if (_Result && Event_DataReceived != null)
                                        Event_DataReceived(_DataItem);
                                    else if (!_Result && Event_RunException != null)
                                    {
                                        ReceiveDataList.Clear();
                                        DiscardInBuffer();
                                        Event_RunException("[" + (serialPortInfo == null ? "" : serialPortInfo.PortName) + "]Modbus接收数据失败，错误信息：校验失败！" + BitConverter.ToString(_DataItem).Replace("-", " ") + " 结果：" + BitConverter.ToString(_CRCResult).Replace("-", " "));
                                    }
                                }
                                break;
                            default:
                                ReceiveDataList.Clear();
                                DiscardInBuffer();
                                break;
                        }
                    }

                }
                catch (Exception ex)
                {
                    ReceiveDataList.Clear();
                    DiscardInBuffer();
                    Event_RunException("[" + (serialPortInfo == null ? "" : serialPortInfo.PortName) + "Modbus]接收数据失败，错误信息：！" + ex.Message);
                }
                Thread.Sleep(1);
            }
        }

        private void SerialPortMng_Event_DataReceived(string PortName, object DataItem)
        {
            try
            {
                byte[] _DataItem = (byte[])DataItem;
                ReceiveDataList.AddRange(_DataItem);
                ReceiveDataTime = DateTime.Now;
                if (Event_ReceiveOrder != null)
                    Event_ReceiveOrder(PortName, _DataItem);
            }
            catch (Exception ex)
            {
                if (Event_RunException != null)
                    Event_RunException("[" + (serialPortInfo == null ? "" : serialPortInfo.PortName) + "Modbus]接收数据失败，错误信息：" + ex.Message);
            }
        }
    }
}