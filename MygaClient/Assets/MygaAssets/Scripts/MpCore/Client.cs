using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using MygaCross;
using System.Text;

namespace MygaClient 
{
    public class State
    {
        public byte[] buffer = new byte[Client.bufferSize];
    }

    public static class Client
    {
        public static readonly int bufferSize = 8 * 1024;
        private static Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        private static State state = new State();

        public static string ip { get; private set; } = "127.0.0.1";
        public static int port { get; private set; } = 7777;
        public static int myId { get; private set; } = 0;
        public static bool connected { get; private set; } = false;

        public static void Connect(string _ip, int _port)
        {
            ip = _ip;
            port = _port;

            _socket.Connect(IPAddress.Parse(ip), port);
            EndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
            _socket.BeginReceiveFrom(state.buffer, 0, bufferSize, SocketFlags.None, ref endPoint, RecieveCallback, state);

            connected = true;
            Handler.ConnectEvents();
        }

        public static void Send(Package _package)
        {
            try
            {
                if (!connected)
                    return;

                _socket.BeginSend(_package.ToBytes(), 0, _package.ToBytes().Length, SocketFlags.None, (ar) =>
                {
                    _socket.EndSend(ar);
                }, state);
            }
            catch
            {
                Disconnect();
            }

        }

        private static void RecieveCallback(IAsyncResult result)
        {
            if (!connected)
                return;

            try
            {
                EndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
                State so = (State)result.AsyncState;
                _socket.EndReceiveFrom(result, ref endPoint);
                _socket.BeginReceiveFrom(so.buffer, 0, bufferSize, SocketFlags.None, ref endPoint, RecieveCallback, so);
                ClientEventSystem.PackageRecieved(so.buffer);
            }
            catch
            {
                Disconnect();
            }
        }

        public static void Disconnect()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            state = new State();

            Debug.Log($"Disconnected from server: {ip}:{port}");

            ip = "127.0.0.1";
            port = 7777;
            myId = 0;
            connected = false;
        }
    }
}