using MygaCross;
using System;

namespace MygaServer
{
    public static class Handler
    {
        public static void ConnectEvents()
        {

            ServerEventSystem.On(ServerEvent.ServerStarted, (eventID) => {
                Console.WriteLine($"Server started on: {Server.Ip}:{Server.Port} with maximum amount of players: {Server.MaxPlayers}!");
            });

            ServerEventSystem.On(ServerEvent.ClientConnected, (eventID) => {
                Server.CurrentPlayers++;
                Console.WriteLine("Player connected: " + Server.CurrentPlayers);

                IntroducePackage package = new IntroducePackage("Hello my dear friend!");
                Server.clients[Server.CurrentPlayers - 1].Send(package);
            });

            ServerEventSystem.On(ServerEvent.ClientDisconnected, (eventID) =>
            {
                Server.CurrentPlayers--;
                Console.WriteLine("Player disconnected: " + Server.CurrentPlayers);
            });

            ServerEventSystem.OnPackageRecieved(new PackageRecieved(OnPlayerLoginPackage), "PlayerLoginData");
            ServerEventSystem.OnPackageRecieved(new PackageRecieved(OnIntroducePackage), "IntroducePackage");
        }

        public static void OnPlayerLoginPackage(byte[] _data)
        {
            using (PlayerLoginData loginData = new PlayerLoginData(_data))
                Console.WriteLine(loginData.ToString());
        }

        public static void OnIntroducePackage(byte[] _data)
        {
            using (IntroducePackage introduce = new IntroducePackage(_data))
                if (introduce.message != "")
                    Console.WriteLine($"Data from client: {introduce.message}");
        }
    }
}
