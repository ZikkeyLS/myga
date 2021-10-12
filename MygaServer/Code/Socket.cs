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
            Client client = new Client((TcpClient)result);
            Server.clients.Add(client);
            ServerEventSystem.StartEvent(ServerEvent.ClientConnected);

            tcpServer.EndAcceptTcpClient(result);
            AcceptTcpClient();
        }
    }
}
