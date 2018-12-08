using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

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
            //lock (_listImage)
            //{
            _listImage.Enqueue(image);
            //}
        }

        private int ImagePoolCoount
        {
            get
            {
                //lock (_listImage)
                //{
                return _listImage.Count;
                //}
            }
        }

        private Image GetImage()
        {
            //lock (_listImage)
            //{
            if (_listImage.Count == 0)
                return null;

            Image result = _listImage.Dequeue();
            return result;
            //}
        }
    }
}
