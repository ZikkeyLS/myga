using System;
using System.Collections.Generic;

namespace MygaServer
{
    public static class Server
    {
        public static int MaxPlayers { get; private set; } = 1;
        public static int CurrentPlayers { get; private set; } = 0;
        public static string Ip { get; private set; } = "";
        public static int Port { get; private set; } = 0;

        public static HashSet<Client> clients = new HashSet<Client>();
        public static bool stop = false;
        private static Socket socket;

        public static void Start(string ip, int port, int maxPlayers)
        {
            Ip = ip;
            Port = port;
            MaxPlayers = maxPlayers;
            ConnectBasicEvents();
            socket = new Socket(ip, port);
        }

        private static void ConnectBasicEvents()
        {

            ServerEventSystem.On(ServerEvent.ServerStarted, (eventID) => {
                Console.WriteLine($"Server started on: {Ip}:{Port} with maximum amount of players: {MaxPlayers}!");
            });

            ServerEventSystem.On(ServerEvent.ClientConnected, (eventID) => {
                CurrentPlayers++;
                Console.WriteLine("Player connected: " + CurrentPlayers);
            });

            ServerEventSystem.OnPackageRecieved(new PackageRecieved((client, data) => {
                CheckerPackage package = new CheckerPackage(data);
                switch (package.packageType)
                {
                    case "PlayerLoginData":
                        PlayerLoginData loginData = new PlayerLoginData(data);
                        Console.WriteLine(loginData.ToString());
                        loginData.Dispose();
                        break;
                    default:
                        Console.WriteLine($"Default or unknown package with type: {package.packageType}");
                        break;
                }
                package.Dispose();
            }));
        }
    }
}
