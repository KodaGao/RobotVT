using System;
using System.Collections.Generic;
using static SK_FModel.SerialPortEnum;

namespace SK_FCommon
{

    /// <summary>
    /// modbus格式化类型
    /// </summary>
    public enum FormatType : byte
    {
        ABCD,
        CDAB,
        BADC,
        DCBA
    }

    public class ModbusRTUControl
    {
        #region CRC Computation
        private void GetCRC(byte[] message, ref byte[] CRC)
        {
            //Function expects a modbus message of any length as well as a 2 byte CRC array in which to 
            //return the CRC values:

            ushort CRCFull = 0xFFFF;
            byte CRCHigh = 0xFF, CRCLow = 0xFF;
            char CRCLSB;

            for (int i = 0; i < (message.Length); i++)
            {
                CRCFull = (ushort)(CRCFull ^ message[i]);

                for (int j = 0; j < 8; j++)
                {
                    CRCLSB = (char)(CRCFull & 0x0001);
                    CRCFull = (ushort)((CRCFull >> 1) & 0x7FFF);

                    if (CRCLSB == 1)
                        CRCFull = (ushort)(CRCFull ^ 0xA001);
                }
            }
            CRC[1] = CRCHigh = (byte)((CRCFull >> 8) & 0xFF);
            CRC[0] = CRCLow = (byte)(CRCFull & 0xFF);
        }
        private void CheckCRC(byte[] message, ref byte[] CRC)
        {
            //Function expects a modbus message of any length as well as a 2 byte CRC array in which to 
            //return the CRC values:

            ushort CRCFull = 0xFFFF;
            byte CRCHigh = 0xFF, CRCLow = 0xFF;
            char CRCLSB;

            for (int i = 0; i < (message.Length - 2); i++)
            {
                CRCFull = (ushort)(CRCFull ^ message[i]);

                for (int j = 0; j < 8; j++)
                {
                    CRCLSB = (char)(CRCFull & 0x0001);
                    CRCFull = (ushort)((CRCFull >> 1) & 0x7FFF);

                    if (CRCLSB == 1)
                        CRCFull = (ushort)(CRCFull ^ 0xA001);
                }
            }
            CRC[1] = CRCHigh = (byte)((CRCFull >> 8) & 0xFF);
            CRC[0] = CRCLow = (byte)(CRCFull & 0xFF);
        }
        #endregion

        #region Check Response
        public bool CheckResponse(byte[] response)
        {
            //Perform a basic CRC check:
            bool breturn = false;
            byte[] CRC = new byte[2];
            CheckCRC(response, ref CRC);
            breturn = (CRC[0] == response[response.Length - 2] && CRC[1] == response[response.Length - 1]) ? true : false;
            return breturn;
        }
        #endregion
        
        #region Modbus标准协议-01:Read Coils
        /// <summary>
        /// Modbus标准协议-01:Read Coils
        /// </summary>
        /// <param name="Id">byte</param>
        /// <param name="Address">short</param>
        /// <param name="Quantity">short</param>
        /// <returns>Modbus Function 数据包</returns>
        /// 
        public byte[] ModbusFc1(byte Id, short Address, short Quantity)
        {
            List<byte> sendData = new List<byte>(255);
            //[1].Send

            sendData.Add((byte)Id);//1:Unit Identifier:This field is used for intra-system routing purpose.
            sendData.Add((byte)FunctionCode.Code01);//8.Function Code : 1 (Read Coils)
            sendData.AddRange(ValueHelper.Instance.GetBytes((short)Address));//3~4.起始地址
            sendData.AddRange(ValueHelper.Instance.GetBytes((short)Quantity));//5~6.需要写入的寄存器数量
            //sendData.AddRange(ValueHelper.Instance.GetBytes((short)value));//7~8.需要写入寄存器的值

            byte[] CRC = new byte[2];
            GetCRC(sendData.ToArray(), ref CRC);
            sendData.AddRange(CRC);//9~10.CRC
            //sendData.AddRange(ValueHelper.Instance.GetBytes((short)0));//9~10.CRC

            return sendData.ToArray();
        }
        #endregion

