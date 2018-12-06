using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using SK_FCommon;
using System.Threading;
using System.Net;
using System.IO;

namespace RobotVT.Controller
{
    public class HIK_AnalysisData
    {
        private Queue<HIK_AlarmInfo> AlarmInfoList;
        /// <summary>
        /// 人脸抓拍报警信号处理
        /// </summary>
        /// <param name="FaceSnapAlarm"></param>
        internal delegate void FaceSnapAlarmEventHandler(HIK_AlarmInfo HIKAlarmInfo);
        /// <summary>
        /// 接收数据事件事件
        /// </summary>
        internal event FaceSnapAlarmEventHandler Event_FaceSnapAlarm;

        public string AbsTimetoDatetime(uint _time_)
        {
            string datetime;
            uint year = (((_time_) >> 26) + 2000);
            uint month = (((_time_) >> 22) & 15);
            uint day = (((_time_) >> 17) & 31);
            uint hour = (((_time_) >> 12) & 31);
            uint minute = (((_time_) >> 6) & 63);
            uint second = (((_time_) >> 0) & 63);

            datetime = year.ToString() + "-" + month.ToString() + "-" + day.ToString() + " " + hour.ToString() + ":" + minute.ToString() + ":" + second.ToString();

            return datetime;
        }
        private void AlarmInfoListQueue(HIK_AlarmInfo iK_AlarmInfo)
        {
            if (AlarmInfoList == null) AlarmInfoList = new Queue<HIK_AlarmInfo>();

            if (AlarmInfoList.Count <= 0 || AlarmInfoList.Count % 8 == 0)
            {
                iK_AlarmInfo.QueueNubmer = 1;
            }
            else if (AlarmInfoList.Count > 0)
            {
                iK_AlarmInfo.QueueNubmer = AlarmInfoList.Count % 8 + 1;
            }

            AlarmInfoList.Enqueue(iK_AlarmInfo);
        }
        
        /// <summary>
        /// 视频数据处理
        /// </summary>
        public void dealAlarm(int lCommand, ref SK_FVision.HIK_NetSDK.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser)
        {
            string ste = "";
            switch (lCommand)
            {
                case SK_FVision.HIK_NetSDK.COMM_ALARM_FACE_DETECTION:
                    ste = "人脸侦测报警信息 " + lCommand.ToString();
                    ProcessCommAlarm_FaceDetect(ref pAlarmer, pAlarmInfo, dwBufLen, pUser);
                    break;
                case SK_FVision.HIK_NetSDK.COMM_UPLOAD_FACESNAP_RESULT:
                    ste = "人脸抓拍报警信息 " + lCommand.ToString();
                    ProcessCommAlarm_FaceSNAP(ref pAlarmer, pAlarmInfo, dwBufLen, pUser);
                    break;
                case SK_FVision.HIK_NetSDK.COMM_SNAP_MATCH_ALARM:
                    ste = "人脸比对结果信息 " + lCommand.ToString();
                    ProcessCommAlarm_SNAPMatch(ref pAlarmer, pAlarmInfo, dwBufLen, pUser);
                    break;
                default:
                    break;
            }
        }
        
        private void ProcessCommAlarm_FaceDetect(ref SK_FVision.HIK_NetSDK.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser)
        {

            SK_FVision.HIK_NetSDK.NET_DVR_FACEDETECT_ALARM struFaceDetectionAlarm = new SK_FVision.HIK_NetSDK.NET_DVR_FACEDETECT_ALARM();

            struFaceDetectionAlarm = (SK_FVision.HIK_NetSDK.NET_DVR_FACEDETECT_ALARM)Marshal.PtrToStructure(pAlarmInfo, typeof(SK_FVision.HIK_NetSDK.NET_DVR_FACEDETECT_ALARM));

            if (struFaceDetectionAlarm.dwFacePicDataLen > 0 && struFaceDetectionAlarm.pFaceImage != null)
            {
                FaceDetectPicSave(struFaceDetectionAlarm);
            }
        }

