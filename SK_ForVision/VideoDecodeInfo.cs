using SK_FVision.FFmpegWarper;
using System;
using System.Drawing;
using static SK_FVision.FFmpegWarper.H264DecodeWrapper;

namespace SK_FVision
{
    class VideoDecodeInfo
    {
        public delegate void ShowBitmap(Bitmap bitmap);
        public int Decode_Result_OK = 0;

        int _videoWidthOrg;
        int _videoHeightOrg;
        public int VideoWidthOrg => _videoWidthOrg;
        public int VideoHeightOrg => _videoHeightOrg;

        public int _frameSizeYuv = 0;

        public int _videoWidthRgb;
        public int _videoHeightRgb;
        public int _frameSizeRgb = 0;
        byte[] _yuvFrame = null;
        byte[] _rgbFrame = null;

        H264Decode _decode = new H264Decode();

        public bool Dispose { get; set; } = false;

        public void StartDecode(byte[] h264Frame)
        {
            if(h264Frame!=null)
            {
                PutToDecode(h264Frame);
                Bitmap bitmap = GetNextBitmapNew();
            }
        }


        public VideoDecodeInfo()
        {
            _decode.Init();
        }

        ~VideoDecodeInfo()
        {
            _decode.Close();
        }

        public void PutToDecode(byte[] h264Frame)
        {
            _decode.PutVideoStream(h264Frame);
        }

        void GetVideoOrgSize()
        {
            if (_videoWidthOrg == 0)
            {
                _decode.GetVideoOrgSize(out _videoWidthOrg, out _videoHeightOrg);
                if (_videoWidthOrg != 0)
                {
                    _videoWidthRgb = _videoWidthOrg;
                    _videoHeightRgb = _videoHeightOrg;
                }
            }
        }


        byte[] CreateVideoFrame_Rgb()
        {
            if (_videoWidthOrg == 0)
            {
                GetVideoOrgSize();
                if (_videoWidthOrg == 0)
                {
                    return null;
                }
            }

            int n = _decode.GetVideoFrameSize_Rgb2(_videoWidthRgb, _videoHeightRgb);
            return new byte[n];
        }

        public byte[] CreateVideoFrame()
        {
            if (_frameSizeYuv == 0)
            {
                _frameSizeYuv = _decode.GetVideoFrameSize();
                if (_frameSizeYuv == 0)
                    return null;

                GetVideoOrgSize();
            }
            return new byte[_frameSizeYuv];
        }

        public byte[] GetYuvData()
        {
            if (_yuvFrame == null)
            {
                _yuvFrame = CreateVideoFrame();
                if (_yuvFrame == null)
                    return null;
            }

            if (!GetYuvData(_yuvFrame))
                return null;
            return _yuvFrame;
        }

        bool GetYuvData(byte[] yuvFrame)
        {
            int ret = _decode.GetNextVideoFrame(yuvFrame, yuvFrame.Length, EN_H264_YU_Formate.Y_V_U);
            if (ret != H264Decode.Decode_Result_OK)
                return false;
            return true;
        }


        public byte[] GetRgbData()
        {
            byte[] yuvData = GetYuvData();
            if (yuvData == null)
                return null;

            byte[] rgbData = ImageConvert.I420ToRGB(yuvData, _videoWidthOrg, _videoHeightOrg);
            return rgbData;
        }

        public byte[] GetRgbData_Ffmpeg()
        {
            if (_rgbFrame == null)
            {
                _rgbFrame = CreateVideoFrame_Rgb();
                if (_rgbFrame == null)
                    return null;
            }

            int ret = _decode.GetNextVideoFrame_Rgb2(_rgbFrame, _rgbFrame.Length,
                             _videoWidthRgb, _videoHeightRgb);

            if (ret != H264Decode.Decode_Result_OK)
                return null;
            return _rgbFrame;
        }

        public Bitmap GetNextBitmapNew()
        {
            byte[] rgb = GetRgbData_Ffmpeg();
            if (rgb == null)
                return null;

            Bitmap result = ImageConvert.RgbToBitmap(rgb, _videoWidthRgb, _videoHeightRgb);
            return result;
        }
        
        internal void Close()
        {
            _decode.Close();
        }
    }
}
