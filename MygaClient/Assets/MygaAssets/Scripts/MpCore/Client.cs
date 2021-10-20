using MygaCross;
using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace MygaClient
{
    public static class Client
    {
        public static int dataBufferSize = 4096;

        public static string ip { get; private set; } = "26.202.163.171";
        public static int port { get; private set; } = 25565;
        public static TCP tcp { get; private set; } = new TCP();
        public static UDP udp { get; private set; } = new UDP();
        private static bool connected = false;

        public static void Connect(string _ip, int _port)
        {
            ip = _ip;
            port = _port;

            connected = true;
            tcp.Connect();
        }

        private static void HandleData(byte[] _data)
        {
            byte[] copiedData = new byte[4096];
            Array.Copy(_data, copiedData, _data.Length);
            ClientEventSystem.PackageRecieved(copiedData);
        }

        public static void SendTCPData(Package package)
        {
            tcp.SendData(package);
        }

        public class TCP
        {
            public TcpClient socket;
            private NetworkStream stream;
            private byte[] receiveBuffer;

            public void Connect()
            {
                socket = new TcpClient
                {
                    ReceiveBufferSize = dataBufferSize,
                    SendBufferSize = dataBufferSize
                };

                receiveBuffer = new byte[dataBufferSize];
                socket.BeginConnect(ip, port, ConnectCallback, socket);
            }

            private void ConnectCallback(IAsyncResult _result)
            {
                socket.EndConnect(_result);

                if (!socket.Connected)
                {
                    return;
                }

                stream = socket.GetStream();
                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
            }

            public void SendData(Package _package)
            {
                try
                {
                    if (socket != null)
                    {
                        stream.BeginWrite(_package.ToBytes(), 0, _package.ToBytes().Length, null, null);
                    }
                }
                catch (Exception _ex)
                {
                    Debug.Log($"Error sending data to server via TCP: {_ex}");
                }
            }

            private void ReceiveCallback(IAsyncResult _result)
            {
                try
                {
                    int _byteLength = stream.EndRead(_result);
                    if (_byteLength <= 0)
                    {
                        Client.Disconnect();
                        return;
                    }

                    HandleData(receiveBuffer);
                    stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
                }
                catch
                {
                    Disconnect();
                }
            }

            private void Disconnect()
            {
                Client.Disconnect();

                stream = null;
                receiveBuffer = null;
                socket = null;
            }
        }

        public class UDP
        {
            public UdpClient socket;
            public static IPEndPoint endPoint;

            public UDP()
            {
                endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            }

            public void Connect(int _localPort)
            {
                socket = new UdpClient(_localPort);

                socket.Connect(endPoint);
                socket.BeginReceive(ReceiveCallback, null);

                using (Package _package = new Package())
                {
                    SendData(_package);
                }
            }

            public void SendData(Package _packet)
            {
                try
                {
                    if (socket != null)
                    {
                        socket.BeginSend(_packet.ToBytes(), _packet.ToBytes().Length, null, null);
                    }
                }
                catch (Exception _ex)
                {
                    Debug.Log($"Error sending data to server via UDP: {_ex}");
                }
            }

            private void ReceiveCallback(IAsyncResult _result)
            {
                try
                {
                    byte[] _data = socket.EndReceive(_result, ref endPoint);
                    socket.BeginReceive(ReceiveCallback, null);

                    if (_data.Length < 4)
                    {
                        Client.Disconnect();
                        return;
                    }

                    HandleData(_data);
                }
                catch
                {
                    Disconnect();
                }
            }

            private void Disconnect()
            {
                Client.Disconnect();

                endPoint = null;
                socket = null;
            }
        }

        private static void Disconnect()
        {
            if (connected)
            {
                connected = false;
                tcp.socket.Close();
                udp.socket.Close();

                Debug.Log("Disconnected from server...");
            }
        }
    }

}
