using System;
using System.Runtime.InteropServices;
using static SK_FVision.FFmpegWarper.H264DecodeWrapper;

namespace SK_FVision.FFmpegWarper
{
    class H264Decode
    {
        public const int Decode_Result_OK = 0;

        long _handleDecode = 0;

        public static void SetDefaultAlgorithm(EN_H264Algorithm flag)
        {
            H264DecodeWrapper.SetDefaultAlgorithm((int)flag);
        }

        ~H264Decode()
        {
            Close();
        }

        public void Init()
        {
            _handleDecode = H264DecodeWrapper.CreateHandle();
        }

        public Int32 SetAlgorithm(EN_H264Algorithm flag)
        {
            return H264DecodeWrapper.SetAlgorithm(_handleDecode, (int)flag);
        }

        public void Close()
        {
            if (_handleDecode == 0)
                return;
            H264DecodeWrapper.CloseHandle(_handleDecode);
            _handleDecode = 0;
        }

        public int PutVideoStream(byte[] buffer)
        {
            return PutVideoStream(buffer, buffer.Length);
        }

        public int PutVideoStream(byte[] buffer, Int32 bufferLen)
        {
            return H264DecodeWrapper.PutVideoStream(_handleDecode, buffer, bufferLen);
        }

        public int GetVideoOrgSize(out int width, out int height)
        {
            int result = H264DecodeWrapper.GetVideoParam(_handleDecode, out width, out height);
            return result;
        }

        public int GetVideoFrameSize()
        {
            int result = H264DecodeWrapper.GetVideoFrameSize(_handleDecode);
            return result;
        }
        public AVPixelFormat GetVideoFrameFormate()
        {
            int result = H264DecodeWrapper.GetVideoFrameFormate(_handleDecode);
            return (AVPixelFormat)result;
        }

        public int GetNextVideoFrame(byte[] buffer, Int32 bufferLen, EN_H264_YU_Formate formate)
        {
            int result = H264DecodeWrapper.GetNextVideoFrame(_handleDecode, buffer, bufferLen, formate);
            return result;
        }
        public int GetNextVideoFrame(byte[] buffer, EN_H264_YU_Formate formate)
        {
            int result = H264DecodeWrapper.GetNextVideoFrame(_handleDecode, buffer, buffer.Length, formate);
            return result;
        }

        public int GetVideoFrameSize_Rgb()
        {
            int result = H264DecodeWrapper.GetVideoFrameSize_Rgb(_handleDecode);
            return result;
        }

        public int GetNextVideoFrame_Rgb(byte[] buffer, Int32 bufferLen)
        {
            int result = H264DecodeWrapper.GetNextVideoFrame_Rgb(_handleDecode, buffer, bufferLen);
            return result;
        }

        public int GetVideoFrameSize_Rgb2(Int32 width, Int32 height)
        {
            int result = H264DecodeWrapper.GetVideoFrameSize_Rgb2(_handleDecode, width, height);
            return result;
        }

        public int GetNextVideoFrame_Rgb2(byte[] buffer, Int32 bufferLen, Int32 width, Int32 height)
        {
            int result = H264DecodeWrapper.GetNextVideoFrame_Rgb2(_handleDecode, buffer, bufferLen, width, height);
            return result;
        }

    }

    public enum EN_H264Algorithm
    {
        SWS_FAST_BILINEAR = 1,
        SWS_BILINEAR = 2,
        SWS_BICUBIC = 4,
        SWS_X = 8,
        SWS_POINT = 0x10,
        SWS_AREA = 0x20,
        SWS_BICUBLIN = 0x40,
        SWS_GAUSS = 0x80,
        SWS_SINC = 0x100,
        SWS_LANCZOS = 0x200,
        SWS_SPLINE = 0x400,
    }

    public class H264DecodeWrapper
    {
        private const string DLLName = @"ffmpeg\bin\x64\LibFfmpegWrapper.dll";

        [DllImport(DLLName, EntryPoint = "H264_CreateHandle", CallingConvention = CallingConvention.Cdecl)]
        private static extern long H264_CreateHandle();