        #region Modbus标准协议-02:Read Discrete Inputs
        /// <summary>
        /// Modbus标准协议-02:Read Discrete Inputs
        /// </summary>
        /// <param name="Id">byte</param>
        /// <param name="Address">short</param>
        /// <param name="Quantity">short</param>
        /// <returns>Modbus Function 数据包</returns>
        /// 
        public byte[] ModbusFc2(byte Id, short Address, short Quantity)
        {
            List<byte> sendData = new List<byte>(255);
            //[1].Send

            sendData.Add((byte)Id);//1:Unit Identifier:This field is used for intra-system routing purpose.
            sendData.Add((byte)FunctionCode.Code02);//8.Function Code : 2 (Read Discrete Inputs)
            sendData.AddRange(ValueHelper.Instance.GetBytes((short)Address));//3~4.起始地址
            sendData.AddRange(ValueHelper.Instance.GetBytes((short)Quantity));//5~6.需要写入的寄存器数量

            byte[] CRC = new byte[2];
            GetCRC(sendData.ToArray(), ref CRC);
            sendData.AddRange(CRC);//9~10.CRC

            return sendData.ToArray();
        }
        #endregion
        
        #region Modbus标准协议-03:Read Holding Registers
        /// <summary>
        /// Modbus标准协议-03:Read Holding Registers
        /// </summary>
        /// <param name="Id">byte</param>
        /// <param name="Address">short</param>
        /// <param name="Quantity">short</param>
        /// <returns>Modbus Function 数据包</returns>
        /// 
        public byte[] ModbusFc3(byte Id, short Address, short Quantity)
        {
            List<byte> sendData = new List<byte>(255);
            //[1].Send

            sendData.Add((byte)Id);//1:Unit Identifier:This field is used for intra-system routing purpose.
            sendData.Add((byte)FunctionCode.Code03);//8.Function Code : 3 (Read Holding Registers)
            sendData.AddRange(ValueHelper.Instance.GetBytes((short)Address));//3~4.起始地址
            sendData.AddRange(ValueHelper.Instance.GetBytes((short)Quantity));//5~6.需要写入的寄存器数量
            //sendData.AddRange(ValueHelper.Instance.GetBytes((short)value));//7~8.需要写入寄存器的值

            byte[] CRC = new byte[2];
            GetCRC(sendData.ToArray(), ref CRC);
            sendData.AddRange(CRC);//9~10.CRC
            //sendData.AddRange(ValueHelper.Instance.GetBytes((short)0));//9~10.CRC

            return sendData.ToArray();
        }

        /// <summary>
        /// Modbus标准协议-03:Read Holding Registers
        /// </summary>
        /// <param name="Id">byte</param>
        /// <param name="AddressHi">ushort</param>
        /// <param name="AddressLo">ushort</param>
        /// <param name="Quantity">ushort</param>
        /// <returns>Modbus Function 数据包</returns>
        /// 
        public byte[] ModbusFc3(byte Id, ushort AddressHi, ushort AddressLo, ushort Quantity)
        {
            List<byte> sendData = new List<byte>(255);
            //[1].Send

            sendData.Add((byte)Id);//1:Unit Identifier:This field is used for intra-system routing purpose.
            sendData.Add((byte)FunctionCode.Code03);//8.Function Code : 3 (Read Holding Registers)
            sendData.Add((byte)AddressHi);//3.起始地址 : 高字节
            sendData.Add((byte)AddressLo);//4.起始地址 : 低字节
            sendData.AddRange(ValueHelper.Instance.GetBytes((short)Quantity));//5~6.需要写入的寄存器数量
            //sendData.AddRange(ValueHelper.Instance.GetBytes((short)value));//7~8.需要写入寄存器的值

            byte[] CRC = new byte[2];
            GetCRC(sendData.ToArray(), ref CRC);
            sendData.AddRange(CRC);//9~10.CRC
            //sendData.AddRange(ValueHelper.Instance.GetBytes((short)0));//9~10.CRC

            return sendData.ToArray();
        }
        #endregion

