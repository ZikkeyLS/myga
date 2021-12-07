using System;

namespace MygaServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Server.Start("127.0.0.1", 7777, 1);

            while (true) 
            {
                if(Console.ReadLine().ToLower() == "stop")
                {
                    ServerSocket.Close();
                    Environment.Exit(0);
                }
            } 
        }
    }
}
