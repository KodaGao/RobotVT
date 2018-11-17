namespace SK_FModel
{
    public class SerialPortDelegate
    {
        public delegate void del_DataReceived(object DataItem);

        public delegate void del_RunException(object ErrorInfo);

        public delegate void del_SenderOrder(string PortName, byte[] Order);

        public delegate void del_ReceiveOrder(string PortName, byte[] Order);
    }
}