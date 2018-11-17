using System;
using System.Collections.Generic;
using System.IO.Ports;
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
        private SerialPort.SerialPortMng SerialPortMng;
        private DateTime ReceiveDataTime;
        private bool ThreadRunFlag;
        private SK_FModel.SerialPortEnum.CRC16CodeCheckType cRC16CodeCheckType;
        object ReceiveDataListLock;
        private SK_FModel.SerialPortInfo serialPortInfo;

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
                if (ReceiveDataList == null)
                    ReceiveDataList = new List<byte>();
                ReceiveDataList.Clear();
                if (SerialPortMng == null)
                {
                    SerialPortMng = new SerialPort.SerialPortMng();
                    SerialPortMng.Event_DataReceived += SerialPortMng_Event_DataReceived;
                    SerialPortMng.Event_SenderOrder += SerialPortMng_Event_SenderOrder;
                    SerialPortMng.Event_RunException += SerialPortMng_Event_RunException;
                }
                SerialPortMng.Init(serialPortInfo);
                ReceiveDataTime = DateTime.Now.AddDays(1);
                ReceiveDataListLock = "ReceiveDataListLock";
            }
            catch (Exception ex)
            {
                throw new Exception("初始化串口信息失败，错误信息：" + ex.Message);
            }
        }

        public void UpdateReceivedBytesThreshold(int value)
        {
            if (SerialPortMng == null) return;
            serialPortInfo.ReceivedBytesThreshold = value;
            SerialPortMng.UpdateReceivedBytesThreshold(serialPortInfo.ReceivedBytesThreshold);
        }

        /// <summary>
        /// 清除指令队列
        /// </summary>
        public void ClearSendOrder()
        {
            if (SerialPortMng != null)
                SerialPortMng.ClearSendOrder();
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

        public void AddSendOrder(byte[] OrderItem)
        {
            if (SerialPortMng == null || !SerialPortMng.IsOpen) return;
            SerialPortMng.AddSendOrder(OrderItem);
        }

        public int SenderOrderInterval
        {
            get
            {
                return SerialPortMng != null ? SerialPortMng.SendOrderInterval : -1;
            }
            set
            {
                if (SerialPortMng != null)
                    SerialPortMng.SendOrderInterval = value;
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
                    SerialPortMng.Close();
                ReceiveDataList.Clear();
            }
            catch (Exception ex)
            {
                throw new Exception("停止Modbus失败，错误信息：" + ex.Message);
            }
        }

        public void DiscardOutBuffer()
        {
            if (SerialPortMng != null)
                SerialPortMng.DiscardOutBuffer();
        }

        public void DiscardInBuffer()
        {
            if (SerialPortMng != null)
                SerialPortMng.DiscardInBuffer();
        }

        private void Thread_AnalysisData()
        {
            //TimeSpan _TS;
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
                    #region 根据寄存器数量判断报文是否发送完成

                    if (ReceiveDataList.Count > 3 && ReceiveDataList.Count >= serialPortInfo.ReceivedBytesThreshold)
                    {
                        _FunctionCode = ReceiveDataList[1];
                        switch (_FunctionCode)
                        {
                            case 0x01:
                            case 0x03:
                            case 0x04:
                                _DataLen = ReceiveDataList[2] + 5;
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
                                        Event_RunException("[" + (serialPortInfo == null ? "" : serialPortInfo.PortName) + "Modbus]接收数据失败，错误信息：校验失败！" + BitConverter.ToString(_DataItem).Replace("-", " ") + " 结果：" + BitConverter.ToString(_CRCResult).Replace("-", " "));
                                }
                                break;

                            case 0x06:
                            case 16:
                            case 15:
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
                                        Event_RunException("[" + (serialPortInfo == null ? "" : serialPortInfo.PortName) + "Modbus]接收数据失败，错误信息：校验失败！" + BitConverter.ToString(_DataItem).Replace("-", " ") + " 结果：" + BitConverter.ToString(_CRCResult).Replace("-", " "));
                                }
                                break;
                        }
                    }

                    #endregion 根据寄存器数量判断报文是否发送完成

                    #region 通过指令反馈时间来判断报文是否发送完成

                    //_TS = DateTime.Now - ReceiveDataTime;
                    //if (_TS.TotalMilliseconds >= orderInterval && ReceiveDataList.Count > 0)
                    //{
                    //    lock (ReceiveDataList)
                    //    {
                    //        _DataItem = ReceiveDataList.ToArray();
                    //        if (cRC16CodeCheckType == Model.CommonEnum.CRC16CodeCheckType.CRC16)
                    //            _Result = _CRC16.CheckResponse(_DataItem, ref _CRCResult);
                    //        else
                    //            _Result = _CRC16Table.CheckResponse(_DataItem, ref _CRCResult);
                    //        ReceiveDataList.Clear();
                    //        ReceiveDataTime = DateTime.Now;
                    //    }
                    //    if (_Result && Event_DataReceived != null)
                    //        Event_DataReceived(_DataItem);
                    //    else if (!_Result && Event_RunException != null)
                    //        //ThreadPool.QueueUserWorkItem(new WaitCallback(Event_RunException), new object[] { "[Modbus]接收数据失败，错误信息：校验失败！" + BitConverter.ToString(_DataItem).Replace("-", " ") });
                    //        Event_RunException("[Modbus]接收数据失败，错误信息：校验失败！" + BitConverter.ToString(_DataItem).Replace("-", " ") + " 结果：" + BitConverter.ToString(_CRCResult).Replace("-", " "));
                    //}

                    #endregion 通过指令反馈时间来判断报文是否发送完成
                }
                catch (Exception ex)
                {
                    Event_RunException("[" + (serialPortInfo == null ? "" : serialPortInfo.PortName) + "Modbus]接收数据失败，错误信息：！" + ex.Message);
                }
                Thread.Sleep(1);
            }
        }

        private void SerialPortMng_Event_DataReceived(object DataItem)
        {
            try
            {
                switch (SerialPortMng.DataFormatType)
                {
                    case SK_FModel.SystemEnum.DataFormatType.Byte:
                        byte[] _DataItem = (byte[])DataItem;
                        lock (ReceiveDataListLock)
                        {
                            ReceiveDataList.AddRange(_DataItem);
                        }
                        ReceiveDataTime = DateTime.Now;
                        if (Event_ReceiveOrder != null)
                            Event_ReceiveOrder(serialPortInfo.PortName,_DataItem);
                        break;

                    case SK_FModel.SystemEnum.DataFormatType.String:
                        break;
                }
            }
            catch (Exception ex)
            {
                if (Event_RunException != null)
                    Event_RunException("[" + (serialPortInfo == null ? "" : serialPortInfo.PortName) + "Modbus]接收数据失败，错误信息：" + ex.Message);
            }
        }
    }
}