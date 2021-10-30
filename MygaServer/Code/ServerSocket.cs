using MygaCross;
using System;
using System.Net;
using System.Net.Sockets;

namespace MygaServer
{
    public class State
    {
        public byte[] buffer = new byte[ServerSocket.bufferSize];
    }

    public static class ServerSocket
    {
        public static readonly int bufferSize = 1024;
        private static readonly Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        private static readonly State state = new State();
        private static bool sending = false;

        public static void Run(string ip, int port)
        {
            Handler.ConnectEvents();

            _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
            _socket.Bind(new IPEndPoint(IPAddress.Parse(ip), port));

            EndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
            _socket.BeginReceiveFrom(state.buffer, 0, bufferSize, SocketFlags.None, ref clientEndPoint, RecieveCallback, state);

            ServerEventSystem.StartEvent(ServerEvent.ServerStarted);
        }

        public static void Close()
        {
            _socket.Close();
        }

        public static void Send(Client _client, Package _package)
        {
            if (sending)
                return;

            sending = true;
            _socket.BeginSendTo(_package.ToBytes(), 0, _package.ToBytes().Length, SocketFlags.None, _client.endPoint, (ar) =>
            {
                sending = false;
                _socket.EndSend(ar);
            }, state);
        }

        public static void Send(EndPoint _endPoint, Package _package)
        {
            _socket.BeginSendTo(_package.ToBytes(), 0, _package.ToBytes().Length, SocketFlags.None, _endPoint, (ar) =>
            {
                _socket.EndSend(ar);
            }, state);
        }

        public static void SendAll(Package _package, int _exceptID = -1)
        {
            foreach(Client client in Server.clients)
            {
                if(client.id != _exceptID)
                    Send(client, _package);
            }
        }

        public static void SendAll(Package _package, int[] _exceptIDs)
        {
            foreach (Client client in Server.clients)
            {
                foreach(int _exceptID in _exceptIDs)
                {
                    if (client.id != _exceptID)
                        Send(client, _package);
                }
            }
        }

        private static void RecieveCallback(IAsyncResult _result)
        {
            EndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
            State so = (State)_result.AsyncState;

            _socket.EndReceiveFrom(_result, ref clientEndPoint);
            _socket.BeginReceiveFrom(so.buffer, 0, bufferSize, SocketFlags.None, ref clientEndPoint, RecieveCallback, so);

            ConnectStatus connectStatus = Server.TryAddClient(clientEndPoint);
            switch (connectStatus) 
            {
                case ConnectStatus.connected:
                    ServerEventSystem.PackageRecieved(so.buffer);
                    break;
                case ConnectStatus.already:
                    ServerEventSystem.PackageRecieved(so.buffer);
                    break;
            }
            
            if(connectStatus == ConnectStatus.full || connectStatus == ConnectStatus.connected)
                using (ConnectPackage package = new ConnectPackage(connectStatus))
                    Send(clientEndPoint, package);
        }
    }
}
