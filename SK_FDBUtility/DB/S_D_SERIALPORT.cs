namespace SK_FDBUtility.DB
{
    public class S_D_SERIALPORT
    {
        public S_D_SERIALPORT()
        {
        }


        /// <summary>
        /// 串口关键字
        /// </summary>
        public string SP_KEY { get; set; }

        /// <summary>
        /// 串口编号
        /// </summary>
        public string SP_PORT { get; set; }

        /// <summary>
        /// 波特率
        /// </summary>
        public string SP_BAUDRATE { get; set; }


        /// <summary>
        /// 数据位
        /// </summary>
        public string SP_DATABIT { get; set; }

        /// <summary>
        /// 校验位
        /// </summary>
        public string SP_PRATITY { get; set; }

        /// <summary>
        /// 停止位
        /// </summary>
        public string SP_STOPBIT { get; set; }

        /// <summary>
        /// 超时时间
        /// </summary>
        public string SP_TIMEOUT { get; set; }

        /// <summary>
        /// 采样时间
        /// </summary>
        public string SP_SAMPLE { get; set; }
    }

//    SELECT r.SP_KEY, r.SP_PORT, r.SP_BAUDRATE, r.SP_DATABIT, r.SP_PRATITY,
//    r.SP_STOPBIT, r.SP_TIMEOUT, r.SP_SAMPLE, r.SP_ACTIVE
//     FROM S_D_SERIALPORT r
}