        #region Modbus标准协议-04:Read Input Registers
        /// <summary>
        /// Modbus标准协议-04:Read Input Registers
        /// </summary>
        /// <param name="Id">byte</param>
        /// <param name="Address">short</param>
        /// <param name="Quantity">short</param>
        /// <returns>Modbus Function 数据包</returns>
        /// 
        public byte[] ModbusFc4(byte Id, short Address, short Quantity)
        {
            List<byte> sendData = new List<byte>(255);
            //[1].Send

            sendData.Add((byte)Id);//1:Unit Identifier:This field is used for intra-system routing purpose.
            sendData.Add((byte)FunctionCode.Code04);//8.Function Code : 4 (Read Input Registers)
            sendData.AddRange(ValueHelper.Instance.GetBytes((short)Address));//3~4.起始地址
            sendData.AddRange(ValueHelper.Instance.GetBytes((short)Quantity));//5~6.需要写入的寄存器数量
            //sendData.AddRange(ValueHelper.Instance.GetBytes((short)value));//7~8.需要写入寄存器的值

            byte[] CRC = new byte[2];
            GetCRC(sendData.ToArray(), ref CRC);
            sendData.AddRange(CRC);//9~10.CRC
            //sendData.AddRange(ValueHelper.Instance.GetBytes((short)0));//9~10.CRC

            return sendData.ToArray();
        }

        /// <summary>
        /// Modbus标准协议-04:Read Input Registers
        /// </summary>
        /// <param name="Id">byte</param>
        /// <param name="AddressHi">ushort</param>
        /// <param name="AddressLo">ushort</param>
        /// <param name="Quantity">ushort</param>
        /// <returns>Modbus Function 数据包</returns>
        /// 
        public byte[] ModbusFc4(byte Id, ushort AddressHi, ushort AddressLo, ushort Quantity)
        {
            List<byte> sendData = new List<byte>(255);
            //[1].Send

            sendData.Add((byte)Id);//1:Unit Identifier:This field is used for intra-system routing purpose.
            sendData.Add((byte)FunctionCode.Code04);//2.Function Code : 4 (Read Input Registers)
            sendData.Add((byte)AddressHi);//3.起始地址 : 高字节
            sendData.Add((byte)AddressLo);//4.起始地址 : 低字节
            sendData.AddRange(ValueHelper.Instance.GetBytes((short)Quantity));//5~6.需要写入的寄存器数量

            byte[] CRC = new byte[2];
            GetCRC(sendData.ToArray(), ref CRC);
            sendData.AddRange(CRC);//9~10.CRC

            return sendData.ToArray();
        }
        #endregion
        
        #region Modbus标准协议-05:Write Single Coil
        /// <summary>
        /// Modbus标准协议-05:Write Single Coil
        /// </summary>
        /// <param name="Id">byte</param>
        /// <param name="CoilAddressHi">ushort</param>
        /// <param name="CoilAddressLo">ushort</param>
        /// <param name="ForceDataHi">ushort</param>
        /// <param name="ForceDataLo">ushort</param>
        /// <returns>Modbus Function 数据包</returns>
        /// 
        public byte[] ModbusFc5(byte Id, ushort CoilAddressHi, ushort CoilAddressLo, ushort ForceDataHi, ushort ForceDataLo)
        {
            List<byte> sendData = new List<byte>(255);
            //[1].Send

            sendData.Add((byte)Id);//1:Unit Identifier:This field is used for intra-system routing purpose.
            sendData.Add((byte)FunctionCode.Code05);//2.Function Code : 5 (Write Single Coil)
            sendData.Add((byte)CoilAddressHi);//3.起始地址-High
            sendData.Add((byte)CoilAddressLo);//4.起始地址-Lo
            sendData.Add((byte)ForceDataHi);//7.需要写入寄存器的值-High
            sendData.Add((byte)ForceDataLo);//8.需要写入寄存器的值-Lo

            byte[] CRC = new byte[2];
            GetCRC(sendData.ToArray(), ref CRC);
            sendData.AddRange(CRC);//9~10.CRC
            //sendData.AddRange(ValueHelper.Instance.GetBytes((short)0));//9~10.CRC

            return sendData.ToArray();
        }
        #endregion

