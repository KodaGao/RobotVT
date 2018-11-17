using System.ComponentModel;
using System.IO.Ports;
using static SK_FModel.SerialPortEnum;

namespace SK_FModel
{
    public enum Protocol
    {
        ///// <summary>
        ///// TCP（Transmission Control Protocol，传输控制协议）;IP（Internet Protocol，网际协议）
        ///// </summary>
        //[Description("TCPIP")]
        //TCPIP,
        /// <summary>
        /// Serial Communications Interface，串行通讯接口
        /// </summary>
        [Description("串口通讯")]
        SerialPort,
        /// <summary>
        /// Image Processing and Pattern Recognition（图像处理和模式识别）
        /// </summary>
        [Description("图像识别")]
        IPPR,
        ///// <summary>
        ///// Universal Serial Bus，通用串行总线
        ///// </summary>
        //[Description("USB")]
        //USB,
        /// <summary>
        /// 4~20mA，电流信号
        /// </summary>
        [Description("电流信号")]
        ADC
    }

    public class SerialModel
    {
        public string serialport { get; set; }
        public SerialPortBaudRates baudrate { get; set; }
        public SerialPortDatabits databit { get; set; }
        public Parity parity { get; set; }
        public StopBits stopbit { get; set; }
        public int timeout { get; set; }
        public int samplerate { get; set; }
        public string sp_active { get; set; }
    }
    public class SerialPortEnum
    {
        /// <summary>
        /// 串口波特率
        /// </summary>
        public enum SerialPortBaudRates : int
        {
            BaudRate_75 = 75,
            BaudRate_110 = 110,
            BaudRate_150 = 150,
            BaudRate_300 = 300,
            BaudRate_600 = 600,
            BaudRate_1200 = 1200,
            BaudRate_2400 = 2400,
            BaudRate_4800 = 4800,
            BaudRate_9600 = 9600,
            BaudRate_14400 = 14400,
            BaudRate_19200 = 19200,
            BaudRate_28800 = 28800,
            BaudRate_38400 = 38400,
            BaudRate_56000 = 56000,
            BaudRate_57600 = 57600,
            BaudRate_115200 = 115200,
            BaudRate_128000 = 128000,
            BaudRate_230400 = 230400,
            BaudRate_256000 = 256000
        }

        public enum FunctionCode : byte
        {
            /// <summary>
            /// Read Coils
            /// </summary>
            Code01 = 1,
            /// <summary>
            /// Read Discrete Inputs
            /// </summary>
            Code02 = 2,
            /// <summary>
            /// Read Holding Registers
            /// </summary>
            Code03 = 3,
            /// <summary>
            /// Read Input Registers
            /// </summary>
            Code04 = 4,
            /// <summary>
            /// Write Single Coil
            /// </summary>
            Code05 = 5,
            /// <summary>
            /// Write Single Register
            /// </summary>
            Code06 = 6,
            /// <summary>
            /// Write Multiple Coils
            /// </summary>
            Code15 = 15,
            /// <summary>
            /// Write Multiple Registers
            /// </summary>
            Code16 = 16
        }
        public enum SerialPortDatabits : int
        {
            FiveBits = 5,
            SixBits = 6,
            SeventBits = 7,
            EightBits = 8
        }

        public enum CRC16CodeCheckType
        {
            CRC16,
            CRCTable
        }
    }
}
