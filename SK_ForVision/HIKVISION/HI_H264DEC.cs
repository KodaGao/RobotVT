using System;
using System.Runtime.InteropServices;

namespace SK_FVision
{
    public class HI_H264DEC
    {
        public const int HI_SUCCESS = 0;
        public const int HI_FAILURE = -1;
        public const int HI_LITTLE_ENDIAN = 1234;
        public const int HI_BIG_ENDIAN = 4321;
        public const int HI_DECODER_SLEEP_TIME = 60000;

        public const int HI_H264DEC_OK = 0;
        public const int HI_H264DEC_NEED_MORE_BITS = -1;
        public const int HI_H264DEC_NO_PICTURE = -2;
        public const int HI_H264DEC_ERR_HANDLE = -3;

        [DllImport("hi_h264dec_w.dll", EntryPoint = "Hi264DecImageEnhance", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Hi264DecImageEnhance(IntPtr hDec, ref hiH264_DEC_FRAME_S pDecFrame, uint uEnhanceCoeff);

        [DllImport("hi_h264dec_w.dll", EntryPoint = "Hi264DecCreate", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr Hi264DecCreate(ref hiH264_DEC_ATTR_S pDecAttr);

        [DllImport("hi_h264dec_w.dll", EntryPoint = "Hi264DecDestroy", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Hi264DecDestroy(IntPtr hDec);

        [DllImport("hi_h264dec_w.dll", EntryPoint = "Hi264DecGetInfo", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Hi264DecGetInfo(ref hiH264_LIBINFO_S pLibInfo);

        [DllImport("hi_h264dec_w.dll", EntryPoint = "Hi264DecFrame", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Hi264DecFrame(IntPtr hDec, IntPtr pStream, uint iStreamLen, ulong ullPTS, ref hiH264_DEC_FRAME_S pDecFrame, uint uFlags);

        [DllImport("hi_h264dec_w.dll", EntryPoint = "Hi264DecAU", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Hi264DecAU(IntPtr hDec, IntPtr pStream, uint iStreamLen, ulong ullPTS, ref hiH264_DEC_FRAME_S pDecFrame, uint uFlags);

        [StructLayout(LayoutKind.Sequential)]
        public struct hiH264_DEC_ATTR_S
        {
            public uint uPictureFormat;
            public uint uStreamInType;
            public uint uPicWidthInMB;
            public uint uPicHeightInMB;
            public uint uBufNum;
            public uint uWorkMode;
            public IntPtr pUserData;
            public uint uReserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct hiH264_DEC_FRAME_S
        {
            public IntPtr pY;
            public IntPtr pU;
            public IntPtr pV;
            public uint uWidth;
            public uint uHeight;
            public uint uYStride;
            public uint uUVStride;
            public uint uCroppingLeftOffset;
            public uint uCroppingRightOffset;
            public uint uCroppingTopOffset;
            public uint uCroppingBottomOffset;
            public uint uDpbIdx;
            public uint uPicFlag;
            public uint bError;
            public uint bIntra;
            public ulong ullPTS;
            public uint uPictureID;
            public uint uReserved;
            public IntPtr pUserData;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct hiH264_LIBINFO_S
        {
            public uint uMajor;
            public uint uMinor;
            public uint uRelease;
            public uint uBuild;
            [MarshalAs(UnmanagedType.LPStr)] public string sVersion;
            [MarshalAs(UnmanagedType.LPStr)] public string sCopyRight;
            public uint uFunctionSet;
            public uint uPictureFormat;
            public uint uStreamInType;
            public uint uPicWidth;
            public uint uPicHeight;
            public uint uBufNum;
            public uint uReserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct hiH264_USERDATA_S
        {
            public uint uUserDataType;
            public uint uUserDataSize;
            public IntPtr pData;
            public IntPtr pNext;
        }
    }
}
