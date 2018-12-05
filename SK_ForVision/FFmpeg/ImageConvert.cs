using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SK_FVision.FFmpegWarper
{
    class ImageConvert
    {
        private static int R = 0;
        private static int G = 1;
        private static int B = 2;

        public static BitmapSource I420ToBitmapSource(byte[] yvuData, int width, int height)
        {
            byte[] rgbData = I420ToRGB(yvuData, width, height);
            using (Bitmap bitmap = RgbToBitmap(rgbData, width, height))
            {
                BitmapSource bitmapSource = ToBitmapSource(bitmap);
                return bitmapSource;
            }
        }

        public static Bitmap I420ToBitmap(byte[] yvuData, int width, int height)
        {
            byte[] rgbData = I420ToRGB(yvuData, width, height);
            Bitmap bitmap = RgbToBitmap(rgbData, width, height);
            return bitmap;
        }

        public static int CalRgbPixelCount(int width, int height)
        {
            return (width * height*3);
        }

        public static byte[] I420ToRGB(byte[] src, int width, int height)
        {
            int count = CalRgbPixelCount(width, height);
            byte[] rgb = new byte[count];
            return I420ToRGB(src,width,height, rgb);
        }

        //I420是yuv420格式，是3个plane，排列方式为(Y)(U)(V)  
        public static byte[] I420ToRGB(byte[] src, int width, int height, byte[] rgb)
        {
            RGB rgbTmp = new RGB();

            int numOfPixel = width * height;
            int positionOfV = numOfPixel;
            int positionOfU = numOfPixel / 4 + numOfPixel;
            for (int i = 0; i < height; i++)
            {
                int startY = i * width;
                int step = (i / 2) * (width / 2);
                int startU = positionOfV + step;
                int startV = positionOfU + step;
                for (int j = 0; j < width; j++)
                {
                    int Y = startY + j;
                    int U = startU + j / 2;
                    int V = startV + j / 2;
                    int index = Y * 3;
                    RGB tmp = YuvToRgb(src[Y], src[U], src[V], ref rgbTmp);
                    rgb[index + R] = tmp.r;
                    rgb[index + G] = tmp.g;
                    rgb[index + B] = tmp.b;
                }
            }

            return rgb;
        }

        private class RGB
        {
            public byte r;
            public byte g;
            public byte b;
        }

        private static RGB YuvToRgb(byte Y, byte U, byte V, ref RGB rgb)
        {
            int r = (int)((Y & 0xff) + 1.4075 * ((V & 0xff) - 128));
            int g = (int)((Y & 0xff) - 0.3455 * ((U & 0xff) - 128) - 0.7169 * ((V & 0xff) - 128));
            int b = (int)((Y & 0xff) + 1.779 * ((U & 0xff) - 128));
            rgb.r = (byte)(r < 0 ? 0 : r > 255 ? 255 : r);
            rgb.g = (byte)(g < 0 ? 0 : g > 255 ? 255 : g);
            rgb.b = (byte)(b < 0 ? 0 : b > 255 ? 255 : b);
            return rgb;
        }

        public static Bitmap RgbToBitmap(byte[] buffer, int width, int height)
        {
            try
            {
                Bitmap bitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                Rectangle m_rect = new Rectangle(0, 0, width, height);
                BitmapData m_bitmapData = bitmap.LockBits(m_rect, ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                IntPtr iptr = m_bitmapData.Scan0;  // 获取bmpData的内存起始位置  

                //// 用Marshal的Copy方法，将刚才得到的内存字节数组复制到BitmapData中  
                System.Runtime.InteropServices.Marshal.Copy(buffer, 0, iptr, width * height * 3);

                bitmap.UnlockBits(m_bitmapData);
                return bitmap;
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

        public static BitmapSource RgbToBitmapSource(byte[] buffer, int width, int height)
        {
            //wpf中的类，winform中没有
            //try
            //{
            //    GCHandle hin = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            //    BitmapSource source = CreateBitmapSourceFromMemorySection(hin.AddrOfPinnedObject(), width, height, PixelFormats.Rgb24, width *3,0);
            //    hin.Free();
            //    return source;
            //}
            //catch (System.Exception ex)
            //{
            //    return null;
            //}
            return null;
        }

        public static WriteableBitmap RgbToWriteableBitmap(byte[] buffer, int width, int height)
        {
            //wpf中的类，winform中没有
            //try
            //{
            //    WriteableBitmap bitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgr24, null);
            //    // bitmap.WritePixels();
            //    bitmap.WritePixels(new Int32Rect(0, 0, width, height), buffer, bitmap.BackBufferStride, 0);
            //    //Marshal.Copy(bitmap.BackBuffer, buffer, 0, buffer.Length);
            //    return bitmap;
            //}
            //catch (System.Exception ex)
            //{
            //    return null;
            //}
            return null;
        }


        public static BitmapSource BitmapToBitmapSource_Old(Bitmap source)
        {
            //wpf中的类，winform中没有
            //IntPtr ptr = source.GetHbitmap(); //obtain the Hbitmap
            //BitmapSource bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
            //    ptr,
            //    IntPtr.Zero,
            //    Int32Rect.Empty,
            //    System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

            //DeleteObject(ptr); //release the HBitmap
            //return bs;
            return null;
        }


        /// <summary>
        /// Convert an IImage to a WPF BitmapSource. The result can be used in the Set Property of Image.Source
        /// </summary>
        /// <param name="image">The Emgu CV Image</param>
        /// <returns>The equivalent BitmapSource</returns>
        public static BitmapSource ToBitmapSource(Bitmap bitmap)
        {
            //IntPtr ptr = bitmap.GetHbitmap(); //obtain the Hbitmap
            //BitmapSource bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
            //    ptr,
            //    IntPtr.Zero,
            //    Int32Rect.Empty,
            //    System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

            //DeleteObject(ptr); //release the HBitmap
            //return bs;
            return null;
        }
     
        public static BitmapSource ToBitmapSourceFast(System.Drawing.Bitmap bitmap)
        {
            var bitmapData = bitmap.LockBits(
                new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);

            //var bitmapSource = BitmapSource.Create(
            //    bitmapData.Width, bitmapData.Height,
            //    bitmap.HorizontalResolution, bitmap.VerticalResolution,
            //    PixelFormats.Bgr24, null,
            //    bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);

            //bitmap.UnlockBits(bitmapData);
            //return bitmapSource;
            return null;
        }

        public static void SaveBitmapImageIntoFile(System.Drawing.Bitmap bitmapImage, string filePath)
        {
            bitmapImage.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
        }

        /// <summary>
        /// Delete a GDI object
        /// </summary>
        /// <param name="o">The poniter to the GDI object to be deleted</param>
        /// <returns></returns>
        [DllImport("gdi32")]
        private static extern int DeleteObject(IntPtr o);
    }
}
