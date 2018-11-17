using System;
using static SK_FModel.SerialPortEnum;

namespace SK_FModel
{
    /// <summary>
    /// 接收数据信息
    /// </summary>
    public class ReceiveDataInfo
    {
        /// <summary>
        /// 参数编号
        /// </summary>
        public int ParaId { get; set; }

        /// <summary>
        /// 代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        public object value { get; set; }
        /// <summary>
        /// 更新日期
        /// </summary>
        public DateTime UpdateDateTime { get; set; }
    }

    public class SerialPortReceiveData
    {
        private byte[] received = null;
        private int id = 255;
        private FunctionCode code = FunctionCode.Code03;
        private short[] valid = null;
        private bool response = false;
        private string message = "";
        public byte[] ReceiveData
        {
            set { received = value; }
            get { return received; }
        }

        public int SlaveID
        {
            set { id = value; }
            get { return id; }
        }

        public FunctionCode FunctionCode
        {
            set { code = value; }
            get { return code; }
        }

        public short[] ValidData
        {
            set { valid = value; }
            get { return valid; }
        }
        public bool ModbusResponse
        {
            set { response = value; }
            get { return response; }
        }
        public string ModbusMessage
        {
            set { message = value; }
            get { return message; }
        }
    }

    public class RLSSendData
    {
        private int id = 255;
        private FunctionCode code = FunctionCode.Code03;

        public int SlaveID
        {
            set { id = value; }
            get { return id; }
        }
        public FunctionCode FunctionCode
        {
            set { code = value; }
            get { return code; }
        }

        public short pollStart { get; set; }
        public short pollLength { get; set; }
        public short pollData { get; set; }

        public byte[] GetReadData()
        {
            string sendmessage = ":" + id.ToString("X2") + 3.ToString("X2") + pollStart.ToString("X4") + pollLength.ToString("X4");
            byte[] message = new byte[sendmessage.Length + 4];

            BuildMessage(sendmessage, ref message);
            return message;
        }

        public byte[] GetWriteData()
        {
            string sendmessage = ":" + id.ToString("X2") + 6.ToString("X2") + pollStart.ToString("X4") + pollData.ToString("X4");
            byte[] message = new byte[sendmessage.Length + 4];

            BuildMessage(sendmessage, ref message);
            return message;
        }

        #region Build Message
        private void BuildMessage(string sendmessage, ref byte[] message)
        {
            //Array to receive CRC bytes:
            byte[] Checksum = new byte[2];

            System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
            byte[] ascii = asciiEncoding.GetBytes(sendmessage);

            for (int i = 0; i < ascii.Length; i++)
            {
                message[i] = ascii[i];
            }

            GetChecksum(message, ref Checksum);
            message[message.Length - 4] = Checksum[0];
            message[message.Length - 3] = Checksum[1];
            message[message.Length - 2] = 0x0D;
            message[message.Length - 1] = 0x0A;
        }
        #endregion

        #region Checksum Computation
        private void GetChecksum(byte[] message, ref byte[] Checksum)
        {
            //Function expects a modbus message of any length as well as a 2 byte CRC array in which to 
            //return the CRC values:

            Int32 sum = 0;

            for (int i = 1; i < (message.Length) - 4; i++)
            {
                sum = sum + message[i];
            }

            Int32 rest = sum % 256;
            string checksum = (255 - rest + 1).ToString("X2");

            System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
            Checksum = asciiEncoding.GetBytes(checksum);
        }
        #endregion
    }
    public class RLSReceiveData
    {
        private byte[] received = null;
        private int id = 255;
        private FunctionCode code = FunctionCode.Code03;
        private byte[] valid = null;
        private bool response = false;
        private string message = "";
        public byte[] ReceiveData
        {
            set
            {
                received = value;
                Checksum_new();
            }
            get { return received; }
        }

        public int SlaveID
        {
            set { id = value; }
            get { return id; }
        }

        public FunctionCode FunctionCode
        {
            set { code = value; }
            get { return code; }
        }

        public byte[] ValidData
        {
            set { valid = value; }
            get { return valid; }
        }
        public bool RLSResponse
        {
            set { response = value; }
            get { return response; }
        }
        public string RLSMessage
        {
            set { message = value; }
            get { return message; }
        }

        #region Checksum Computation
        private void Checksum()
        {
            //Function expects a modbus message of any length as well as a 2 byte CRC array in which to 
            //return the CRC values:
            System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
            Int32 sum = 0;

            id = Convert.ToInt32(asciiEncoding.GetString(received, 1, 2), 16);
            code = (FunctionCode)Convert.ToInt32(asciiEncoding.GetString(received, 3, 2), 16);
            valid = new byte[(received.Length) - 5];
            for (int i = 1; i < (received.Length) - 4; i++)
            {
                sum = sum + received[i];
                valid[i - 1] = received[i];
            }

            Int32 rest = sum % 256;
            string checksum = (255 - rest + 1).ToString("X2");

            byte[] Checksum = asciiEncoding.GetBytes(checksum);

            if (Checksum[0] == received[(received.Length) - 4] && Checksum[1] == received[(received.Length) - 3])
            {
                message = "Successful"; response = true;
            }
            else
            {
                message = "SumError"; response = false; valid = null; id = 255;
            }
        }

        private void Checksum_new()
        {
            Int32 sum = 0;
            for (int i = 1; i < received.Length - 1; i++)
            {
                sum = sum + received[i];
            }
            sum = (~sum) + 1;


            if (received[0] == 0)
            {
                message = "TimeOut"; ; response = false; valid = null;
            }
            if (received[0] == 255 && received[8] == (byte)sum)
            {
                message = "Successful"; response = true;
            }
            else if (received[0] == 255 && received[8] != (byte)sum)
            {
                message = "SumError"; response = false; valid = null;
            }
        }
        #endregion
    }

    public class GasGenerationSendData
    {
        private byte[] start_flag = { 0xFF, 0xFE };
        private short length = 2;
        private short funcation = 0x05;
        private short cmd = 0x05;
        private short ch = 0x01;
        private short[] msb_lsb = null;
        private string message = "";
        
        public byte[] StartFlag
        {
            get { return start_flag; }
        }

        public short Datalength
        {
            set { length = value; }
            get { return length; }
        }

        public short Function
        {
            set { cmd = value; }
            get { return cmd; }
        }

        public short Cmd
        {
            set { cmd = value; }
            get { return cmd; }
        }
        public short Ch
        {
            set { ch = value; }
            get { return ch; }
        }

        public short[] Msb_Lsb
        {
            set { msb_lsb = value; }
            get { return msb_lsb; }
        }

        public string ModbusMessage
        {
            set { message = value; }
            get { return message; }
        }
    }
    public class GasGenerationReceiveData
    {
        private byte[] received = null;
        private short[] start_flag = { 0xFF, 0xFE };
        private short[] end_flag = { 0xFF, 0xFA };
        private short cmd = 0x05;
        private short length = 2;
        private short[] valid = null;
        private string message = "";
        public byte[] ReceiveData
        {
            set { received = value; }
            get { return received; }
        }

        public short[] EndFlag
        {
            get { return end_flag; }
        }

        public short Cmd
        {
            set { cmd = value; }
            get { return cmd; }
        }

        public short Datalength
        {
            set { length = value; }
            get { return length; }
        }

        public short[] ValidData
        {
            set { valid = value; }
            get { return valid; }
        }

        public string ModbusMessage
        {
            set { message = value; }
            get { return message; }
        }
    }

}