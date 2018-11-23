﻿using RobotVT.Controller.SerialPortMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotVT.Controller
{
    public class StaticInfo
    {
        public static string ApplicationTitle = "地面站视觉跟踪管理";
        public static string AppPlatformTitle = ApplicationTitle + "（终端）";
        public static string AppManageTitle = ApplicationTitle + "（管理端）";
        public static string SystemStartPath = System.Windows.Forms.Application.StartupPath + "\\";
        public static string LogFileHomePath = SystemStartPath + "\\SysLog\\";
        public static string CapturePath = SystemStartPath + "\\CaptureImage\\"+ DateTime.Today.ToString("yyyyMMdd") + "\\";
        public static string CameraSetFormTitle = "设备信息";
        public static string ControlObject = "控制器";

        public static string WirelessController = "VTWire";

        public static Queue<SK_FModel.SystemMessageInfo> QueueMessageInfo;
        public static bool IsSaveLogInfo;
        public static bool IsLoadCaputeImage;

        /// <summary>
        /// 数据库文件绝对路径
        /// </summary>
        public static string DBFilePath = SystemStartPath + "DB\\SK.FDB";

        /// <summary>
        /// FireBirdDB
        /// </summary>
        public static SK_FDBUtility.FirebirdDBOperator FirebirdDBOperator;


        /// <summary>
        /// 共享的配置参数信息
        /// </summary>
        public static ConfigInfo ConfigInfo;

        /// <summary>
        /// Modbus
        /// </summary>
        internal static ModbusHelper ModbusHelper;

        /// <summary>
        /// RobotInfoModule
        /// </summary>
        internal static RobotInfoModule RobotIM;
        

        /// <summary>
        /// 指令发送间隔 单位：ms
        /// </summary>
        public static int SenderOrderInterval = 500;

        /// <summary>
        /// 海康数据接卸
        /// </summary>
        public static HIK_AnalysisData HIKAnalysis; 

    }
}
