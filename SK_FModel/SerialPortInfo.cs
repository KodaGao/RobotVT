namespace SK_FModel
{
    public class SerialPortInfo
    {
        public SerialPortInfo()
        {
            ReceivedBytesThreshold = 1;
            DataBits = 8;
            RtsEnable = false;
            DtrEnable = false;
            StopBits = System.IO.Ports.StopBits.One;
            Parity = System.IO.Ports.Parity.None;
            BaudRate = 19200;
            PortName = "COM1";
        }

        /// <summary>
        /// 串口号
        /// </summary>
        public string PortName { get; set; }

        /// <summary>
        /// 波特率
        /// </summary>
        public int BaudRate { get; set; }

        /// <summary>
        /// 奇偶校验位
        /// </summary>
        public System.IO.Ports.Parity Parity { get; set; }

        /// <summary>
        /// 停止位
        /// </summary>
        public System.IO.Ports.StopBits StopBits { get; set; }

        /// <summary>
        /// 获取或设置 System.IO.Ports.SerialPort.DataReceived 事件发生前内部输入缓冲区中的字节数。 默认：1
        /// </summary>
        public int ReceivedBytesThreshold { get; set; }

        /// <summary>
        /// 获取或设置每个字节的标准数据位长度。
        /// </summary>
        public int DataBits { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值指示在串行通信中是否启用请求发送 (RTS) 信号。 默认：False
        /// </summary>
        public bool RtsEnable { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值在串行通信过程中启用数据终端就绪 (DTR) 信号。默认：False
        /// </summary>
        public bool DtrEnable { get; set; }

        public string NewLine { get; set; }

        /// <summary>
        /// 接收指令间隔
        /// </summary>
        public int ReceiveOrderInterval { get; set; }
        /// <summary>
        /// 发送指令间隔
        /// </summary>
        public int SenderOrderInterval { get; set; }
    }
}