using MygaCross;
using System;
using System.Net.Sockets;
using UnityEngine;

namespace MygaClient
{
    public static class Socket
    {
        public static void Connect(string _ip, int _port)
        {
            Handler.ConnectEvents();
            TcpSocket.Connect(_ip, _port);
        }

        public static void SendTCPData(Package package)
        {
            TcpSocket.SendData(package);
        }

        public static void SendTCPData(byte[] data)
        {
            TcpSocket.stream.Write(data, 0, data.Length);
        }

        private static class TcpSocket
        {
            public static TcpClient client;
            public static NetworkStream stream;
            public static byte[] data = new byte[4096];

            public static void Connect(string _ip, int _port)
            {
                client = new TcpClient(_ip, _port);
                stream = client.GetStream();

                try
                {
                    stream.BeginRead(data, 0, data.Length, RecieveCallback, null);
                }
                catch (Exception e)
                {
                    Debug.LogWarning(e.Message);
                    Disconnect();
                }
            }

            private static void RecieveCallback(IAsyncResult _result)
            {
                int _byteLength = stream.EndRead(_result);
                if (_byteLength <= 0)
                {
                    Disconnect();
                    return;
                }

                ClientEventSystem.PackageRecieved(data);
                stream.BeginRead(data, 0, data.Length, RecieveCallback, null);
            }

            public static void Disconnect()
            {
                client.Close();
                client.Dispose();
                stream.Close();
                stream.Dispose();
            }

            public static void SendData(Package package)
            {
                byte[] data = package.ToBytes();
                stream.Write(data, 0, data.Length);
            }
        }

        private static class UdpSocket
        {

        }
    }
}
