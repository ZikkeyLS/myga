using MygaCross;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MygaServer
{
    public static class ServerSocket
    {
        private static Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        private static int bufSize = 8 * 1024;
        private static State state = new State();
        private static EndPoint epFrom = new IPEndPoint(IPAddress.Any, 0);
        private static AsyncCallback recv = null;

        public class State
        {
            public byte[] buffer = new byte[bufSize];
        }

        public static void Connect(string address, int port)
        {
            _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
            _socket.Bind(new IPEndPoint(IPAddress.Parse(address), port));
            Receive();
        }

        public static void Send(Package _package)
        {
            _socket.BeginSend(_package.ToBytes(), 0, _package.ToBytes().Length, SocketFlags.None, (ar) =>
            {
                _socket.EndSend(ar);
            }, state);
        }

        private static void Receive()
        {
            _socket.BeginReceiveFrom(state.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv = (ar) =>
            {
                State so = (State)ar.AsyncState;
                int bytes = _socket.EndReceiveFrom(ar, ref epFrom);
                _socket.BeginReceiveFrom(so.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv, so);
                ServerEventSystem.PackageRecieved(so.buffer);
            }, state);
        }

        public static void Run(string ip, int port)
        {
            Handler.ConnectEvents();
            Connect(ip, port);
            ServerEventSystem.StartEvent(ServerEvent.ServerStarted);
        }

        public static void Close()
        {
            _socket.Close();
        }
    }
}
