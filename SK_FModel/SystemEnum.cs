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

        public enum MessageType
        {
            Error,
            Normal,
            Warning,
            Exception
        }

    }
}