        #region Modbus标准协议-06:Write Single Register
        /// <summary>
        /// Modbus标准协议-06:Write Single Register
        /// </summary>
        /// <param name="Id">byte</param>
        /// <param name="Address">short</param>
        /// <param name="Value">short</param>
        /// <returns>Modbus Function 数据包</returns>
        /// 
        public byte[] ModbusFc6(byte Id, short Address, short Value)
        {
            List<byte> sendData = new List<byte>(255);
            //[1].Send

            sendData.Add((byte)Id);//1:Unit Identifier:This field is used for intra-system routing purpose.
            sendData.Add((byte)FunctionCode.Code06);//8.Function Code : 2 (Read Multiple Register)
            sendData.AddRange(ValueHelper.Instance.GetBytes((short)Address));//3~4.起始地址
            //sendData.AddRange(ValueHelper.Instance.GetBytes((short)Registers));//5~6.需要写入的寄存器数量
            sendData.AddRange(ValueHelper.Instance.GetBytes((short)Value));//7~8.需要写入寄存器的值

            byte[] CRC = new byte[2];
            GetCRC(sendData.ToArray(), ref CRC);
            sendData.AddRange(CRC);//9~10.CRC

            return sendData.ToArray();
        }
        /// <summary>
        /// Modbus标准协议-06:Write Single Register
        /// </summary>
        /// <param name="Id">byte</param>
        /// <param name="Address">short</param>
        /// <param name="Value">ushort</param>
        /// <returns>Modbus Function 数据包</returns>
        /// 
        public byte[] ModbusFc6(byte Id, short Address, ushort Value)
        {
            List<byte> sendData = new List<byte>(255);
            //[1].Send
            ValueHelper.LittleEndian = true;
            sendData.Add((byte)Id);//1:Unit Identifier:This field is used for intra-system routing purpose.
            sendData.Add((byte)FunctionCode.Code06);//8.Function Code : 2 (Read Multiple Register)
            sendData.AddRange(ValueHelper.Instance.GetBytes((ushort)Address));//3~4.起始地址
            sendData.AddRange(ValueHelper.Instance.GetBytes((ushort)Value));//7~8.需要写入寄存器的值

            byte[] CRC = new byte[2];
            GetCRC(sendData.ToArray(), ref CRC);
            sendData.AddRange(CRC);//9~10.CRC

            return sendData.ToArray();
        }
        /// <summary>
        /// Modbus标准协议-06:Write Single Register
        /// </summary>
        /// <param name="Id">byte</param>
        /// <param name="Address">short</param>
        /// <param name="Value">double</param>
        /// <returns>Modbus Function 数据包</returns>
        /// 
        public byte[] ModbusFc6(byte Id, short Address, double Value)
        {
            List<byte> sendData = new List<byte>(255);
            //[1].Send

            sendData.Add((byte)Id);//1:Unit Identifier:This field is used for intra-system routing purpose.
            sendData.Add((byte)FunctionCode.Code06);//8.Function Code : 2 (Read Multiple Register)
            sendData.AddRange(ValueHelper.Instance.GetBytes((short)Address));//3~4.起始地址
            //sendData.AddRange(ValueHelper.Instance.GetBytes((short)Registers));//5~6.需要写入的寄存器数量
            sendData.AddRange(ValueHelper.Instance.GetBytes(Convert.ToInt16(Value)));//7~8.需要写入寄存器的值

            byte[] CRC = new byte[2];
            GetCRC(sendData.ToArray(), ref CRC);
            sendData.AddRange(CRC);//9~10.CRC
            //sendData.AddRange(ValueHelper.Instance.GetBytes((short)0));//9~10.CRC

            return sendData.ToArray();
        }

        #endregion

