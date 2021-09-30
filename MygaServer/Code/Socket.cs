using System;
using System.Net;
using System.Net.Sockets;

namespace MygaServer
{
    public class Socket
    {
        private TcpListener tcpServer;

        public Socket() { }
        public Socket(string ip, int port, bool start = true) { Run(ip, port, start); }

        public void Run(string ip, int port, bool start = true)
        {
            if (!start)
                return;

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
            finally
            {
                tcpServer.Stop();
            }
        }

        public void AcceptTcpClient()
        {
            TcpClient client = tcpServer.AcceptTcpClient();
            OnClientConnection(client);
            AcceptTcpClient();
        }

        private void OnClientConnection(TcpClient tcpClient)
        {
            ServerEventSystem.StartEvent(ServerEvent.ClientConnected);
            Client client = new Client(tcpClient);
            Server.clients.Add(client);
        }
    }
}
