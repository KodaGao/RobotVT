namespace SK_FModel
{
    public class SerialPortDelegate
    {
        //public delegate void DataReceivedEventHandler(object DataItem);

        public delegate void DataReceivedEventHandler(string PortName, byte[] Order);

        public delegate void RunExceptionEventHandler(object ErrorInfo);

        public delegate void SenderOrderEventHandler(string PortName, byte[] Order);

        public delegate void ReceiveOrderEventHandler(string PortName, byte[] Order);

        public delegate void UpdateRealTimeDataEventHandler(object DataItem);
    }
}