using System;
using System.Drawing;

namespace RobotVT.Controller
{
    public class HIK_AlarmInfo
    {
        public int QueueNubmer;
        public Image BackgroudPic;
        public Image FacePic;
        public Image FaceModelPic;
        public string Abstime;
        public string DevIP;
        public uint FaceScore;
        public int FaceType;//0,黑名单；1，人脸比对成功；
        public string Name;
        public string CertificateNumber;

        //public byte byType;//黑白名单标志：0-全部，1-白名单，2-黑名单
        //public byte byLevel;//黑名单等级，0-全部，1-低，2-中，3-高
    }

}