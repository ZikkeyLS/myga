namespace MygaCross
{
    public class PlayerLoginData : Package
    {
        public string username;
        public string password;

        public PlayerLoginData(string _username, string _password) : base("PlayerLoginData")
        {
            username = _username;
            password = _password;
            Write(username);
            Write(password);
        }

        public PlayerLoginData(byte[] _data) : base(_data)
        {
            username = reader.ReadString();
            password = reader.ReadString();
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", this.username, this.password);
        }
    }
}