using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotVT.Controller.SerialPortMethods
{
    internal class SendOrder

    {
        /// <summary>
        /// 设备地址码
        /// </summary>
        internal byte DeviceAddressId { get; set; }

        /// <summary>
        /// 功能码
        /// </summary>
        internal SK_FModel.SerialPortEnum.FunctionCode FunctionCode { get; set; }

        /// <summary>
        /// 起始地址
        /// </summary>
        internal SystemEnum.RegisterAddress RegisterAddress { get; set; }

        /// <summary>
        /// 读写类型
        /// </summary>
        internal SK_FModel.SerialPortEnum.ReadWriteType ReadWriteType { get; set; }

        /// <summary>
        /// 寄存器数量
        /// </summary>
        internal short RegisterNum { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        internal List<byte> value { get; set; }
    }


    internal class ReturnOrder

    {
        /// <summary>
        /// 设备地址码
        /// </summary>
        internal byte DeviceAddressId { get; set; }

        /// <summary>
        /// 功能码
        /// </summary>
        internal SK_FModel.SerialPortEnum.FunctionCode FunctionCode { get; set; }

        /// <summary>
        /// 起始地址
        /// </summary>
        internal SystemEnum.RegisterAddress RegisterAddress { get; set; }

        /// <summary>
        /// 读写类型
        /// </summary>
        internal SK_FModel.SerialPortEnum.ReadWriteType ReadWriteType { get; set; }

        /// <summary>
        /// 寄存器数量
        /// </summary>
        internal short RegisterNum { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        internal List<byte> value { get; set; }
    }
}
