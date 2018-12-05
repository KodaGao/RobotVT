using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK_FVision
{
    class VideoFileRead
    {
        public event Action<VideoFileRead, Bitmap> OnDecodeVideo;
        VideoDecodeInfo _decode = new VideoDecodeInfo();

        public void ReadFile(string file)
        {
            byte[] videoBytes = GetBytesFromFile(file);

            int offset = 0;
            byte[] h264Frame = null;
            int count = 0;
            while (true)
            {
                h264Frame = GetNextFrame(videoBytes, ref offset);
                if (h264Frame == null)
                    break;
                PutToDecode(h264Frame);
                count++;

                Bitmap bitmap = GetNextBitmap();
                if(bitmap != null)
                {
                    OnDecodeVideo?.Invoke(this, bitmap);
                }
            }
        }


        public void PutToDecode(byte[] h264Frame)
        {
            _decode.PutToDecode(h264Frame);
            Bitmap bitmap = GetNextBitmap();
            if (bitmap != null)
            {
                OnDecodeVideo?.Invoke(this, bitmap);
            }
        }

        public Bitmap GetNextBitmap()
        {
            Bitmap bitmap = _decode.GetNextBitmapNew();
            return bitmap;
        }

        private byte[] GetNextFrame(byte[] videoBytes, ref int offset)
        {
            int startIndex = -1;

            for (int i = offset + 2; i < videoBytes.Length; i++)
            {
                byte b1 = videoBytes[i - 2];
                byte b2 = videoBytes[i - 1];
                byte b3 = videoBytes[i];

                if (startIndex == -1)
                {
                    if (b1 == 0x00 && b2 == 0x00 && b3 == 0x01)
                    {
                        startIndex = i - 2;
                    }
                }
                else
                {
                    if (b1 == 0x00 && b2 == 0x00 && (b3 == 0x01 || b3 == 0x01))
                    {
                        offset = i - 2;
                        if (videoBytes[i - 3] == 0)
                            offset--;

                        int len = offset - startIndex;
                        byte[] result = new byte[len];
                        Buffer.BlockCopy(videoBytes, startIndex, result, 0, len);
                        return result;
                    }
                }
            }

            offset = videoBytes.Length;
            return null;
        }

        public static byte[] GetBytesFromFile(string filePath)
        {
            int offset = 0;// 15811;
            using (FileStream fs = new FileStream(filePath, FileMode.Open))//初始化文件流
            {
                byte[] array = new byte[fs.Length - offset];//初始化字节数组
                fs.Position = offset;
                fs.Read(array, 0, array.Length - offset);//读取流中数据到字节数组中
                fs.Close();//关闭流        
                return array;
            }
        }
    }
}
