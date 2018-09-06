using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotVT.Model
{
    public class S_P_COM
    {
        public S_P_COM()
        {
        }
        

        /// <summary>
        /// 摄像机位置 cloud/云台摄像机，front back left right
        /// </summary>
        public string VT_ID { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string VT_NAME { get; set; }

        /// <summary>
        /// 登陆密码
        /// </summary>
        public string VT_PASSWORD { get; set; }


        /// <summary>
        /// IP
        /// </summary>
        public string VT_IP { get; set; }

        /// <summary>
        /// 端口号
        /// </summary>
        public string VT_PORT { get; set; }
    }
}
