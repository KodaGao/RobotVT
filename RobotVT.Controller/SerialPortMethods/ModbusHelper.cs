using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RobotVT.Controller.SerialPortMethods
{
    internal class ModbusHelper
    {
        private SK_FModbus.ModbusMng ModbusMng;

        internal ModbusHelper()
        {
        }

        private Queue<byte[]> ReceiveOrderList;
        private bool ThreadRunFlag;

        /// <summary>
        /// 运行消息事件
        /// </summary>
        internal event SK_FModel.SystemDelegate.del_RuningMessage Event_RuningMessage;
        /// Debug消息事件
        /// </summary>
        internal event SK_FModel.SystemDelegate.del_DebugMessage Event_DebugMessage;
        /// <summary>
        /// 更新实时数据事件
        /// </summary>
        internal event SK_FModel.SerialPortDelegate.UpdateRealTimeDataEventHandler Event_UpdateRealTimeData;

        /// <summary>
        /// 接收数据信息代理
        /// </summary>
        /// <param name="ReceiveData"></param>
        internal delegate void ReceiveDataEventHandler(SK_FModel.ReceiveData ReceiveData);
        /// <summary>
        /// 接收数据事件事件
        /// </summary>
        internal event ReceiveDataEventHandler Event_ReceiveData;

        /// <summary>
        /// 发送数据信息代理
        /// </summary>
        /// <param name="ReceiveData"></param>
        internal delegate void SendDataEventHandler(SK_FModel.ReceiveData ReceiveData);
        /// <summary>
        /// 发送数据事件事件
        /// </summary>
        internal event SendDataEventHandler Event_SendData;


        private SK_FModel.SerialPortInfo serialPortInfo;
        internal void Init(SK_FModel.SerialPortInfo SerialPortInfo)
        {
            serialPortInfo = SerialPortInfo;
            if (ModbusMng == null)
            {
                ModbusMng = new SK_FModbus.ModbusMng();
                ModbusMng.Event_DataReceived += ModbusMng_Event_DataReceived;
                ModbusMng.Event_RunException += ModbusMng_Event_RunException;
                ModbusMng.Event_SenderOrder += ModbusMng_Event_SenderOrder;
                ModbusMng.Event_ReceiveOrder += ModbusMng_Event_ReceiveOrder;
            }
            if (ModbusMng.IsOpen)
            {
                ModbusMng.ClearSendOrder();
                ModbusMng.Stop();
            }
            ModbusMng.Init(serialPortInfo);
            ModbusMng.CRC16CodeCheckType = SK_FModel.SerialPortEnum.CRC16CodeCheckType.CRCTable;
            ModbusMng.SenderOrderInterval = serialPortInfo.SenderOrderInterval;
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
        private byte[] CreateOrder(byte DeviceAddressId, SystemEnum.RegisterAddress RegisterAddress, SK_FModel.SerialPortEnum.ReadWriteType ReadWriteType, SK_FModel.SerialPortEnum.FunctionCode FunctionCode, short RegisterNum, byte[] value)
    {
        try
        {
            if (RegisterNum <= 0) throw new Exception("寄存器数量不能小于1！");
            byte[] _Result = null, _TempByte, _CRC16Byte = new byte[2];
                SK_FModbus.CRC16 _CRC16 = new SK_FModbus.CRC16();
                switch (ReadWriteType)
                {
                    case SK_FModel.SerialPortEnum.ReadWriteType.Read:

                        #region 读

                        _Result = new byte[8];
                        _Result[0] = DeviceAddressId;
                        _Result[1] = (byte)(int)FunctionCode;
                        _TempByte = BitConverter.GetBytes((short)RegisterAddress);
                        Array.Reverse(_TempByte);
                        _Result[2] = _TempByte[0];
                        _Result[3] = _TempByte[1];
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

                    case SK_FModel.SerialPortEnum.ReadWriteType.Write:

                        _Result = new byte[8];
                        _Result[0] = DeviceAddressId;
                        _Result[1] = 0x06;
                        _TempByte = BitConverter.GetBytes((short)RegisterAddress);
                        Array.Reverse(_TempByte);
                        _Result[2] = _TempByte[0];
                        _Result[3] = _TempByte[1];
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

        /// <summary>
        ///
        /// </summary>
        /// <param name="DeviceAddressId">设备地址码 默认：0x01</param>
        /// <param name="OrderType">指令类型</param>
        /// <param name="ReadWriteType">读写类型</param>
        /// <param name="RegisterNum">寄存器数量</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        private byte[] CreateReturnOrder(byte DeviceAddressId, SystemEnum.RegisterAddress RegisterAddress, SK_FModel.SerialPortEnum.FunctionCode FunctionCode, short RegisterNum, byte[] value)
        {
            try
            {
                if (RegisterNum <= 0) throw new Exception("寄存器数量不能小于1！");
                byte[] _Result = null, _TempByte, _CRC16Byte = new byte[2];
                SK_FModbus.CRC16 _CRC16 = new SK_FModbus.CRC16();

                switch (FunctionCode)
                {
                    case SK_FModel.SerialPortEnum.FunctionCode.Code03:
                        _Result = new byte[8];
                        _Result[0] = DeviceAddressId;
                        _Result[1] = 0x06;
                        _TempByte = BitConverter.GetBytes((short)RegisterAddress);
                        Array.Reverse(_TempByte);
                        _Result[2] = _TempByte[0];
                        _Result[3] = _TempByte[1];
                        _Result[4] = value[0];
                        _Result[5] = value[1];
                        _TempByte = new byte[_Result.Length - 2];
                        Array.Copy(_Result, _TempByte, _TempByte.Length);
                        _CRC16.GetCRC(_TempByte, ref _CRC16Byte);
                        _Result[6] = _CRC16Byte[0];
                        _Result[7] = _CRC16Byte[1];
                        break;
                    case SK_FModel.SerialPortEnum.FunctionCode.Code16:
                        _Result = new byte[8];
                        _Result[0] = DeviceAddressId;
                        _Result[1] = (byte)(int)FunctionCode;
                        _TempByte = BitConverter.GetBytes((short)RegisterAddress);
                        Array.Reverse(_TempByte);
                        _Result[2] = _TempByte[0];
                        _Result[3] = _TempByte[1];
                        _TempByte = BitConverter.GetBytes(RegisterNum);
                        Array.Reverse(_TempByte);
                        _Result[4] = _TempByte[0];
                        _Result[5] = _TempByte[1];
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



        internal void UpdateReceivedBytesThreshold(int value)
        {
            if (ModbusMng == null) return;
            ModbusMng.UpdateReceivedBytesThreshold(value);
        }

        /// <summary>
        /// 生成SendOrder对象
        /// </summary>
        /// <param name="ReadWriteType"></param>
        /// <param name="WriteSingle"></param>
        /// <param name="RegisterAddress"></param>
        /// <param name="RegisterNum"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal SendOrder CreateSendOrder(byte DeviceAddressId, SK_FModel.SerialPortEnum.ReadWriteType ReadWriteType, SK_FModel.SerialPortEnum.FunctionCode WriteSingle, SystemEnum.RegisterAddress RegisterAddress, short RegisterNum, List<byte> value)
        {
            SendOrder _SendOrder;
            try
            {
                _SendOrder = new SendOrder();
                _SendOrder.DeviceAddressId = DeviceAddressId;
                _SendOrder.ReadWriteType = ReadWriteType;
                _SendOrder.FunctionCode = WriteSingle;
                _SendOrder.RegisterAddress = RegisterAddress;
                if (_SendOrder.ReadWriteType == SK_FModel.SerialPortEnum.ReadWriteType.Read)
                {
                    #region 生成读指令对象

                    switch (RegisterAddress)
                    {
                        default:
                            _SendOrder.RegisterNum = RegisterNum;
                            _SendOrder.value = new List<byte>();
                            break;
                    }

                    #endregion 生成读指令对象
                }
                else if (_SendOrder.ReadWriteType == SK_FModel.SerialPortEnum.ReadWriteType.Write)
                {
                    switch (RegisterAddress)
                    {
                        default:
                            _SendOrder.RegisterNum = RegisterNum;
                            _SendOrder.value = value;
                            break;
                    }
                }

                return _SendOrder;
            }
            catch (Exception ex)
            {
                throw new Exception("生成指令对象失败，错误信息：" + ex.Message);
            }
        }

        /// <summary>
        /// 生成SendOrder对象
        /// </summary>
        /// <param name="ReadWriteType"></param>
        /// <param name="WriteSingle"></param>
        /// <param name="RegisterAddress"></param>
        /// <param name="RegisterNum"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal ReturnOrder CreateReturnOrder(byte DeviceAddressId, SK_FModel.SerialPortEnum.FunctionCode FunctionCode, short RegisterNum, List<byte> value)
        {
            ReturnOrder _ReturnOrder;
            try
            {
                _ReturnOrder = new ReturnOrder();
                _ReturnOrder.DeviceAddressId = DeviceAddressId;
                _ReturnOrder.FunctionCode = FunctionCode;
                switch (FunctionCode)
                {
                    case SK_FModel.SerialPortEnum.FunctionCode.Code03:
                        _ReturnOrder.RegisterNum = RegisterNum;
                        _ReturnOrder.value = new List<byte>();
                        break;

                    case SK_FModel.SerialPortEnum.FunctionCode.Code16:
                        _ReturnOrder.RegisterNum = RegisterNum;
                        _ReturnOrder.value = value;
                        break;
                }
                return _ReturnOrder;
            }
            catch (Exception ex)
            {
                throw new Exception("生成指令对象失败，错误信息：" + ex.Message);
            }
        }

        public void DiscardOutBuffer()
        {
            if (ModbusMng != null)
                ModbusMng.DiscardOutBuffer();
        }

        public void DiscardInBuffer()
        {
            if (ModbusMng != null)
                ModbusMng.DiscardInBuffer();
        }

        /// <summary>
        /// 提交指令
        /// </summary>
        /// <param name="OrderType"></param>
        /// <param name="OrderValue"></param>
        internal void SubmitOrder(SendOrder SendOrder)
        {
            if (!ModbusMng.IsOpen)
                throw new Exception("仪器未连接！");
            try
            {
                byte[] _OrderItem = CreateOrder(SendOrder.DeviceAddressId, SendOrder.RegisterAddress, SendOrder.ReadWriteType, SendOrder.FunctionCode, SendOrder.RegisterNum, SendOrder.value == null ? new byte[] { } : SendOrder.value.ToArray());
                ModbusMng.AddSendOrder(_OrderItem);
            }
            catch (Exception ex)
            {
                throw new Exception("提交指令失败，错误信息：" + ex.Message);
            }
        }
        internal void SubmitOrder(ReturnOrder SendOrder)
        {
            if (!ModbusMng.IsOpen)
                throw new Exception("仪器未连接！");
            try
            {
                byte[] _OrderItem = CreateOrder(SendOrder.DeviceAddressId, SendOrder.RegisterAddress, SendOrder.ReadWriteType, SendOrder.FunctionCode, SendOrder.RegisterNum, SendOrder.value == null ? new byte[] { } : SendOrder.value.ToArray());
                ModbusMng.AddSendOrder(_OrderItem);
            }
            catch (Exception ex)
            {
                throw new Exception("提交指令失败，错误信息：" + ex.Message);
            }
        }

        internal void ClearSendOrder()
        {
            if (ModbusMng != null)
                ModbusMng.ClearSendOrder();
        }

        private void ModbusMng_Event_ReceiveOrder(string PortName, byte[] Order)
        {
            if (Event_DebugMessage != null)
                Event_DebugMessage(null, "[" + PortName + "]" + "接收：" + BitConverter.ToString(Order).Replace("-", " "));
        }

        private void ModbusMng_Event_SenderOrder(string PortName, byte[] Order)
        {
            if (Event_DebugMessage != null)
                Event_DebugMessage(null, "[" + PortName + "]" + "发送：" + BitConverter.ToString(Order).Replace("-", " "));
        }

        /// <summary>
        /// 开始
        /// </summary>
        public void Start()
        {
            try
            {
                if (ModbusMng != null)
                    ModbusMng.Start();
                ThreadRunFlag = true;
                Thread _AnalysisDataTH = new Thread(new ThreadStart(Thread_AnalysisData));
                _AnalysisDataTH.IsBackground = true;
                _AnalysisDataTH.Start();
            }
            catch (Exception ex)
            {
                throw new Exception("启动[Modbus]失败，错误信息：" + ex.Message);
            }
        }

        /// <summary>
        /// 停止
        /// </summary>
        internal void Stop()
        {
            try
            {
                ThreadRunFlag = false;
                if (ModbusMng != null)
                    ModbusMng.Stop();
            }
            catch (Exception ex)
            {
                throw new Exception("停止[Modbus]失败，错误信息：" + ex.Message);
            }
        }

        private void Thread_AnalysisData()
        {
            byte[] _DataItem;
            int _ByteLen;
            SK_FModel.ReceiveData _ReceiveData;
            if (ReceiveOrderList == null)
                ReceiveOrderList = new Queue<byte[]>();
            while (ThreadRunFlag)
            {
                try
                {
                    if (ReceiveOrderList.Count > 0)
                    {
                        _DataItem = ReceiveOrderList.Dequeue();
                        if (_DataItem.Length >= 3)
                        {
                            _ReceiveData = new SK_FModel.ReceiveData();
                            _ReceiveData.DeviceAddressId = _DataItem[0];
                            _ReceiveData.FunctionCode = (SK_FModel.SerialPortEnum.FunctionCode)_DataItem[1];

                            byte[] _RegisterNumber = new byte[2];
                            _RegisterNumber[0] = _DataItem[5];
                            _RegisterNumber[1] = _DataItem[4];
                            _ReceiveData.Quantity = (byte)SK_FCommon.ValueHelper.Instance.GetShort(_RegisterNumber);

                            switch (_ReceiveData.FunctionCode)
                            {
                                case SK_FModel.SerialPortEnum.FunctionCode.Code03:
                                    _ByteLen = _DataItem[2];
                                    if (_DataItem.Length == _ByteLen + 5)
                                    {
                                        _ReceiveData.DataItem = new byte[_ByteLen];
                                        Array.Copy(_DataItem, 3, _ReceiveData.DataItem, 0, _ReceiveData.DataItem.Length);

                                        ClearSendOrder();
                                        Event_ReceiveData?.Invoke(_ReceiveData);
                                    }
                                    else
                                        throw new Exception("解析数据失败，错误信息：数据长度不正确！" + "\r\n" + BitConverter.ToString(_DataItem).Replace("-", " "));
                                    break;

                                case SK_FModel.SerialPortEnum.FunctionCode.Code16:
                                    _ByteLen = _DataItem[6];
                                    if (_DataItem.Length == _ByteLen + 9)
                                    {
                                        _ReceiveData.DataItem = new byte[_ByteLen];
                                        Array.Copy(_DataItem, 7, _ReceiveData.DataItem, 0, _ReceiveData.DataItem.Length);
                                        Event_ReceiveData?.Invoke(_ReceiveData);
                                        Event_UpdateRealTimeData?.Invoke(_ReceiveData);
                                    }
                                    else
                                        throw new Exception("解析数据失败，错误信息：数据长度不正确！" + "\r\n" + BitConverter.ToString(_DataItem).Replace("-", " "));
                                    break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Event_RuningMessage?.Invoke(StaticInfo.ControlObject, "解析数据失败，错误信息：" + ex.Message, SK_FModel.SystemEnum.MessageType.Exception);
                }
                Thread.Sleep(100);
            }
        }

        private void ModbusMng_Event_RunException(object ErrorInfo)
        {
            Methods.SaveExceptionLog(SK_FModel.SystemEnum.LogType.Exception, ErrorInfo.ToString());            
            Event_RuningMessage?.Invoke(StaticInfo.ControlObject, ErrorInfo.ToString(), SK_FModel.SystemEnum.MessageType.Exception);
        }

        private void ModbusMng_Event_DataReceived(object DataItem)
        {
            if (ReceiveOrderList == null)
                ReceiveOrderList = new Queue<byte[]>();
            ReceiveOrderList.Enqueue((byte[])DataItem);
        }
    }
}
