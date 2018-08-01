
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using RobotVT.Model;
using SK_FVision;

namespace RobotVT.Controller
{
    public class Methods
    {
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
        /// 初始化配置信息
        /// </summary>
        public static void InitConfig()
        {
            try
            {
                //Relations.Common.IniMng _IniMng = GetIniMng();
                //StaticInfo.ConfigInfo = new RelationsLMPModel.Config();
                //StaticInfo.ConfigInfo.ApplicatinType = (RelationsLMPModel.SystemEnum.ApplicatinType)_IniMng.ReadIntValue("Config", "ApplicatinType", 0, 255);
                //StaticInfo.ConfigInfo.ShowDebugMessage = _IniMng.ReadBoolValue("Config", "ShowDebug", false);
                //StaticInfo.ConfigInfo.DataTrans = _IniMng.ReadBoolValue("Config", "DataTrans", false);
                //StaticInfo.ConfigInfo.DataTransToManage = _IniMng.ReadBoolValue("Config", "DataTransToManage", false);
                //StaticInfo.ConfigInfo.ClearMessageInterval = _IniMng.ReadIntValue("Config", "ClearMessageInterval", 10, 255);
                //StaticInfo.ConfigInfo.DefaultPRODUCTID = _IniMng.ReadStringValue("Config", "DefaultPRODUCTID", string.Empty, 255);
                //StaticInfo.ConfigInfo.Humidity = _IniMng.ReadFloatValue("Config", "Humidity", 0, 255);
                //StaticInfo.ConfigInfo.Temperature = _IniMng.ReadIntValue("Config", "Temperature", 0, 255);
                //StaticInfo.ConfigInfo.ReportPrefix = _IniMng.ReadStringValue("Config", "ReportPrefix", string.Empty, 255);
                //StaticInfo.ConfigInfo.CustomerType = (RelationsLMPModel.SystemEnum.CustomerType)_IniMng.ReadIntValue("Config", "CustomerType", 0, 255);
                //StaticInfo.ConfigInfo.CreateFactoryNumber = _IniMng.ReadBoolValue("Config", "CreateFactoryNumber", false);
                //StaticInfo.ConfigInfo.IPEndPoint = new System.Net.IPEndPoint(System.Net.IPAddress.Parse(_IniMng.ReadStringValue("Config", "IpAddress", "127.0.0.1", 255)), _IniMng.ReadIntValue("Config", "Port", 9000, 255));

                //StaticInfo.FTPServerInfo = new RelationsLMPModel.FTPServerInfo();
                //StaticInfo.FTPServerInfo.RemoteHost = _IniMng.ReadStringValue("FTP", "RemoteHost", "192.168.1.83", 255);
                //StaticInfo.FTPServerInfo.RemotePath = _IniMng.ReadStringValue("FTP", "RemotePath", "", 255);
                //StaticInfo.FTPServerInfo.UserName = _IniMng.ReadStringValue("FTP", "UserName", "lmpuser", 255);
                //StaticInfo.FTPServerInfo.PassWord = _IniMng.ReadStringValue("FTP", "PassWord", "aaaaaa", 255);
                //StaticInfo.FTPServerInfo.RemotePort = _IniMng.ReadIntValue("FTP", "RemotePort", 22, 255);

                //StaticInfo.ConfigInfo.ReportHomePath = StaticInfo.SystemStartPath + "Report\\";
                //StaticInfo.ConfigInfo.ReportTemplateHomePath = StaticInfo.SystemStartPath + "ReportTemplate\\";
                //StaticInfo.ConfigInfo.TempDirPath = StaticInfo.SystemStartPath + "Temp\\";
                //StaticInfo.AccuracyInfo = new RelationsLMPModel.AccuracyInfo();
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
                //if (!Directory.Exists(StaticInfo.ConfigInfo.TempDirPath))
                //    Directory.CreateDirectory(StaticInfo.ConfigInfo.TempDirPath);

                //if (!Directory.Exists(StaticInfo.ConfigInfo.ReportHomePath))
                //    Directory.CreateDirectory(StaticInfo.ConfigInfo.ReportHomePath);

                //if (!Directory.Exists(StaticInfo.ConfigInfo.ReportTemplateHomePath))
                //    Directory.CreateDirectory(StaticInfo.ConfigInfo.ReportTemplateHomePath);
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
        
        public static List<S_D_CameraSet> GetS_D_CameraSetList(int PRODUCTIDLen)
        {
            try
            {
                List<S_D_CameraSet> _Result = new List<S_D_CameraSet>();
                if (StaticInfo.FirebirdDBOperator == null) return _Result;

                S_D_CameraSet _S_D_CameraSet;
                string _SqlScript = string.Format("Select VT_ID, VT_NAME, VT_PASSWORD, VT_IP, VT_PORT From S_D_CameraSet;", PRODUCTIDLen);
                System.Data.DataTable _DTTemp;
                _DTTemp = StaticInfo.FirebirdDBOperator.ReturnDataTable(_SqlScript);
                foreach (System.Data.DataRow _DR in _DTTemp.Rows)
                {
                    _S_D_CameraSet = new S_D_CameraSet();
                    _S_D_CameraSet.VT_ID = _DR["VT_ID"].ToString();
                    _S_D_CameraSet.VT_NAME = _DR["VT_NAME"].ToString();
                    _S_D_CameraSet.VT_PASSWORD = _DR["VT_PASSWORD"].ToString();
                    _S_D_CameraSet.VT_IP = _DR["VT_IP"].ToString();
                    _S_D_CameraSet.VT_PORT = _DR["VT_PORT"].ToString();
                    _Result.Add(_S_D_CameraSet);
                }
                return _Result;
            }
            catch (Exception ex)
            {
                throw new Exception("获取摄像头参数信息失败，错误信息：" + ex.Message);
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