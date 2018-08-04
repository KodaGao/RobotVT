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

        public static Queue<SK_FModel.SystemMessageInfo> QueueMessageInfo;
        public static bool IsSaveLogInfo;

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
    }
}
