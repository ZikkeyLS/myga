using MygaCross;
using System;
using System.Net;
using System.Net.Sockets;

namespace MygaServer
{
    public static class Socket
    {
        private static TcpListener tcpServer;

        public static void Run(string ip, int port)
        {

            tcpServer = new TcpListener(IPAddress.Parse(ip), port);
            tcpServer.Start();
            ServerEventSystem.StartEvent(ServerEvent.ServerStarted);

            try
            {
                AcceptTcpClient();
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
        }

        public static void Stop()
        {
            tcpServer.Stop();
        }

        private static void AcceptTcpClient()
        {
            tcpServer.BeginAcceptTcpClient(OnClientConnection, null);
        }

        private static void OnClientConnection(IAsyncResult result)
        {
            Client client = new Client(tcpServer.EndAcceptTcpClient(result));
            Server.clients.Add(client);

            if (Server.clients.Count >= Server.MaxPlayers)
            {
                ErrorPackage package = new ErrorPackage("Server is already full, try reconnect later!");
                client.SendData(package);
                ServerIntroducePackage package2 = new ServerIntroducePackage("ect later!");
                client.SendData(package2);
                // client.Disconnect();
                package.Dispose();
            }
            else
                ServerEventSystem.StartEvent(ServerEvent.ClientConnected);

            AcceptTcpClient();
        }
    }
}
