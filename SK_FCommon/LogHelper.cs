using System;
using System.IO;
using System.Text;

namespace SK_FCommon
{
    /// <summary>
    /// 日志帮助类
    /// </summary>
    public class LogHelper
    {
        /// <summary>
        /// 保存日志
        /// </summary>
        /// <param name="LogType"></param>
        /// <param name="LogInfo"></param>
        /// <param name="LogFileHomePath"></param>
        /// <param name="IsAdd"></param>
        public static void SaveLog(SK_FModel.SystemEnum.LogType LogType, string LogInfo, string LogFileHomePath)
        {
            try
            {
                if (!Directory.Exists(LogFileHomePath))
                    Directory.CreateDirectory(LogFileHomePath);
                if (!LogFileHomePath.EndsWith("\\"))
                    LogFileHomePath += "\\";
                string _FilePath = LogFileHomePath + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
                StreamWriter _SW = new StreamWriter(_FilePath, true, Encoding.Default);
                _SW.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + LogInfo + "\r\n");
                _SW.Flush();
                _SW.Close();
                _SW.Dispose();
                _SW = null;
            }
            catch (Exception ex)
            {
                throw new Exception("保存日志失败，错误信息：" + ex.Message);
            }
        }
    }
}
