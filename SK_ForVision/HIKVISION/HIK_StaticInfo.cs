using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK_FVision
{
    public class HIK_StaticInfo
    {
        /// <summary>
        /// 应用层取流协议
        /// </summary>
        public enum ProtoType : int
        {
            /// <summary>
            /// 私有协议
            /// </summary>
            [Description("私有协议")]
            Private = 0,

            /// <summary>
            /// RTSP协议
            /// </summary>
            [Description("RTSP协议")]
            RTSP = 1
        }

        /// <summary>
        /// 应用层取流协议
        /// </summary>
        public enum HTTPS : int
        {
            /// <summary>
            /// HTTP
            /// </summary>
            [Description("HTTP")]
            HTTP = 0,

            /// <summary>
            ///HTTPS
            /// </summary>
            [Description("HTTPS")]
            HTTPS = 1,

            /// <summary>
            /// 自适应
            /// </summary>
            [Description("自适应")]
            Auto = 2
        }



    }
}
