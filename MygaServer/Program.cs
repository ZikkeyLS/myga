using System;

namespace MygaServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Server.Start("127.0.0.1", 25565, 1);

            while (true) 
            {
                ThreadManager.UpdateMain();
                if(Console.ReadLine().ToLower() == "stop")
                {
                    Socket.Stop();
                    Environment.Exit(0);
                }
            } 
        }
    }
}