        #region Modbus标准协议-15:Write Multiple Coils
        /// <summary>
        /// Modbus标准协议-15:Write Multiple Coils
        /// </summary>
        /// <param name="Id">byte</param>
        /// <param name="CoilAddress">ushort</param>
        /// <param name="Quantity">ushort</param>
        /// <param name="ForceData">ushort[]写入的值</param>
        /// <returns>Modbus Function 数据包</returns>
        /// 
        public byte[] ModbusFc15(byte Id, ushort CoilAddress, ushort Quantity, byte[] ForceData)
        {
            List<byte> sendData = new List<byte>(255);
            //[1].Send

            sendData.Add((byte)Id);//1:Unit Identifier:This field is used for intra-system routing purpose.
            sendData.Add((byte)FunctionCode.Code15);//2.Function Code : 5 (Write Single Coil)
            sendData.AddRange(ValueHelper.Instance.GetBytes((short)CoilAddress));//3~4.起始地址
            sendData.AddRange(ValueHelper.Instance.GetBytes((short)Quantity));//5~6.长度
            sendData.Add((byte)ForceData.Length);//7.操作Coils的字节长度
            sendData.AddRange(ForceData);//8.需要写入寄存器的值

            byte[] CRC = new byte[2];
            GetCRC(sendData.ToArray(), ref CRC);
            sendData.AddRange(CRC);//9~10.CRC

            return sendData.ToArray();
        }
        #endregion

        #region Modbus标准协议-16:Write Registers
        /// <summary>
        /// Modbus标准协议-16:Write Registers
        /// </summary>
        /// <param name="Id">byte</param>
        /// <param name="Address">short</param>
        /// <param name="Quantity">short</param>
        /// <param name="Values">float 写入的值</param>
        /// <returns>Modbus Function 数据包</returns>
        /// 
        public byte[] ModbusFc16(byte Id, short Address, short Quantity, float Values)
        {
            List<byte> sendData = new List<byte>(255);
            //[1].Send

            sendData.Add((byte)Id);//1:Unit Identifier:This field is used for intra-system routing purpose.
            sendData.Add((byte)FunctionCode.Code16);//2.Function Code : 5 (Write Single Coil)
            sendData.AddRange(ValueHelper.Instance.GetBytes((short)Address));//3~4.起始地址
            sendData.AddRange(ValueHelper.Instance.GetBytes((short)Quantity));//5~6.长度
            sendData.Add((byte)(Quantity * 2));//7.
            byte[] intBuffer = BitConverter.GetBytes(Values);
            sendData.Add(intBuffer[0]);
            sendData.Add(intBuffer[1]);
            sendData.Add(intBuffer[3]);
            sendData.Add(intBuffer[2]);

            byte[] CRC = new byte[2];
            GetCRC(sendData.ToArray(), ref CRC);
            sendData.AddRange(CRC);//9~10.CRC

            return sendData.ToArray();
        }

        /// <summary>
        /// Modbus标准协议-16:Write Registers
        /// </summary>
        /// <param name="Id">byte</param>
        /// <param name="Address">short</param>
        /// <param name="Quantity">short</param>
        /// <param name="Values">short[]写入的值</param>
        /// <returns>Modbus Function 数据包</returns>
        /// 
        public byte[] ModbusFc16(byte Id, short Address, short Quantity, short[] Values)
        {
            List<byte> sendData = new List<byte>(255);
            //[1].Send

            sendData.Add((byte)Id);//1:Unit Identifier:This field is used for intra-system routing purpose.
            sendData.Add((byte)FunctionCode.Code16);//2.Function Code : 5 (Write Single Coil)
            sendData.AddRange(ValueHelper.Instance.GetBytes((short)Address));//3~4.起始地址
            sendData.AddRange(ValueHelper.Instance.GetBytes((short)Quantity));//5~6.长度
            sendData.Add((byte)(Quantity * 2));//7.
            for (int i = 0; i < Values.Length;i++ )
            {
                sendData.AddRange(ValueHelper.Instance.GetBytes((short)Values[i]));//7.需要写入寄存器的值
            }

            byte[] CRC = new byte[2];
            GetCRC(sendData.ToArray(), ref CRC);
            sendData.AddRange(CRC);//9~10.CRC

            return sendData.ToArray();
        }