        private void FaceDetectPicSave(SK_FVision.HIK_NetSDK.NET_DVR_FACEDETECT_ALARM struAlarm)
        {
            //人脸侦测
            if (struAlarm.dwFacePicDataLen > 0)
            {
                string devIP = struAlarm.struDevInfo.struDevIP.sIpV4.ToString().Replace('.', '_');
                string time = AbsTimetoDatetime(struAlarm.dwAbsTime);

                string str = devIP + "@" + time + ".jpg";
                string strname = StaticInfo.CapturePath + str;
                int iBackgroudLen = (int)struAlarm.dwPicDataLen;
                byte[] byBackgroud = new byte[iBackgroudLen];
                Marshal.Copy(struAlarm.pImage, byBackgroud, 0, iBackgroudLen);
                SK_FCommon.DirFile.CreateFile(strname, byBackgroud, iBackgroudLen);

                string strFace = devIP + "@" + time + "@face.jpg";
                string strnameFace = StaticInfo.CapturePath + strFace;
                int iFaceLen = (int)struAlarm.dwFacePicDataLen;
                byte[] byFace = new byte[iFaceLen];
                Marshal.Copy(struAlarm.pFaceImage, byFace, 0, iFaceLen);
                SK_FCommon.DirFile.CreateFile(strnameFace, byFace, iFaceLen);

            }
        }


        private void ProcessCommAlarm_FaceSNAP(ref SK_FVision.HIK_NetSDK.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser)
        {
            SK_FVision.HIK_NetSDK.NET_VCA_FACESNAP_RESULT struFaceSnap = new SK_FVision.HIK_NetSDK.NET_VCA_FACESNAP_RESULT();

            struFaceSnap = (SK_FVision.HIK_NetSDK.NET_VCA_FACESNAP_RESULT)Marshal.PtrToStructure(pAlarmInfo, typeof(SK_FVision.HIK_NetSDK.NET_VCA_FACESNAP_RESULT));

            if (struFaceSnap.dwBackgroundPicLen > 0 && struFaceSnap.pBuffer2 != null)
            {
                FaceSnapPic(struFaceSnap);
            }
        }

        private void FaceSnapPic(SK_FVision.HIK_NetSDK.NET_VCA_FACESNAP_RESULT struAlarm)
        {
            //人脸抓拍
            if (struAlarm.dwFacePicLen > 0)
            {
                HIK_AlarmInfo iK_AlarmInfo = new HIK_AlarmInfo();

                string devIP = struAlarm.struDevInfo.struDevIP.sIpV4.ToString().Replace('.', '_');
                string time = AbsTimetoDatetime(struAlarm.dwAbsTime).Replace(':', '~');

                string str = devIP + "@" + time + ".jpg";
                string strname = StaticInfo.CapturePath + str;
                int iBackgroudLen = (int)struAlarm.dwBackgroundPicLen;
                byte[] byBackgroud = new byte[iBackgroudLen];
                Marshal.Copy(struAlarm.pBuffer2, byBackgroud, 0, iBackgroudLen);
                SK_FCommon.DirFile.CreateFile(strname, byBackgroud, iBackgroudLen);

                string strFace = devIP + "@" + time + "@face.jpg";
                string strnameFace = StaticInfo.CapturePath + strFace;
                int iFaceLen = (int)struAlarm.dwFacePicLen;
                byte[] byFace = new byte[iFaceLen];
                Marshal.Copy(struAlarm.pBuffer1, byFace, 0, iFaceLen);
                SK_FCommon.DirFile.CreateFile(strnameFace, byFace, iFaceLen);

                System.IO.MemoryStream ms = new System.IO.MemoryStream(byBackgroud);
                iK_AlarmInfo.BackgroudPic = System.Drawing.Image.FromStream(ms);
                System.IO.MemoryStream msface = new System.IO.MemoryStream(byFace);
                iK_AlarmInfo.FacePic = System.Drawing.Image.FromStream(msface);
                iK_AlarmInfo.FaceScore = struAlarm.dwFaceScore;
                iK_AlarmInfo.Abstime = AbsTimetoDatetime(struAlarm.dwAbsTime);
                iK_AlarmInfo.DevIP = struAlarm.struDevInfo.struDevIP.sIpV4.ToString();

                AlarmInfoListQueue(iK_AlarmInfo);
                Event_FaceSnapAlarm?.Invoke(iK_AlarmInfo);
            }
        }


