using System;
using System.Collections.Generic;

namespace MygaServer
{
    public static class Server
    {
        public static int MaxPlayers { get; private set; } = 1;
        public static int CurrentPlayers { get; private set; } = 0;
        public static bool stop = false;
        private static Socket socket = new Socket();

        public static Dictionary<string, Action> serverEvents = new Dictionary<string, Action>()
        {
            { "ClientConnected", () => { } },
            { "ClientDisconnected", () => { } },
            { "DataHandled",() => { } }
        };

        public static void On(string eventName, Action action)
        {
            serverEvents[eventName] = action;
        }

        public static void StartEvent(string eventName)
        {
            serverEvents[eventName]();
        }

        public static void Start(string ip, int port, int maxPlayers)
        {
            MaxPlayers = maxPlayers;
            ConnectEvents();
            socket.Run(ip, port);
        }

        private static void ConnectEvents()
        {
            On("ClientConnected", () => {
                CurrentPlayers++;
                Console.WriteLine("Player connected: " + CurrentPlayers);
            });
        }
    }
}
