namespace MygaCross
{
    public class ConnectPackage : Package
    {
        public ConnectStatus status;

        public ConnectPackage(ConnectStatus _connectStatus) : base("ConnectPackage")
        {
            status = _connectStatus;
            Write((int)_connectStatus);
        }

        public ConnectPackage(byte[] _data) : base(_data)
        {
            status = (ConnectStatus)reader.ReadInt();
        }
    }
}
