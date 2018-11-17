using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK_FControl
{
    public class StaticInfo
    {
        public static string SystemStartPath = System.Windows.Forms.Application.StartupPath + "\\";
        public static string LogFileHomePath = SystemStartPath + "\\SysLog\\";
        
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
        /// 系统配置参数信息
        /// </summary>
        public static SK_FModel.SystemConfigInfo SystemConfigInfo;
    }
}
