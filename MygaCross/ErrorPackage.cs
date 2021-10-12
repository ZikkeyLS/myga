namespace MygaCross
{
    public class ErrorPackage : Package
    {
        public string message;

        public ErrorPackage(string _message) : base("ErrorPackage")
        {
            message = _message;
            Write(_message);
        }

        public ErrorPackage(byte[] _data) : base(_data)
        {
            message = reader.ReadString();
        }

        public override string ToString()
        {
            return message;
        }
    }
}
