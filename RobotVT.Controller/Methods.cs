
using System;
using System.IO;
using RobotVT.Controller.SerialPortMethods;
using SK_FVision;

namespace RobotVT.Controller
{
    public class Methods
    {
        /// <summary>
        /// 运行消息事件
        /// </summary>SearchSensorBaseInfo
        public event SK_FModel.SystemDelegate.del_RuningMessage Event_RuningMessage;
        /// <summary>
        /// Debug消息事件
        /// </summary>
        public event SK_FModel.SystemDelegate.del_DebugMessage Event_DebugMessage;

        /// <summary>
        /// 保存异常日志
        /// </summary>
        /// <param name="Info"></param>
        public static void SaveExceptionLog(SK_FModel.SystemEnum.LogType LogType, string Info)
        {
            try
            {
                SK_FCommon.LogHelper.SaveLog(LogType, Info, StaticInfo.LogFileHomePath);
            }
            catch (Exception ex)
            {
                throw new Exception("保存异常日志失败，错误信息：" + ex.Message);
            }
        }


        /// <summary>
        /// 保存异常日志
        /// </summary>
        /// <param name="Info"></param>
        public static void SaveExceptionLog(SK_FModel.SystemEnum.LogType LogType, SK_FModel.SystemMessageInfo _MessageInfo)
        {
            try
            {
                string Info = _MessageInfo.Content;

                SK_FCommon.LogHelper.SaveLog(LogType, Info, StaticInfo.LogFileHomePath);
            }
            catch (Exception ex)
            {
                throw new Exception("保存异常日志失败，错误信息：" + ex.Message);
            }
        }


        /// <summary>
        /// 初始化配置信息
        /// </summary>=
        public static void InitConfig()
        {
            try
            {
                StaticInfo.TargetFollow = new TargetFollow();
                StaticInfo.TargetFollow.Init();
                StaticInfo.RobotIM = new RobotInfoModule();
                StaticInfo.RobotIM.Init();

                SK_FCommon.DirFile.CreateDirectory(StaticInfo.CapturePath);
            }
            catch (Exception _Ex)
            {
                RobotVT.Controller.Methods.SaveExceptionLog(SK_FModel.SystemEnum.LogType.Exception, "初始化配置信息异常：" + _Ex.Message + "\r\n" + _Ex.StackTrace);
                throw new Exception("初始化配置信息失败，错误信息：" + _Ex.Message);
            }
        }

        /// <summary>
        /// 清除系统资源
        /// </summary>
        public static void DisposeSystemResources()
        {
            try
            {
                StaticInfo.RobotIM.Stop();
                StaticInfo.TargetFollow.Stop();
                DBClose();
                ClearTempResources();
            }
            catch (Exception _Ex)
            {
                RobotVT.Controller.Methods.SaveExceptionLog(SK_FModel.SystemEnum.LogType.Exception, "清楚系统资源异常：" + _Ex.Message + "\r\n" + _Ex.StackTrace);
                throw new Exception("清除系统资源失败，错误信息：" + _Ex.Message);
            }
        }

        /// <summary>
        /// 初始化临时资源
        /// </summary>
        public static void InitTempResources()
        {
            try
            {
                bool m_bInitSDK = false;
                m_bInitSDK = HIK_NetSDK.NET_DVR_Init();
                if (m_bInitSDK == false)
                {
                    //MessageBox.Show("NET_DVR_Init error!");
                    throw new Exception("初始化临时资源失败，错误信息：NET_DVR_Init error!");
                }
                else
                {
                    //设置连接时间与重连时间
                    HIK_NetSDK.NET_DVR_SetConnectTime(2000, 1);
                    HIK_NetSDK.NET_DVR_SetReconnect(10000, 1);

                    //保存SDK日志 To save the SDK log
                    HIK_NetSDK.NET_DVR_SetLogToFile(3, @"SdkLog\", true);
                }
                StaticInfo.HIKAnalysis = new HIK_AnalysisData();

            }
            catch (Exception _Ex)
            {
                RobotVT.Controller.Methods.SaveExceptionLog(SK_FModel.SystemEnum.LogType.Exception, "初始化临时资源异常：" + _Ex.Message + "\r\n" + _Ex.StackTrace);
                throw new Exception("初始化临时资源失败，错误信息：" + _Ex.Message);
            }
        }

        /// <summary>
        /// 清除临时资源
        /// </summary>
        private static void ClearTempResources()
        {
            try
            {
                if (Directory.Exists(StaticInfo.ConfigInfo.TempDirPath))
                    Directory.Delete(StaticInfo.ConfigInfo.TempDirPath, true);
            }
            catch { }
        }


        /// <summary>
        /// 初始化Firebird数据库
        /// </summary>
        public static void InitDB()
        {
            try
            {
                StaticInfo.FirebirdDBOperator = new SK_FDBUtility.FirebirdDBOperator();
                StaticInfo.FirebirdDBOperator.Database = StaticInfo.DBFilePath;
                StaticInfo.FirebirdDBOperator.UserID = "sysdba";
                StaticInfo.FirebirdDBOperator.Password = "masterkey";  //icuryy4me masterkey
                StaticInfo.FirebirdDBOperator.Charset = "NONE";
            }
            catch (Exception _Ex)
            {
                RobotVT.Controller.Methods.SaveExceptionLog(SK_FModel.SystemEnum.LogType.Exception, "初始化数据库异常：" + _Ex.Message + "\r\n" + _Ex.StackTrace);
                throw new Exception("初始化数据库失败，错误信息：" + _Ex.Message);
            }
        }

        /// <summary>
        /// 连接数据库
        /// </summary>
        public static void DBOpen()
        {
            try
            {
                StaticInfo.FirebirdDBOperator.Open();
            }
            catch (Exception _Ex)
            {
                RobotVT.Controller.Methods.SaveExceptionLog(SK_FModel.SystemEnum.LogType.Exception, "打开数据库异常：" + _Ex.Message + "\r\n" + _Ex.StackTrace);
                throw new Exception("打开数据库失败，错误信息：" + _Ex.Message);
            }
        }

        /// <summary>
        /// 断开数据库
        /// </summary>
        public static void DBClose()
        {
            try
            {
                StaticInfo.FirebirdDBOperator.Close();
            }
            catch (Exception _Ex)
            {
                RobotVT.Controller.Methods.SaveExceptionLog(SK_FModel.SystemEnum.LogType.Exception, "关闭数据库异常：" + _Ex.Message + "\r\n" + _Ex.StackTrace);
                throw new Exception("关闭数据库失败，错误信息：" + _Ex.Message);
            }
        }
                
        /// <summary>
        /// 获取程序Title
        /// </summary>
        public static string GetApplicationTitle()
        {
            return StaticInfo.AppPlatformTitle;
        }


    }
}