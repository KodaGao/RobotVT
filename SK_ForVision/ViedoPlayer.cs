using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace SK_FVision
{
    public partial class ViedoPlayer : UserControl
    {
        public ViedoPlayer()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | //不擦除背景 ,减少闪烁
             ControlStyles.OptimizedDoubleBuffer | //双缓冲
             ControlStyles.UserPaint, //使用自定义的重绘事件,减少闪烁
             true);

            InitializeComponent();
            this.Load += new System.EventHandler(this.ViedoPlayer_Load);
            RealPlayWnd.MouseDown += new MouseEventHandler(this.RealPlayWnd_MouseDown); 
            RealPlayWnd.MouseUp += new MouseEventHandler(this.RealPlayWnd_MouseUp);
            RealPlayWnd.MouseDoubleClick += new MouseEventHandler(this.RealPlayWnd_MouseDoubleClick);
            RealPlayWnd.MouseMove += new MouseEventHandler(this.RealPlayWnd_MouseMove);
        }


        public virtual void ViedoPlayer_Load(object sender, EventArgs e)
        {
        }

        public virtual void RealPlayWnd_MouseMove(object sender, MouseEventArgs e)
        { }

        public virtual void RealPlayWnd_MouseDoubleClick(object sender, MouseEventArgs e)
        { }

        public virtual void RealPlayWnd_MouseUp(object sender, MouseEventArgs e)
        { }

        public virtual void RealPlayWnd_MouseDown(object sender, MouseEventArgs e)
        {
        }

        /// <summary>
        /// 焦距变大
        /// </summary>
        /// <param name="lUserID"></param>
        /// <param name="lChannel"></param>
        /// <param name="bAuto"></param>
        /// <param name="lRealHandle">摄像机句柄</param>
        /// <param name="dwstop">0，开始；1，停止</param>
        public void PTZControl_ZoomIn(int lUserID, int lChannel, bool bAuto, int lRealHandle, uint dwstop)
        {
            if (bAuto)
            {
                HIK_NetSDK.NET_DVR_PTZControl(lRealHandle, HIK_NetSDK.ZOOM_IN, dwstop);
            }
            else
            {
                HIK_NetSDK.NET_DVR_PTZControl_Other(lUserID, lChannel, HIK_NetSDK.ZOOM_IN, dwstop);
            }
        }
        /// <summary>
        /// 焦距变小
        /// </summary>
        /// <param name="lUserID"></param>
        /// <param name="lChannel"></param>
        /// <param name="bAuto"></param>
        /// <param name="lRealHandle">摄像机句柄</param>
        /// <param name="dwstop">0，开始；1，停止</param>
        public void PTZControl_ZoomOut(int lUserID, int lChannel, bool bAuto, int lRealHandle, uint dwstop)
        {
            if (bAuto)
            {
                HIK_NetSDK.NET_DVR_PTZControl(lRealHandle, HIK_NetSDK.ZOOM_OUT, dwstop);
            }
            else
            {
                HIK_NetSDK.NET_DVR_PTZControl_Other(lUserID, lChannel, HIK_NetSDK.ZOOM_OUT, dwstop);
            }
        }
        /// <summary>
        /// 焦距前调
        /// </summary>
        /// <param name="lUserID"></param>
        /// <param name="lChannel"></param>
        /// <param name="bAuto"></param>
        /// <param name="lRealHandle">摄像机句柄</param>
        /// <param name="dwstop">0，开始；1，停止</param>
        public void PTZControl_FocusNear(int lUserID, int lChannel, bool bAuto, int lRealHandle, uint dwstop)
        {
            if (bAuto)
            {
                HIK_NetSDK.NET_DVR_PTZControl(lRealHandle, HIK_NetSDK.FOCUS_NEAR, dwstop);
            }
            else
            {
                HIK_NetSDK.NET_DVR_PTZControl_Other(lUserID, lChannel, HIK_NetSDK.FOCUS_NEAR, dwstop);
            }
        }
        /// <summary>
        /// 焦距后调
        /// </summary>
        /// <param name="lUserID"></param>
        /// <param name="lChannel"></param>
        /// <param name="bAuto"></param>
        /// <param name="lRealHandle">摄像机句柄</param>
        /// <param name="dwstop">0，开始；1，停止</param>
        public void PTZControl_FocusFar(int lUserID, int lChannel, bool bAuto, int lRealHandle, uint dwstop)
        {
            if (bAuto)
            {
                HIK_NetSDK.NET_DVR_PTZControl(lRealHandle, HIK_NetSDK.FOCUS_FAR, dwstop);
            }
            else
            {
                HIK_NetSDK.NET_DVR_PTZControl_Other(lUserID, lChannel, HIK_NetSDK.FOCUS_FAR, dwstop);
            }
        }

        public bool CanPlay { set; get; } = false;


        private VideoDecodeInfo H264_Decode;
        public int ImagePlaySpan { get; set; } = 25; //图片播放间隔 毫秒 每秒40帧
        Queue<Image> _listImage;
        public int pWidth = 0, pHeight = 0;

        public virtual void InitH264Thread()
        {
            H264_Decode = new SK_FVision.VideoDecodeInfo();
            _listImage = new Queue<Image>();


            CanPlay = true;
            System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(RecvThread));
            t.IsBackground = true;
            t.Start();

        }

        public void DecodeH264(byte[] h264frame)
        {
            if (H264_Decode == null) return;
            H264_Decode.PutToDecode(h264frame);
            Bitmap bitmap = H264_Decode.GetNextBitmapNew();
            if (bitmap != null)
            {
                //防止读取到内存的数据太多
                while (ImagePoolCoount > 200)
                {
                    Thread.Sleep(1);
                }

                AddImage(bitmap);
            }
        }

        public void GetPictureSize()
        {
            pWidth = H264_Decode._videoWidthRgb;
            pHeight = H264_Decode._videoHeightRgb;
        }

        public void StopH264Thread()
        {
            CanPlay = false;
            RealPlayWnd.Image = null;
            _listImage.Clear();
        }

        private void RecvThread()
        {
            while (CanPlay)
            {
                this.BeginInvoke((Action)(() =>
                {
                    Image image = GetImage();
                    if (image != null)
                        PlayNewImage(image);
                }));
                Thread.Sleep(ImagePlaySpan);
            }
        }

        private void PlayNewImage(Image image)
        {
            if (RealPlayWnd.Image != null)
            {
                RealPlayWnd.Image.Dispose();
            }

            RealPlayWnd.Image = image;
        }

        private void AddImage(Image image)
        {
            lock (_listImage)
            {
                _listImage.Enqueue(image);
            }
        }

        private int ImagePoolCoount
        {
            get
            {
                lock (_listImage)
                {
                    return _listImage.Count;
                }
            }
        }

        private Image GetImage()
        {
            lock (_listImage)
            {
                if (_listImage.Count == 0)
                    return null;

                Image result = _listImage.Dequeue();
                return result;
            }
        }
    }
}
