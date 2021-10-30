namespace MygaCross
{
    public class ErrorPackage : Package
    {
        public string message = string.Empty;
        public bool disconnect = false;

        public ErrorPackage(string _message, bool _disconnect = false) : base("ErrorPackage")
        {
            message = _message;
            disconnect = _disconnect;
            Write(_message);
            Write(_disconnect);
        }

        public ErrorPackage(byte[] _data) : base(_data)
        {
            message = reader.ReadString();
            disconnect = reader.ReadBool();
        }

        public override string ToString()
        {
            return message;
        }
    }
}
