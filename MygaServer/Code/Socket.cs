using System;
using System.Net;
using System.Net.Sockets;

namespace MygaServer
{
    public class Socket
    {
        private TcpListener tcpServer;
        private UdpClient udpServer;
        private static TcpClient client;

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
                while (true)
                {
                    DoBeginAcceptTcpClient(tcpServer);
                }
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

        public void DoBeginAcceptTcpClient(TcpListener listener)
        {

            listener.BeginAcceptTcpClient(
                new AsyncCallback(DoAcceptTcpClientCallback),
                listener);
        }

        public void DoAcceptTcpClientCallback(IAsyncResult ar)
        {
            TcpListener listener = (TcpListener)ar.AsyncState;

            TcpClient client = listener.EndAcceptTcpClient(ar);
            OnClientConnection(client);
        }

        private void OnClientConnection(TcpClient tcpClient)
        {
            Client client = new Client(tcpClient);
            Server.clients.Add(client);
            Package package = new Package(0);
            package.writer.Write("Hello from server!");
            ServerEventSystem.StartEvent(ServerEvent.ClientConnected);
        }
    }
}
