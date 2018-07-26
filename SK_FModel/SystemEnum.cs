using System.ComponentModel;

namespace SK_FModel
{
    public class SystemEnum
    {
        /// <summary>
        /// 日志类型
        /// </summary>
        public enum LogType : int
        {
            /// <summary>
            /// 异常
            /// </summary>
            [Description("异常")]
            Exception = 0,

            /// <summary>
            /// 正常
            /// </summary>
            [Description("正常")]
            Normal = 1,

            /// <summary>
            /// 运行
            /// </summary>
            [Description("运行")]
            Runing = 2,

            /// <summary>
            /// 错误
            /// </summary>
            [Description("错误")]
            Error = 3
        }

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


        public enum MessageType
        {
            Error,
            Normal,
            Warning,
            Exception
        }

    }
}
