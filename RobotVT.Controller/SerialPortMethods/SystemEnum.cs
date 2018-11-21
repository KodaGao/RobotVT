using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotVT.Controller.SerialPortMethods
{
    internal class SystemEnum
    {

        /// <summary>
        /// 寄存器地址
        /// </summary>
        public enum RegisterAddress : int
        {
            /// <summary>
            /// 4~20mA
            /// </summary>
            RealValue4To20 = 0x0000,

            /// <summary>
            /// RS485 即：NA1013、NA1000J
            /// </summary>
            RelalValueRS485 = 0x1000,

            ProtocolConversion = 0x9200,

            /// <summary>
            /// 读SN
            /// </summary>
            ReadSN = 0x9005
        }

    }
}
