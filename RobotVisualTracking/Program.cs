using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace RobotVT
{
    static class Program
    {
        private static event SK_FModel.SystemDelegate.del_InitSystem Event_InitSystem;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += Application_ThreadException;
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            try
            {
                if (SK_FCommon.Methods.CheckProcess())
                    throw new Exception("当前系统已经启动，只能启动一个系统实例");

                Event_InitSystem += Program_Event_InitSystem;
                SK_FControl.Form.SplashScreen.ShowSplashScreen();
                Event_InitSystem?.Invoke("初始化配置信息...");
                RobotVT.Controller.Methods.InitConfig();
                Event_InitSystem?.Invoke("初始化配置信息完成");
                
                Event_InitSystem?.Invoke("初始化临时资源...");
                RobotVT.Controller.Methods.InitTempResources();
                Event_InitSystem?.Invoke("初始化临时资源完成");

                Event_InitSystem?.Invoke("初始化数据库...");
                RobotVT.Controller.Methods.InitDB();
                Event_InitSystem?.Invoke("初始化数据库完成");

                Event_InitSystem?.Invoke("连接数据库...");
                RobotVT.Controller.Methods.DBOpen();
                Event_InitSystem?.Invoke("连接数据库完成");


                Event_InitSystem?.Invoke("开始启动程序...");
                VisualTracking _MainForm = new VisualTracking();
                _MainForm.Event_SystemLoadFinish += _MainForm_Event_SystemLoadFinish;
                Application.Run(_MainForm);

            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("系统启动失败，错误信息：【{0}】！", ex.Message), "异常信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private static void Program_Event_InitSystem(string Message)
        {
            SK_FControl.Form.SplashScreen.ShowRuningMessage(Message);
        }
        private static void _MainForm_Event_SystemLoadFinish()
        {
            if (SK_FControl.Form.SplashScreen.Instance != null)
            {
                SK_FControl.Form.SplashScreen.Instance.BeginInvoke(new MethodInvoker(SK_FControl.Form.SplashScreen.Instance.Dispose));
                SK_FControl.Form.SplashScreen.Instance = null;
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                Exception _Ex = (Exception)e.ExceptionObject;
                SK_FCommon.LogHelper.SaveLog(SK_FModel.SystemEnum.LogType.Exception, "未处理异常：" + _Ex.Message + "\r\n" + _Ex.StackTrace, RobotVT.Controller.StaticInfo.LogFileHomePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("系统内部异常【{0}】。", ex.Message), "异常信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            try
            {
                SK_FCommon.LogHelper.SaveLog(SK_FModel.SystemEnum.LogType.Exception, "未处理异常：" + e.Exception.Message + "\r\n" + e.Exception.StackTrace, RobotVT.Controller.StaticInfo.LogFileHomePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("系统内部异常【{0}】。", ex.Message), "异常信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
