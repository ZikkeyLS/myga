namespace MygaClient
{
    public class PlayerLoginData : Package
    {
        public string username;
        public string password;

        public PlayerLoginData(int _id, string _username, string _password) : base(_id, "PlayerLoginData")
        {
            username = _username;
            password = _password;
            writer.Write(username);
            writer.Write(password);
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
