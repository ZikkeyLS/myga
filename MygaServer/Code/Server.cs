using MygaCross;
using System;
using System.Collections.Generic;

namespace MygaServer
{

    public static class Server
    {
        public static int MaxPlayers { get; private set; } = 1;
        public static int CurrentPlayers = 0;
        public static string Ip { get; private set; } = "";
        public static int Port { get; private set; } = 0;

        public readonly static List<Client> clients = new List<Client>();

        public static void Start(string ip, int port, int maxPlayers)
        {
            Ip = ip;
            Port = port;
            MaxPlayers = maxPlayers;
          
            ServerSocket.Run(ip, port);
        }
    }
}
