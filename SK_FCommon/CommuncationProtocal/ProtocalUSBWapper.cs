using System;
using System.IO.Ports;

namespace SK_FCommon
{
    internal class ProtocalUSBWapper : ProtocalWapper, IDisposable
    {
        //需要更改为USB对应的事件
        ////<summary>
        ////完整协议的记录处理事件
        ////</summary>
        public override event DataReceivedEventHandler DataReceived;
        public override event SerialErrorReceivedEventHandler Error;

        public override void Connect()
        {
            throw new NotImplementedException();
        }
        public override void DisConnect()
        {
            throw new NotImplementedException();
        }
        public override int WriteData(byte[] SendData, ref  byte[] ReceiveData, int Overtime)
        {
            throw new NotImplementedException();
        }
        public override object WriteData(byte[] SendData, byte[] ReceiveData, int Overtime)
        {
            throw new NotImplementedException();
        }
        public override object WriteData(byte[] SendData, byte[] ReceiveData, int Overtime, int Delay)
        {
            throw new NotImplementedException();
        }

        public override void DiscardBuffer()
        {
            throw new NotImplementedException();
        }
        public override bool IsConnect()
        {
            throw new NotImplementedException();
        }

        public override void Connect_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }
        public override void Connect_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}