        private void ProcessCommAlarm_SNAPMatch(ref SK_FVision.HIK_NetSDK.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser)
        {
            //回调函数中获取的报警类型(lCommand)为COMM_SNAP_MATCH_ALARM，
            //报警信息(pAlarmInfo)对应结构体：NET_VCA_FACESNAP_MATCH_ALARM。
            //fSimilarity相似度大于0表示人脸比对成功，可以获取人脸库相关数据；
            //fSimilarity相似度为0，需要通过NET_VCA_BLACKLIST_INFO结构体里面的byType类型判断，
            //值为1表示陌生人模式，值为2表示人脸比对失败。
            SK_FVision.HIK_NetSDK.NET_VCA_FACESNAP_MATCH_ALARM struFaceMatchAlarm = new SK_FVision.HIK_NetSDK.NET_VCA_FACESNAP_MATCH_ALARM();

            struFaceMatchAlarm = (SK_FVision.HIK_NetSDK.NET_VCA_FACESNAP_MATCH_ALARM)Marshal.PtrToStructure(pAlarmInfo, typeof(SK_FVision.HIK_NetSDK.NET_VCA_FACESNAP_MATCH_ALARM));

            if (struFaceMatchAlarm.fSimilarity > 0 && struFaceMatchAlarm.struSnapInfo.pBuffer1 != null)
            {
                Facesnap_MathchAsync(struFaceMatchAlarm);
            }

            if (struFaceMatchAlarm.fSimilarity == 0 && struFaceMatchAlarm.struBlackListInfo.struBlackListInfo.byType == 1)
            {
                ////陌生人
                //Facesnap_MathchAsync(struFaceMatchAlarm);
            }
            if (struFaceMatchAlarm.fSimilarity == 0 && struFaceMatchAlarm.struBlackListInfo.struBlackListInfo.byType == 1)
            {
                ////人脸比对失败
                //Facesnap_MathchAsync(struFaceMatchAlarm);
            }
        }

        private void Facesnap_MathchAsync(SK_FVision.HIK_NetSDK.NET_VCA_FACESNAP_MATCH_ALARM struAlarm)
        {
            try
            {
                //人脸抓拍
                if (struAlarm.struSnapInfo.dwSnapFacePicLen > 0)
                {
                    HIK_AlarmInfo iK_AlarmInfo = new HIK_AlarmInfo();

                    int iSnapLen = (int)struAlarm.struSnapInfo.dwSnapFacePicLen;
                    byte[] bySnap = new byte[iSnapLen];
                    Marshal.Copy(struAlarm.pSnapPicBuffer, bySnap, 0, iSnapLen);
                    iK_AlarmInfo.FacePic = Image.FromStream(new System.IO.MemoryStream(bySnap)); ;

                    //int iModelLen = (int)struAlarm.dwModelDataLen;
                    //byte[] byModel = new byte[iModelLen];
                    //Marshal.Copy(struAlarm.pModelDataBuffer, byModel, 0, iModelLen);
                    //System.IO.MemoryStream ms = new System.IO.MemoryStream(byModel);
                    //iK_AlarmInfo.BackgroudPic = System.Drawing.Image.FromStream(ms);

                    iK_AlarmInfo.Abstime = AbsTimetoDatetime(struAlarm.struSnapInfo.dwAbsTime);
                    iK_AlarmInfo.DevIP = struAlarm.struSnapInfo.struDevInfo.struDevIP.sIpV4.ToString();
                    iK_AlarmInfo.FaceScore = (uint)(struAlarm.fSimilarity * 100);
                    iK_AlarmInfo.FaceType = 1;

                    AlarmInfoListQueue(iK_AlarmInfo);
                    Event_FaceSnapAlarm?.Invoke(iK_AlarmInfo);
                }
            }
            catch (Exception _Ex)
            {
                RobotVT.Controller.Methods.SaveExceptionLog(SK_FModel.SystemEnum.LogType.Exception, "人脸抓拍图片转换异常：" + _Ex.Message + "\r\n" + _Ex.StackTrace);
                throw new Exception("人脸抓拍图片转换失败，错误信息：" + _Ex.Message);
            }
        }

