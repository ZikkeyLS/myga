namespace MygaCross
{
    public class ServerIntroducePackage : Package
    {
        public string message;

        public ServerIntroducePackage(string _message) : base("ServerIntroducePackage")
        {
            message = _message;
            Write(_message);
        }

        public ServerIntroducePackage(byte[] _data) : base(_data)
        {
            message = reader.ReadString();
        }

        public override string ToString()
        {
            return message;
        }
    }
}
