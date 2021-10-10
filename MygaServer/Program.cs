namespace MygaServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Server.Start("127.0.0.1", 25565, 100);
        }
    }
}
