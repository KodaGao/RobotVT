using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing;

namespace SK_FCommon
{
    public class Methods
    {
        public static string GetEnumDescription(Enum enumValue)
        {
            try
            {
                string str = enumValue.ToString();
                System.Reflection.FieldInfo field = enumValue.GetType().GetField(str);
                object[] objs = field.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if (objs == null || objs.Length == 0) return str;
                System.ComponentModel.DescriptionAttribute da = (System.ComponentModel.DescriptionAttribute)objs[0];
                return da.Description;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 打开文件
        /// </summary>
        /// <param name="FilePath"></param>
        public static void StartFile(string FilePath)
        {
            System.Diagnostics.Process.Start(FilePath);
        }

        /// <summary>
        /// 终止当前线程
        /// </summary>
        public static void KillCurrProcess()
        {
            try
            {
                Process CurProcess;
                CurProcess = Process.GetCurrentProcess();
                CurProcess.Kill();
            }
            catch { }
            finally
            {
                GC.Collect();
            }
        }

        // 按比例缩放图片
        public static Bitmap ZoomPicture(Image SrcImage, int Width, int Height, Image.GetThumbnailImageAbort Callback)
        {
            return (Bitmap)SrcImage.GetThumbnailImage(Width, Height, Callback, IntPtr.Zero);
        }

        /// <summary>
        /// 复制对象
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static object CopyObject(object source)
        {
            if (source == null)
                return null;
            Object objectReturn = null;
            using (MemoryStream stream = new MemoryStream())
            {
                try
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, source);
                    stream.Position = 0;

                    objectReturn = formatter.Deserialize(stream);
                }
                catch (Exception e)
                {
                }
            }
            return objectReturn;
        }

        /// <summary>
        /// 字符串转换字节
        /// </summary>
        /// <param name="str2"></param>
        /// <returns></returns>
        public static byte StringToByte(string str)
        {
            char[] arr = str.ToCharArray();
            int b = 0;
            for (int i = 0; i < 8; i++)
            {
                if (arr[i] == '1')
                {
                    int c = 1;
                    for (int j = 0; j < 7 - i; j++)
                    {
                        c *= 2;
                    }
                    b += c;
                }
            }
            return Convert.ToByte(b);
        }

        /// <summary>
        /// 整形转换为8位二进制字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string IntTo8byteString(int value)
        {
            string _Result;
            _Result = Convert.ToString(value, 2).PadLeft(8, '0');
            return _Result;
        }

        ///  <summary>
        ///  序列化为二进制字节数组
        ///  </summary>
        ///  <param  name="request">要序列化的对象 </param>
        ///  <returns>字节数组 </returns>
        public static byte[] SerializeBinary(object request)
        {
            BinaryFormatter serializer = new BinaryFormatter();
            MemoryStream memStream = new MemoryStream();
            serializer.Serialize(memStream, request);
            return memStream.GetBuffer();
        }

        /// <summary>
        /// 反射DLL，并得到接口对象
        /// </summary>
        /// <param name="DllFileName"></param>
        /// <param name="InterfaceName"></param>
        /// <returns></returns>
        public static object ReflectionAssembly(string DllFileName, string InterfaceName)
        {
            object _Result = null;
            try
            {
                Assembly _asm = Assembly.LoadFrom(DllFileName);
                Type[] types = _asm.GetTypes();
                Type _TempType = null;
                foreach (Type _tempInfo in types)
                {
                    if (_tempInfo.GetInterface(InterfaceName) != null)
                    {
                        _TempType = _tempInfo;
                        break;
                    }
                }
                if (_TempType == null)
                {
                    throw new Exception(string.Format("【{0}】接口未实现。", InterfaceName));
                }
                _Result = _asm.CreateInstance(_TempType.FullName);
                return _Result;
            }
            catch (Exception ex)
            {
                throw new Exception("反射DLL失败，错误信息：" + ex.Message);
            }
        }

        ///  <summary>
        ///  从二进制数组反序列化得到对象
        ///  </summary>
        ///  <param  name="buf">字节数组 </param>
        ///  <returns>得到的对象 </returns>
        public static object DeserializeBinary(byte[] buf)
        {
            MemoryStream memStream = new MemoryStream(buf);
            memStream.Position = 0;
            BinaryFormatter deserializer = new BinaryFormatter();
            object newobj = deserializer.Deserialize(memStream);
            memStream.Close();
            return newobj;
        }

        /// <summary>
        /// 写测试文档
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        public static void WriteTestLog(string path, string content)
        {
            try
            {
                StreamWriter sw = new StreamWriter(path, true);
                sw.AutoFlush = true;
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss\t") + content);
                sw.Close();
            }
            catch (Exception ex)
            {
            }
        }

        public static string GetAssemblyTitle()
        {
            string _Title = string.Empty;
            object[] _Attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
            if (_Attributes.Length > 0)
            {
                AssemblyTitleAttribute _TitleAttribute = (AssemblyTitleAttribute)_Attributes[0];
                _Title = _TitleAttribute.Title;
            }
            if (string.IsNullOrWhiteSpace(_Title))
                _Title = Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            return _Title;
        }

