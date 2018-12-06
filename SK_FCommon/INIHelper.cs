using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace SK_FCommon
{
    public class INIHelper
    {
        //文件INI名称 
        public string Path;

        /**/
        ////声明读写INI文件的API函数 
        [DllImport("kernel32")]

        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);


        [DllImport("kernel32")]

        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);


        //类的构造函数，传递INI文件名 
        public INIHelper(string inipath)
        {
            // 
            // TODO: Add constructor logic here 
            // 
            Path = inipath;
        }

        //写INI文件 
        public void WriteValue(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, this.Path);

        }

        //读取INI文件指定 
        public string ReadValue(string section, string key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(section, key, "", temp, 255, this.Path);
            return temp.ToString();

        }
        /**/
        /// <summary>
        /// 验证文件是否存在
        /// </summary>
        /// <returns>布尔值</returns>
        public bool ExistFile()
        {
            return false;
            //return File.Exists(this.Path);
        }

        /// <summary>
        /// 删除ini文件下所有段落
        /// </summary>
        public void ClearAllSection()
        {
            WriteValue(null, null, null);
        }
        /// <summary>
        /// 删除ini文件下personal段落下的所有键
        /// </summary>
        /// <param name="Section"></param>
        public void ClearSection(string Section)
        {
            WriteValue(Section, null, null);
        }

    }
}
