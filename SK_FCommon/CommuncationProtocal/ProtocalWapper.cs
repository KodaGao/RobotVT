using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using SK_FModel;

namespace SK_FCommon
{
    public abstract class ProtocalWapper : IDisposable
    {
        private static ProtocalWapper _Instance;
        public static ProtocalWapper CreateInstance(Protocol protocol)
        {
            if (_Instance == null)
            {
                switch (protocol)
                {
                    //case Protocol.TCPIP:
                    //    _Instance = new ProtocalTCPIPWapper();
                    //    break;
                    case Protocol.SerialPort:
                        _Instance = new ProtocalSerialPortWapper();
                        break;
                    case Protocol.IPPR:
                        _Instance = new ProtocalIPPRWapper();
                        break;
                    //case Protocol.USB:
                    //    _Instance = new ProtocalUSBWapper();
                    //    break;
                    case Protocol.ADC:
                        _Instance = new ProtocalSerialPortWapper();
                        break;
                    default:
                        break;
                }
            }
            return _Instance;
        }

        public Protocol Protocol;
        public object ConnectObject;
        ////<summary>
        ////完整协议的记录处理事件
        ////</summary>
        public abstract event DataReceivedEventHandler DataReceived;
        public abstract event SerialErrorReceivedEventHandler Error;

        public abstract bool IsConnect();
        public abstract void Connect();
        public abstract void DisConnect();
        public abstract int WriteData(byte[] SendData, ref  byte[] ReceiveData, int Overtime);
        public abstract object WriteData(byte[] SendData, byte[] ReceiveData, int Overtime);
        public abstract object WriteData(byte[] SendData, byte[] ReceiveData, int Overtime,int Delay);
        public abstract void DiscardBuffer();


        public abstract void Connect_DataReceived(object sender, SerialDataReceivedEventArgs e);
        public abstract void Connect_ErrorReceived(object sender, SerialErrorReceivedEventArgs e);

        #region Transaction Identifier
        /// <summary>
        /// 数据序号标识
        /// </summary>
        private byte dataIndex = 0;

        protected byte CurrentDataIndex
        {
            get { return this.dataIndex; }
        }

        protected byte NextDataIndex()
        {
            return ++this.dataIndex;
        }
        #endregion

        #region IDisposable 成员
        public virtual void Dispose()
        {
        }
        #endregion
    }

    public delegate void DataReceivedEventHandler(DataReceivedEventArgs e);
    public class DataReceivedEventArgs : EventArgs
    {
        public string DataReceived;
        public string DataSend;
        public string DataReceivedTime;
        public string DataSendTime;
        public DataReceivedEventArgs(string m_DataSend, string m_DataSendTime, string m_DataReceived, string m_DataReceivedTime)
        {
            this.DataSend = m_DataSend;
            this.DataSendTime = m_DataSendTime;
            this.DataReceived = m_DataReceived;
            this.DataReceivedTime = m_DataReceivedTime;
        }
    }

}