        [DllImport(DLLName, EntryPoint = "H264_CloseHandle", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 H264_CloseHandle(long handle);

        [DllImport(DLLName, EntryPoint = "H264_PutVideoStream", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 H264_PutVideoStream(long handle, IntPtr buffer, Int32 bufferLen);

        [DllImport(DLLName, EntryPoint = "H264_GetVideoParam", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 H264_GetVideoParam(long handle, IntPtr width, IntPtr height);

        [DllImport(DLLName, EntryPoint = "H264_GetVideoFrameSize", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 H264_GetVideoFrameSize(long handle);

        [DllImport(DLLName, EntryPoint = "H264_GetVideoFrameFormate", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 H264_GetVideoFrameFormate(long handle);

        [DllImport(DLLName, EntryPoint = "H264_GetNextVideoFrame", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 H264_GetNextVideoFrame(long handle, IntPtr buffer, Int32 bufferLen, Int32 yuFormate);


        [DllImport(DLLName, EntryPoint = "H264_GetVideoFrameSize_Rgb", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 H264_GetVideoFrameSize_Rgb(long handle);

        [DllImport(DLLName, EntryPoint = "H264_GetNextVideoFrame_Rgb", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 H264_GetNextVideoFrame_Rgb(long handle, IntPtr buffer, Int32 bufferLen);


        [DllImport(DLLName, EntryPoint = "H264_GetVideoFrameSize_Rgb2", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 H264_GetVideoFrameSize_Rgb2(long handle, Int32 width, Int32 height);

        [DllImport(DLLName, EntryPoint = "H264_GetNextVideoFrame_Rgb2", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 H264_GetNextVideoFrame_Rgb2(long handle, IntPtr buffer, Int32 bufferLen, Int32 width, Int32 height);

        [DllImport(DLLName, EntryPoint = "H264_SetDefaultAlgorithm", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 H264_SetDefaultAlgorithm(Int32 flag);

        [DllImport(DLLName, EntryPoint = "H264_SetAlgorithm", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 H264_SetAlgorithm(long handle,Int32 flag);


        public static long CreateHandle()
        {
            return H264_CreateHandle();
        }

        public static long CloseHandle(long handle)
        {
            return H264_CloseHandle(handle);
        }

        public static int PutVideoStream(long handle, byte[] buffer, Int32 bufferLen)
        {
            GCHandle hin = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            int result = H264_PutVideoStream(handle, hin.AddrOfPinnedObject(), bufferLen);
            hin.Free();
            return result;
        }

        public static int GetVideoParam(long handle, out int width, out int height)
        {
            width = 0;
            height = 0;
            byte[] width2 = new byte[4];
            byte[] height2 = new byte[4];

            GCHandle hin_width = GCHandle.Alloc(width2, GCHandleType.Pinned);
            GCHandle hin_height = GCHandle.Alloc(height2, GCHandleType.Pinned);
            int result = H264_GetVideoParam(handle, hin_width.AddrOfPinnedObject(), hin_height.AddrOfPinnedObject());
            hin_width.Free();
            hin_height.Free();

            if (result != 0)
            {
                return result;
            }

            width = BitConverter.ToInt32(width2, 0);
            height = BitConverter.ToInt32(height2, 0);
            return result;
        }

        public static int GetVideoFrameSize(long handle)
        {
            int result = H264_GetVideoFrameSize(handle);
            return result;
        }
        public static int GetVideoFrameFormate(long handle)
        {
            int result = H264_GetVideoFrameFormate(handle);
            return result;
        }

        public static int GetNextVideoFrame(long handle, byte[] buffer, Int32 bufferLen, EN_H264_YU_Formate formate)
        {
            GCHandle hin = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            int result = H264_GetNextVideoFrame(handle, hin.AddrOfPinnedObject(), bufferLen, (int)formate);
            hin.Free();
            return result;
        }

        public static int GetVideoFrameSize_Rgb(long handle)
        {
            int result = H264_GetVideoFrameSize_Rgb(handle);
            return result;
        }

        public static int GetNextVideoFrame_Rgb(long handle, byte[] buffer, Int32 bufferLen)
        {
            GCHandle hin = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            int result = H264_GetNextVideoFrame_Rgb(handle, hin.AddrOfPinnedObject(), bufferLen);
            hin.Free();
            return result;
        }

        public static int GetVideoFrameSize_Rgb2(long handle, Int32 width, Int32 height)
        {
            int result = H264_GetVideoFrameSize_Rgb2(handle, width, height);
            return result;
        }

        public static  Int32 SetDefaultAlgorithm(Int32 flag)
        {
            return H264_SetDefaultAlgorithm(flag);
        }

        public static Int32 SetAlgorithm(long handle, Int32 flag)
        {
            return H264_SetAlgorithm(handle,flag);
        }

        public static int GetNextVideoFrame_Rgb2(long handle, byte[] buffer, Int32 bufferLen, Int32 width, Int32 height)
        {
            GCHandle hin = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            int result = H264_GetNextVideoFrame_Rgb2(handle, hin.AddrOfPinnedObject(), bufferLen, width, height);
            hin.Free();
            return result;
        }

        public enum EN_H264_YU_Formate
        {
            Y_U_V = 1,
            Y_V_U = 2,
        }
    }
}

//LibFfmpegWrapper_API INT64 H264_CreateHandle();
//LibFfmpegWrapper_API INT32 H264_CloseHandle(INT64 handle);

//LibFfmpegWrapper_API INT32 H264_PutVideoStream(INT64 handle, char* buffer, INT32 bufferLen);
//LibFfmpegWrapper_API INT32 H264_GetVideoParam(INT64 handle, INT32& width, INT32& height);
//LibFfmpegWrapper_API INT32 H264_GetVideoFrameSize(INT64 handle);
//LibFfmpegWrapper_API INT32 H264_GetNextVideoFrame(INT64 handle, char* buffer, INT32 bufferLen);