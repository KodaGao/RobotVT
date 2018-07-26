using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RobotVT.Controller
{
    public class ConfigInfo
    {
        /// <summary>
        /// 显示调试信息
        /// </summary>
        public bool ShowDebugMessage { get; set; }

        /// <summary>
        /// 报告模板保存主目录
        /// </summary>
        public string ReportTemplateHomePath { get; set; }

        /// <summary>
        /// 报告保存主目录
        /// </summary>
        public string ReportHomePath { get; set; }
        
        /// <summary>
        /// 创建出厂编号
        /// </summary>
        public bool CreateFactoryNumber { get; set; }

        /// <summary>
        /// 临时目录
        /// </summary>
        public string TempDirPath { get; set; }

        public int ClearMessageInterval { get; set; }

        public string DefaultPRODUCTID { get; set; }

        public bool DataTrans { get; set; }

        public bool DataTransToManage { get; set; }

        /// <summary>
        /// 相对湿度
        /// </summary>
        public float Humidity { get; set; }

        public float Temperature { get; set; }

        public string ReportPrefix { get; set; }

        //public SystemEnum.CustomerType CustomerType { get; set; }

        public System.Net.IPEndPoint IPEndPoint { get; set; }
    }
}