        private void Facesnap_MathchBlack(SK_FVision.HIK_NetSDK.NET_VCA_FACESNAP_MATCH_ALARM struAlarm)
        {
            try
            {
                //黑名单
                if (struAlarm.struBlackListInfo.dwBlackListPicLen > 0)
                {
                    HIK_AlarmInfo iK_AlarmInfo = new HIK_AlarmInfo();

                    int iModelLen = (int)struAlarm.struBlackListInfo.dwBlackListPicLen;
                    byte[] byModel = new byte[iModelLen];
                    if (struAlarm.struBlackListInfo.pBuffer1 == IntPtr.Zero) return;
                    Marshal.Copy(struAlarm.struBlackListInfo.pBuffer1, byModel, 0, iModelLen);

                    System.IO.MemoryStream ms = new System.IO.MemoryStream(byModel);
                    iK_AlarmInfo.FacePic = System.Drawing.Image.FromStream(ms);
                    iK_AlarmInfo.Abstime = AbsTimetoDatetime(struAlarm.struSnapInfo.dwAbsTime);
                    iK_AlarmInfo.DevIP = struAlarm.struSnapInfo.struDevInfo.struDevIP.sIpV4.ToString();
                    iK_AlarmInfo.FaceType = struAlarm.struBlackListInfo.struBlackListInfo.byType;

                    AlarmInfoListQueue(iK_AlarmInfo);
                    Event_FaceSnapAlarm?.Invoke(iK_AlarmInfo);
                }
            }
            catch (Exception _Ex)
            {
                RobotVT.Controller.Methods.SaveExceptionLog(SK_FModel.SystemEnum.LogType.Exception, "黑名单图片转换异常：" + _Ex.Message + "\r\n" + _Ex.StackTrace);
                throw new Exception("黑名单图片转换失败，错误信息：" + _Ex.Message);
            }
        }

        private void PostISAPI()
        {
            ////POST / ISAPI / Intelligent / analysisImage / face
            //string strURL = "http://localhost/WinformSubmit.php";
            //System.Net.HttpWebRequest request;
            //request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(strURL);
            ////Post请求方式
            //request.Method = "POST";
            //// 内容类型
            //request.ContentType = "application/x-www-form-urlencoded";

            //byte[] payload;

            ////设置请求的 ContentLength 
            //request.ContentLength = payload.Length;
            ////获得请 求流
            //System.IO.Stream writer = request.GetRequestStream();
            ////将请求参数写入流
            //writer.Write(payload, 0, payload.Length);
            //// 关闭请求流
            //writer.Close();
            //System.Net.HttpWebResponse response;
            //// 获得响应流
            //response = (System.Net.HttpWebResponse)request.GetResponse();
            //System.IO.StreamReader myreader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8);
            //string responseText = myreader.ReadToEnd();
            //myreader.Close();

        }


        private uint m_dwAbilityType = 0;
        private string xmlInput;
        private uint iLastErr = 0;
        public Int32 m_lUserID = -1;
        private void GetDeviceAbility()
        {
            IntPtr pInBuf;
            Int32 nSize;
            if (xmlInput == null)
            {
                pInBuf = IntPtr.Zero;
                nSize = 0;
            }
            else
            {
                nSize = xmlInput.Length;
                pInBuf = Marshal.AllocHGlobal(nSize);
                pInBuf = Marshal.StringToHGlobalAnsi(xmlInput);
            }
            DeviceAbility(12);

            int XML_ABILITY_OUT_LEN = 3 * 1024 * 1024;
            IntPtr pOutBuf = Marshal.AllocHGlobal(XML_ABILITY_OUT_LEN);

            if (!SK_FVision.HIK_NetSDK.NET_DVR_GetDeviceAbility(m_lUserID, m_dwAbilityType, pInBuf, (uint)nSize, pOutBuf, (uint)XML_ABILITY_OUT_LEN))
            {
                iLastErr = SK_FVision.HIK_NetSDK.NET_DVR_GetLastError();
                string strErr = "NET_DVR_GetDeviceAbility failed, error code= " + iLastErr + "\r\n" + GetErrorDescription(iLastErr);
                //获取设备能力集失败，输出错误号 Failed to get the capability set and output the error code
                //textBoxCapability.Text = strErr;
            }
            else
            {
                string strOutBuf = Marshal.PtrToStringAnsi(pOutBuf, XML_ABILITY_OUT_LEN);
                strOutBuf = strOutBuf.Replace(">\n<", ">\r\n<");
                //textBoxCapability.Text = strOutBuf;
            }
            Marshal.FreeHGlobal(pInBuf);
            Marshal.FreeHGlobal(pOutBuf);

        }
        private string GetErrorDescription(uint iErrCode)
        {
            string strDescription = "";
            switch (iErrCode)
            {
                case 1000:
                    strDescription = "设备不支持该能力节点的获取";
                    break;
                case 1001:
                    strDescription = "输出内存不足";
                    break;
                case 1002:
                    strDescription = "无法找到对应的本地xml";
                    break;
                case 1003:
                    strDescription = "加载本地xml出错";
                    break;
                case 1004:
                    strDescription = "设备能力数据格式错误";
                    break;
                case 1005:
                    strDescription = "能力集类型错误";
                    break;
                case 1006:
                    strDescription = "XML能力节点格式错误";
                    break;
                case 1007:
                    strDescription = "输入的能力XML节点值错误";
                    break;
                case 1008:
                    strDescription = "XML版本不匹配";
                    break;
                default:
                    break;
            }
            return strDescription;
        }

