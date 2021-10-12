namespace MygaClient
{
    public static class Network
    {
        public static void Connect(string ip, int port)
        {
            Socket.Connect(ip, port);
        }
    }
}
