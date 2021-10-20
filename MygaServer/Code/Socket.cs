using MygaCross;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace MygaServer
{
    public static class Socket
    {
        private static TcpListener tcpServer;
        private static UdpClient udpServer;

        public static void Run(string ip, int port)
        {

            tcpServer = new TcpListener(IPAddress.Parse(ip), port);
            tcpServer.Start();

            udpServer = new UdpClient(port);
            udpServer.BeginReceive(UDPReceiveCallback, null);

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
            tcpServer.BeginAcceptTcpClient(TCPConnectCallback, null);
        }

        private static void TCPConnectCallback(IAsyncResult _result)
        {
            TcpClient _client = tcpServer.EndAcceptTcpClient(_result);
            tcpServer.BeginAcceptTcpClient(TCPConnectCallback, null);

            Client client = new Client(Server.clients.Count);
            Server.clients.Add(client);
            client.tcp.Connect(_client);

            Console.WriteLine($"Incoming connection from {_client.Client.RemoteEndPoint}...");

            if (Server.clients.Count > Server.MaxPlayers)
            {
                SpamSend(client);
                Console.WriteLine($"{_client.Client.RemoteEndPoint} failed to connect: Server full!");
            }
            else
            {
               ServerEventSystem.StartEvent(ServerEvent.ClientConnected);
            }
        }


        private static void UDPReceiveCallback(IAsyncResult _result)
        {
            try
            {
                IPEndPoint _clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] _data = udpServer.EndReceive(_result, ref _clientEndPoint);
                udpServer.BeginReceive(UDPReceiveCallback, null);

                if (_data.Length < 4)
                {
                    return;
                }

                using (Package _package = new Package(_data))
                {
                    int _clientId = _package.reader.ReadInt();

                    if (_clientId == 0)
                    {
                        return;
                    }

                    if (Server.clients[_clientId].udp.endPoint == null)
                    {
                        Server.clients[_clientId].udp.Connect(_clientEndPoint);
                        return;
                    }

                    if (Server.clients[_clientId].udp.endPoint.ToString() == _clientEndPoint.ToString())
                    {
                        Server.clients[_clientId].HandleData(_data);
                    }
                }
            }
            catch (Exception _ex)
            {
                Console.WriteLine($"Error receiving UDP data: {_ex}");
            }
        }

        private static void SpamSend(Client client)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            for(int i = 0; i < 10000; i++)
            {
                ErrorPackage package = new ErrorPackage("Server is already full, try reconnect later!");
                client.tcp.SendData(package);

                package.Dispose();
            }
            Console.WriteLine("Ready in: " + (stopwatch.ElapsedMilliseconds/1000));
        }

        public static void SendUDPData(IPEndPoint _clientEndPoint, Package _package)
        {
            try
            {
                if (_clientEndPoint != null)
                {
                    udpServer.BeginSend(_package.ToBytes(), _package.ToBytes().Length, _clientEndPoint, null, null);
                }
            }
            catch (Exception _ex)
            {
                Console.WriteLine($"Error sending data to {_clientEndPoint} via UDP: {_ex}");
            }
        }
    }
}
