using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

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

        private void ProcessCommAlarm_FaceSNAP(ref SK_FVision.HIK_NetSDK.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser)
        {
            SK_FVision.HIK_NetSDK.NET_VCA_FACESNAP_RESULT struFaceSnap = new SK_FVision.HIK_NetSDK.NET_VCA_FACESNAP_RESULT();

            struFaceSnap = (SK_FVision.HIK_NetSDK.NET_VCA_FACESNAP_RESULT)Marshal.PtrToStructure(pAlarmInfo, typeof(SK_FVision.HIK_NetSDK.NET_VCA_FACESNAP_RESULT));

            if (struFaceSnap.dwBackgroundPicLen > 0 && struFaceSnap.pBuffer2 != null)
            {
                FaceSnapPic(struFaceSnap);
            }
        }

        private void ProcessCommAlarm_SNAPMatch(ref SK_FVision.HIK_NetSDK.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser)
        {
            SK_FVision.HIK_NetSDK.NET_VCA_FACESNAP_MATCH_ALARM struFaceMatchAlarm = new SK_FVision.HIK_NetSDK.NET_VCA_FACESNAP_MATCH_ALARM();

            struFaceMatchAlarm = (SK_FVision.HIK_NetSDK.NET_VCA_FACESNAP_MATCH_ALARM)Marshal.PtrToStructure(pAlarmInfo, typeof(SK_FVision.HIK_NetSDK.NET_VCA_FACESNAP_MATCH_ALARM));

            if (struFaceMatchAlarm.fSimilarity > 0 && struFaceMatchAlarm.pSnapPicBuffer != null && struFaceMatchAlarm.byPicTransType == 0)
            {
                Facesnap_Mathch(struFaceMatchAlarm);
            }
            //保存黑名单人脸图片
            if (struFaceMatchAlarm.struBlackListInfo.dwBlackListPicLen > 0 && struFaceMatchAlarm.struBlackListInfo.pBuffer1 != null)
            {
                Facesnap_MathchBlack(struFaceMatchAlarm);
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
                iK_AlarmInfo.Abstime = AbsTimetoDatetime(struAlarm.dwAbsTime);
                iK_AlarmInfo.DevIP = struAlarm.struDevInfo.struDevIP.sIpV4.ToString();

                if (AlarmInfoList.Count <= 0 || AlarmInfoList.Count % 8 == 0)
                {
                    iK_AlarmInfo.QueueNubmer = 1;
                }
                else if (AlarmInfoList.Count > 0)
                {
                    iK_AlarmInfo.QueueNubmer = AlarmInfoList.Count % 8 + 1;
                }

                AlarmInfoList.Enqueue(iK_AlarmInfo);
                Event_FaceSnapAlarm?.Invoke(iK_AlarmInfo);
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
        
        private void Facesnap_Mathch(SK_FVision.HIK_NetSDK.NET_VCA_FACESNAP_MATCH_ALARM struAlarm)
        {
            //人脸抓拍
            if (struAlarm.struSnapInfo.dwSnapFacePicLen > 0)
            {
                HIK_AlarmInfo iK_AlarmInfo = new HIK_AlarmInfo();

                string devIP = struAlarm.struSnapInfo.struDevInfo.struDevIP.sIpV4.ToString().Replace('.', '_');
                string time = AbsTimetoDatetime(struAlarm.struSnapInfo.dwAbsTime).Replace(':', '~');
                
                string str = devIP + "@" + time + "@face.jpg";
                string strname = StaticInfo.CapturePath + str;
                int iSnapLen = (int)struAlarm.dwSnapPicLen;
                byte[] bySnap = new byte[iSnapLen];
                Marshal.Copy(struAlarm.pSnapPicBuffer, bySnap, 0, iSnapLen);
                //SK_FCommon.DirFile.CreateFile(strname, bySnap, iSnapLen);

                string strFace = devIP + "@" + time + "@model.jpg";
                string strnameModel = StaticInfo.CapturePath + strFace;
                int iModelLen = (int)struAlarm.dwModelDataLen;
                byte[] byModel = new byte[iModelLen];
                Marshal.Copy(struAlarm.pModelDataBuffer, byModel, 0, iModelLen);
                //SK_FCommon.DirFile.CreateFile(strnameModel, byModel, iModelLen);

                System.IO.MemoryStream ms = new System.IO.MemoryStream(byModel);
                iK_AlarmInfo.BackgroudPic = System.Drawing.Image.FromStream(ms);
                System.IO.MemoryStream msface = new System.IO.MemoryStream(bySnap);
                iK_AlarmInfo.FacePic = System.Drawing.Image.FromStream(msface);
                iK_AlarmInfo.Abstime = AbsTimetoDatetime(struAlarm.struSnapInfo.dwAbsTime);
                iK_AlarmInfo.DevIP = struAlarm.struSnapInfo.struDevInfo.struDevIP.sIpV4.ToString();
                

                if (AlarmInfoList.Count <= 0 || AlarmInfoList.Count % 8 == 0)
                {
                    iK_AlarmInfo.QueueNubmer = 1;
                }
                else if (AlarmInfoList.Count > 0)
                {
                    iK_AlarmInfo.QueueNubmer = AlarmInfoList.Count % 8 + 1;
                }

                AlarmInfoList.Enqueue(iK_AlarmInfo);
                Event_FaceSnapAlarm?.Invoke(iK_AlarmInfo);
            }
        }

        private void Facesnap_MathchBlack(SK_FVision.HIK_NetSDK.NET_VCA_FACESNAP_MATCH_ALARM struAlarm)
        {
            //人脸抓拍
            if (struAlarm.struSnapInfo.dwSnapFacePicLen > 0)
            {
                HIK_AlarmInfo iK_AlarmInfo = new HIK_AlarmInfo();

                string devIP = struAlarm.struSnapInfo.struDevInfo.struDevIP.sIpV4.ToString().Replace('.', '_');
                string time = AbsTimetoDatetime(struAlarm.struSnapInfo.dwAbsTime).Replace(':', '~');

                string str = devIP + "@" + time + "@face.jpg";
                string strname = StaticInfo.CapturePath + str;
                int iSnapLen = (int)struAlarm.dwSnapPicLen;
                byte[] bySnap = new byte[iSnapLen];
                Marshal.Copy(struAlarm.pSnapPicBuffer, bySnap, 0, iSnapLen);
                //SK_FCommon.DirFile.CreateFile(strname, bySnap, iSnapLen);

                string strFace = devIP + "@" + time + "@black.jpg";
                string strnameModel = StaticInfo.CapturePath + strFace;
                int iModelLen = (int)struAlarm.struBlackListInfo.dwBlackListPicLen;
                byte[] byModel = new byte[iModelLen];
                Marshal.Copy(struAlarm.struBlackListInfo.pBuffer1, byModel, 0, iModelLen);
                //SK_FCommon.DirFile.CreateFile(strnameModel, byModel, iModelLen);

                System.IO.MemoryStream ms = new System.IO.MemoryStream(byModel);
                iK_AlarmInfo.BackgroudPic = System.Drawing.Image.FromStream(ms);
                System.IO.MemoryStream msface = new System.IO.MemoryStream(bySnap);
                iK_AlarmInfo.FacePic = System.Drawing.Image.FromStream(msface);
                iK_AlarmInfo.Abstime = AbsTimetoDatetime(struAlarm.struSnapInfo.dwAbsTime);
                iK_AlarmInfo.DevIP = struAlarm.struSnapInfo.struDevInfo.struDevIP.sIpV4.ToString();


                if (AlarmInfoList.Count <= 0 || AlarmInfoList.Count % 8 == 0)
                {
                    iK_AlarmInfo.QueueNubmer = 1;
                }
                else if (AlarmInfoList.Count > 0)
                {
                    iK_AlarmInfo.QueueNubmer = AlarmInfoList.Count % 8 + 1;
                }

                AlarmInfoList.Enqueue(iK_AlarmInfo);
                Event_FaceSnapAlarm?.Invoke(iK_AlarmInfo);
            }
        }
    }
}