        /// <summary>
        /// 创建XLS文件
        /// </summary>
        /// <param name="InfoList">内容数组String</param>
        /// <param name="Path">XLS文件路径</param>
        public static void WriterExcel(List<string[]> InfoList, string Path)
        {
            string FileStr = "";
            StreamWriter SW = new StreamWriter(Path, true, System.Text.Encoding.Default);

            foreach (string[] _ItemInfo in InfoList)
            {
                for (int i = 0; i < _ItemInfo.Length; i++)
                {
                    FileStr += _ItemInfo[i] + (Char)9;
                }
                FileStr += (char)13;
            }

            SW.WriteLine(FileStr);
            SW.Close();
            SW = null;
        }

        /// <summary>
        /// 验证本系统是否运行的第二个实例
        /// </summary>
        /// <returns></returns>
        public static bool CheckProcess()
        {
            Process CurProcess;
            string CurrFileName;
            Process[] ProcessList;
            bool IsExists = false;
            int ProcCount = 0;
            CurProcess = Process.GetCurrentProcess();
            CurrFileName = CurProcess.MainModule.FileName;
            ProcessList = Process.GetProcessesByName(CurProcess.ProcessName);
            foreach (Process process in ProcessList)
            {
                if (process.MainModule.FileName == CurrFileName)
                {
                    ProcCount++;
                    if (ProcCount >= 2)
                    {
                        IsExists = true;
                        break;
                    }
                }
            }
            return IsExists;
        }

        /// <summary>
        /// 获取AssemblyVersion的值
        /// </summary>
        /// <returns></returns>
        public static string GetAssemblyVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        /// <summary>
        /// 验证字符串是否为合法的数字。如果不合法返回False，Num为-1
        /// </summary>
        /// <param name="Phone"></param>
        /// <returns></returns>
        public static bool IsInt(string Str, out int Num)
        {
            try
            {
                Num = int.Parse(Str);
                return true;
            }
            catch
            {
                Num = -1;
                return false;
            }
        }

        /// <summary>
        /// 验证字符串是否为合法的数字
        /// </summary>
        /// <param name="Phone"></param>
        /// <returns></returns>
        public static bool IsInt(string Str)
        {
            try
            {
                int.Parse(Str);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 验证字符串是否为合法的日期类型，合法返回格式化后的日期类型，不合法返回0
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        public static string IsDateTime(string Str)
        {
            DateTime DT;
            try
            {
                DT = Convert.ToDateTime(Str);
                return DT.ToString("yyyy-MM-dd HH:mm:ss");
            }
            catch
            {
                return "0";
            }
        }

        /// <summary>
        /// 判断是否为合法的IP地址
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIPAddress(string ip)
        {
            string _StrTemp = @"(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])";
            if (System.Text.RegularExpressions.Regex.IsMatch(ip, _StrTemp))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 检查目录是否存在，如果不存在则创建
        /// </summary>
        /// <param name="path"></param>
        public static void CheckAndCreateDir(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 判断进度是否已经运行
        /// </summary>
        /// <returns></returns>
        public static bool CheckProcess(string FileName, string ProccessName)
        {
            bool IsExists = false;
            Process[] ProcessList;
            ProcessList = Process.GetProcessesByName(ProccessName);
            if (ProcessList.Length <= 0)
                ProcessList = Process.GetProcessesByName(ProccessName.Replace(".exe", ""));
            foreach (Process process in ProcessList)
            {
                if (process.MainModule.FileName == FileName)
                {
                    IsExists = true;
                    break;
                }
            }
            return IsExists;
        }

        /// <summary>
        /// 验证字符串是否为合法的日期类型。如果不合法返回当前时间
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        public static bool IsDateTime(string Str, out DateTime DescTime)
        {
            try
            {
                DescTime = Convert.ToDateTime(Str);
                return true;
            }
            catch
            {
                DescTime = DateTime.Now;
                return false;
            }
        }

        /// <summary>
        /// 验证字符串是否为合法的数字，不限长度
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        public static bool IsFloat(string Str)
        {
            try
            {
                float.Parse(Str);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 验证字符串是否为合法的数字
        /// </summary>
        /// <param name="Phone"></param>
        /// <returns></returns>
        public static bool IsFloat(string Str, out float Num)
        {
            try
            {
                Num = float.Parse(Str);
                return true;
            }
            catch
            {
                Num = -1;
                return false;
            }
        }

        /// <summary>
        /// 判断是否是指定长度的数字
        /// </summary>
        /// <param name="Str"></param>
        /// <param name="Len"></param>
        /// <returns></returns>
        public static bool IsInt(string Str, int Len)
        {
            try
            {
                for (int i = 0; i < Str.Length; i++)
                {
                    int.Parse(Str[i].ToString());
                }
                if (Str.Length == Len)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
