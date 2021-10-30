namespace MygaCross
{
    public class IntroducePackage : Package
    {
        public string message;

        public IntroducePackage(string _message) : base("IntroducePackage")
        {
            message = _message;
            Write(_message);
        }

        public IntroducePackage(byte[] _data) : base(_data)
        {
            message = reader.ReadString();
        }

        public override string ToString()
        {
            return message;
        }
    }
}
