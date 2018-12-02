using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotVT.Controller
{
    internal class TargetFollowEnum
    {
        /// <summary>
        /// 寄存器地址
        /// </summary>
        public enum TargetCommand : byte
        {
            /// <summary>
            /// 选取高清SDI一路跟踪点
            /// </summary>
            SDI = 0x27,
            /// <summary>
            /// 选取高清PAL一路跟踪点
            /// </summary>
            PAL = 0x28,
            /// <summary>
            /// 输出分辨率为1080P(默认)
            /// </summary>
            P1080 = 0x29,
            /// <summary>
            /// 输出分辨率为720P
            /// </summary>
            P720 = 0x30,
            /// <summary>
            /// 取消跟踪
            /// </summary>
            Cancel = 0x25,
            /// <summary>
            /// 确认指令
            /// </summary>
            Check = 0x00
        }

        /// <summary>
        /// 寄存器地址
        /// </summary>
        public enum TargetStatus : byte
        {
            /// <summary>
            /// 正在跟踪
            /// </summary>
            Targeting = 0x01,
            /// <summary>
            /// 不在跟踪状态
            /// </summary>
            UnTargeting = 0x00
        }

    }
    internal class TargetFollowInfo
    {
        public TargetFollowInfo()
        {
            Framehead1 = 0xaa;
            Framehead2 = 0x55;
            Framehead3 = 0x00;
            Command = TargetFollowEnum.TargetCommand.P1080;

            res1 = 0x00;
            res2 = 0x00;
            res3 = 0x00;
            res4 = 0x00;
            res5 = 0x00;

        }

        public byte[] TargetFollowMessage()
        {
            byte[] buf = new byte[13];
            byte[] _TempByte;

            buf[0] = Framehead1;
            buf[1] = Framehead2;
            buf[2] = Framehead3;
            buf[3] = (byte)Command;
            
            ushort temp = Convert.ToUInt16(Convert.ToString(PitchCoordinate, 2), 2);
            _TempByte = BitConverter.GetBytes(temp);
            buf[4] = _TempByte[0];
            buf[5] = _TempByte[1];

            temp = Convert.ToUInt16(Convert.ToString(AzimuthCoordinate, 2), 2);
            _TempByte = BitConverter.GetBytes(temp);

            buf[6] = _TempByte[0];
            buf[7] = _TempByte[1];

            buf[8] = res1;
            buf[9] = res2;
            buf[10] = res3;
            buf[11] = res4;
            buf[12] = res5;

            return buf;
        }

        /// <summary>
        /// 帧头1
        /// </summary>
        public byte Framehead1 { get; set; }
        /// <summary>
        /// 帧头2
        /// </summary>
        public byte Framehead2 { get; set; }
        /// <summary>
        /// 固定值
        /// </summary>
        public byte Framehead3 { get; set; }
        /// <summary>
        /// 指令
        /// </summary>
        public TargetFollowEnum.TargetCommand Command { get; set; }
        /// <summary>
        /// 俯仰坐标连续量Y
        /// </summary>
        public short PitchCoordinate { get; set; }
        /// <summary>
        /// 方位坐标连续量X
        /// </summary>
        public short AzimuthCoordinate { get; set; }

        public byte res1 { get; set; }
        public byte res2 { get; set; }
        public byte res3 { get; set; }
        public byte res4 { get; set; }
        public byte res5 { get; set; }
    }

    internal class TargetFollowRecvInfo
    {
        public TargetFollowRecvInfo(byte[] recv)
        {
            Framehead1 = recv[0];
            Framehead2 = recv[1];
            TargetStatus = recv[2];
            PitchCoordinate = BitConverter.ToInt16(new byte[] { recv[4], recv[3] }, 0);
            AzimuthCoordinate = BitConverter.ToInt16(new byte[] { recv[6], recv[5] }, 0);
            Count = BitConverter.ToInt32(new byte[] { recv[8], recv[7], recv[10], recv[9] }, 0);

            SumCheck = recv[11];

        }
        /// <summary>
        /// 帧头1
        /// </summary>
        public byte Framehead1 { get; set; }
        /// <summary>
        /// 帧头2
        /// </summary>
        public byte Framehead2 { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public byte TargetStatus { get; set; }
        /// <summary>
        /// 俯仰脱靶量
        /// </summary>
        public short PitchCoordinate { get; set; }
        /// <summary>
        /// 方位脱靶量
        /// </summary>
        public short AzimuthCoordinate { get; set; }
        /// <summary>
        /// 计数器
        /// </summary>
        public Int32 Count { get; set; }

        /// <summary>
        /// 校验
        /// </summary>
        public byte SumCheck { get; set; }
    }
}
