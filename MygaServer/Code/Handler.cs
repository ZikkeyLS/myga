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
                Console.WriteLine("Player connected: " + Server.CurrentPlayers);
                Server.CurrentPlayers++;

                IntroducePackage package = new IntroducePackage("Hello my dear friend!");
                Server.clients[Server.CurrentPlayers - 1].Send(package);
            });

            ServerEventSystem.On(ServerEvent.ClientDisconnected, (eventID) =>
            {
                Server.CurrentPlayers--;
                Console.WriteLine("Player disconnected: " + Server.CurrentPlayers);
            });

            ServerEventSystem.OnPackageRecieved(new PackageRecieved((data) => {
                CheckerPackage package = new CheckerPackage(data);
                switch (package.packageType)
                {
                    case "PlayerLoginData":
                        PlayerLoginData loginData = new PlayerLoginData(data);
                        Console.WriteLine(loginData.ToString());
                        loginData.Dispose();
                        break;
                    case "IntroducePackage":
                        IntroducePackage introduce = new IntroducePackage(data);
                        Console.WriteLine($"Data from client: {introduce.message}");
                        introduce.Dispose();
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