        private void DeviceAbility(int index)
        {
            switch (index)
            {
                case 0:	//软硬件能力
                    m_dwAbilityType = SK_FVision.HIK_NetSDK.DEVICE_SOFTHARDWARE_ABILITY;
                    break;
                case 1:	//Wifi能力
                    m_dwAbilityType = SK_FVision.HIK_NetSDK.DEVICE_NETWORK_ABILITY;
                    break;
                case 2: //编码能力
                    m_dwAbilityType = SK_FVision.HIK_NetSDK.DEVICE_ENCODE_ALL_ABILITY_V20;
                    xmlInput = "<AudioVideoCompressInfo><AudioChannelNumber>1</AudioChannelNumber><VoiceTalkChannelNumber>1</VoiceTalkChannelNumber><VideoChannelNumber>1</VideoChannelNumber></AudioVideoCompressInfo>";
                    break;
                case 3:	//设备前端参数能力
                    m_dwAbilityType = SK_FVision.HIK_NetSDK.IPC_FRONT_PARAMETER_V20;
                    xmlInput = "<CAMERAPARA><ChannelNumber>1</ChannelNumber></CAMERAPARA>";
                    break;
                case 4:	//Raid能力
                    m_dwAbilityType = SK_FVision.HIK_NetSDK.DEVICE_RAID_ABILITY;
                    break;
                case 5:	//设备报警能力
                    m_dwAbilityType = SK_FVision.HIK_NetSDK.DEVICE_ALARM_ABILITY;
                    xmlInput = "<AlarmAbility version='2.0'><channelID>1</channelID></AlarmAbility>";
                    break;
                case 6:	//设备数字通道能力
                    m_dwAbilityType = SK_FVision.HIK_NetSDK.DEVICE_DYNCHAN_ABILITY;
                    xmlInput = "<DynChannelAbility version='2.0'><channelNO>1</channelNO></DynChannelAbility>";
                    break;
                case 7: //设备用户管理参数能力
                    m_dwAbilityType = SK_FVision.HIK_NetSDK.DEVICE_USER_ABILITY;
                    xmlInput = "<UserAbility version='2.0'/>";
                    break;
                case 8: //设备网络应用参数能力
                    m_dwAbilityType = SK_FVision.HIK_NetSDK.DEVICE_NETAPP_ABILITY;
                    xmlInput = "<NetAppAbility version='2.0'></NetAppAbility>";
                    break;
                case 9: //设备图像参数能力
                    m_dwAbilityType = SK_FVision.HIK_NetSDK.DEVICE_VIDEOPIC_ABILITY;
                    xmlInput = "<VideoPicAbility version='2.0'><channelNO>1</channelNO></VideoPicAbility>";
                    break;
                case 10: //设备JPEG抓图能力
                    m_dwAbilityType = SK_FVision.HIK_NetSDK.DEVICE_JPEG_CAP_ABILITY;
                    xmlInput = "<JpegCaptureAbility version='2.0'><channelNO>1</channelNO></JpegCaptureAbility>";
                    break;
                case 11: //设备RS232和RS485串口能力
                    m_dwAbilityType = SK_FVision.HIK_NetSDK.DEVICE_SERIAL_ABILITY;
                    xmlInput = "<SerialAbility version='2.0'/>";
                    break;
                case 12: //报警事件处理能力集
                    m_dwAbilityType = SK_FVision.HIK_NetSDK.DEVICE_ABILITY_INFO;
                    xmlInput = "<EventAbility version='2.0'><channelNO>1</channelNO></EventAbility>";
                    break;
                default:
                    m_dwAbilityType = 0;
                    break;
            }
           // 能力集类型：DEVICE_ABILITY_INFO，能力集：EventAbility，节点：< FaceDetection >）
        }

    }
}
