using System.Collections.Generic;
using System.Net;

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

        public static bool ClientExist(EndPoint _endPoint)
        {
            foreach (Client client in clients)
            {
                if (client.endPoint.ToString() == _endPoint.ToString())
                    return true;
            }

            return false;
        }

        public static void TryAddClient(EndPoint _clientEndPoint)
        {
            if (ClientExist(_clientEndPoint))
                return;

            Client client = new Client(clients.Count, _clientEndPoint);
            clients.Add(client);
            ServerEventSystem.StartEvent(ServerEvent.ClientConnected);
        }
    }
}
