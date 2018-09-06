using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK_FVision
{
    public class HIK_Methods
    {
        public HIK_Methods()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        ////时间解析宏定义
        //#define GET_YEAR(_time_)      (((_time_)>>26) + 2000) 
        //#define GET_MONTH(_time_)     (((_time_)>>22) & 15)
        //#define GET_DAY(_time_)       (((_time_)>>17) & 31)
        //#define GET_HOUR(_time_)      (((_time_)>>12) & 31) 
        //#define GET_MINUTE(_time_)    (((_time_)>>6)  & 63)
        //#define GET_SECOND(_time_)    (((_time_)>>0)  & 63)

        /// <summary>
        /// 摄像机时间转换
        /// </summary>
        public static uint GetYear(uint time)
        {
            return (((time) >> 26) + 2000);
        }
        public static uint GetMonth(uint time)
        {
            return (((time) >> 22) & 15);
        }
        public static uint GetDay(uint time)
        {
            return (((time) >> 17) & 31);
        }
        public static uint GetHour(uint time)
        {
            return (((time) >> 12) & 31);
        }
        public static uint GetMinute(uint time)
        {
            return (((time) >> 6) & 63);
        }
        public static uint GetSecond(uint time)
        {
            return (((time) >> 0) & 63);
        }


        //SK_FVision.HIK_NetSDK.NET_DVR_TIME struAbsTime = new SK_FVision.HIK_NetSDK.NET_DVR_TIME();
        //struAbsTime.dwYear = SK_FVision.HIK_Methods.GetYear(struFaceMatchAlarm.struSnapInfo.dwAbsTime);
        //struAbsTime.dwMonth = SK_FVision.HIK_Methods.GetYear(struFaceMatchAlarm.struSnapInfo.dwAbsTime);
        //struAbsTime.dwDay = SK_FVision.HIK_Methods.GetYear(struFaceMatchAlarm.struSnapInfo.dwAbsTime);
        //struAbsTime.dwHour = SK_FVision.HIK_Methods.GetYear(struFaceMatchAlarm.struSnapInfo.dwAbsTime);
        //struAbsTime.dwMinute = SK_FVision.HIK_Methods.GetYear(struFaceMatchAlarm.struSnapInfo.dwAbsTime);
        //struAbsTime.dwSecond = SK_FVision.HIK_Methods.GetYear(struFaceMatchAlarm.struSnapInfo.dwAbsTime);
        public static string GetDateTime_Time(uint time)
        {
            return GetHour(time).ToString() + GetMinute(time).ToString() + GetSecond(time).ToString();
        }

    }
}