        /// <summary>
        /// Modbus标准协议-16:Write Registers
        /// </summary>
        /// <param name="Id">byte</param>
        /// <param name="Address">short</param>
        /// <param name="Quantity">short</param>
        /// <param name="Values">ushort[]写入的值</param>
        /// <returns>Modbus Function 数据包</returns>
        /// 
        public byte[] ModbusFc16(byte Id, short Address, short Quantity, ushort[] Values)
        {
            List<byte> sendData = new List<byte>(255);
            //[1].Send

            sendData.Add((byte)Id);//1:Unit Identifier:This field is used for intra-system routing purpose.
            sendData.Add((byte)FunctionCode.Code16);//2.Function Code : 5 (Write Single Coil)
            sendData.AddRange(ValueHelper.Instance.GetBytes((short)Address));//3~4.起始地址
            sendData.AddRange(ValueHelper.Instance.GetBytes((short)Quantity));//5~6.长度
            sendData.Add((byte)(Quantity * 2));//7.
            //for (int i = 0; i < Values.Length; i++)
            //{
            //    sendData.AddRange(ValueHelper.Instance.GetBytes((ushort)Values[i]));//7.需要写入寄存器的值
            //}

            for (int i = 0; i < Values.Length; i++)
            {
                sendData.Add((byte)(Values[i] >> 8));
                sendData.Add((byte)(Values[i]));
            }

            byte[] CRC = new byte[2];
            GetCRC(sendData.ToArray(), ref CRC);
            sendData.AddRange(CRC);//9~10.CRC

            return sendData.ToArray();
        }
        #endregion

        #region Modbus标准协议-命令返回包处理

        /// <summary>
        /// Modbus标准协议-命令返回包处理
        /// </summary>
        /// <param name="rd">SerialPortReceiveData接收数据包</param>
        /// <returns>bool 数据包是否有效</returns>
        /// 
        public bool ModbusReturn(int BytesToRead, ref SK_FModel.SerialPortReceiveData rd)
        {
            List<short> receData = new List<short>(255);
            bool bvalidData = false;

            //Evaluate message:
            bvalidData = CheckResponse(rd.ReceiveData) ? true : false;
            if (bvalidData)
            {
                //Return requested register values:
                for (int i = 0; i < (rd.ReceiveData.Length - 5) / 2; i++)
                {
                    short value = rd.ReceiveData[2 * i + 3];
                    value <<= 8;
                    value += rd.ReceiveData[2 * i + 4];

                    receData.Add(value);
                }
                rd.ValidData = receData.ToArray();
                rd.ModbusMessage = "Successful";
            }
            else if (!bvalidData && BytesToRead == rd.ReceiveData.Length)
            {
                rd.ModbusMessage = "CRC error";
            }
            else
            {
                rd.ModbusMessage = "Timeout";
            }
            rd.SlaveID = rd.ReceiveData[0];
            rd.FunctionCode = (FunctionCode)rd.ReceiveData[1];
            rd.ModbusResponse = bvalidData;

            return bvalidData;
        }
        #endregion
        
        #region Modbus标准协议-返回数据包
        /// <summary>
        /// Modbus标准协议-返回数据包
        /// </summary>
        /// <param name="Id">byte</param>
        /// <param name="functioncode">FunctionCode</param>
        /// <param name="Quantity">short</param>
        /// <param name="Quantity">short</param>
        /// <returns>Modbus Function 数据包</returns>
        /// 
        public byte[] ModbusResponse(byte Id, FunctionCode functioncode, short[] Values)
        {
            List<byte> sendData = new List<byte>(255);
            //[1].Send

            sendData.Add((byte)Id);//1:Unit Identifier:This field is used for intra-system routing purpose.
            sendData.Add((byte)functioncode);//2.Function Code : 1 (Read Coils)
            sendData.Add((byte)(Values.Length * 2));//3.寄存器数量
            for (int i = 0; i < Values.Length; i++)
            {
                sendData.AddRange(ValueHelper.Instance.GetBytes((short)Values[i]));//7.需要写入寄存器的值
            }

            byte[] CRC = new byte[2];
            GetCRC(sendData.ToArray(), ref CRC);
            sendData.AddRange(CRC);//9~10.CRC

            return sendData.ToArray();
        }
        #endregion

    }
}
