using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using MygaCross;

namespace MygaClient 
{
    public static class Client
    {
        public static int dataBufferSize = 4096;

        public static string ip = "26.202.163.171";
        public static int port = 25565;
        public static int myId = 0;
        public static TCP tcp = new TCP();
        public static UDP udp = new UDP();

        private static bool connected = false;

        public static void Connect(string _ip, int _port)
        {
            ip = _ip;
            port = _port;

            connected = true;
            tcp.Connect();
        }

        public static void SendTCPData(Package _package)
        {
            tcp.SendData(_package);
        }

        public static void SendUDPData(Package _package)
        {
            udp.SendData(_package);
        }

        public class TCP
        {
            public TcpClient socket;

            private NetworkStream stream;
            private Package receivedData;
            private byte[] receiveBuffer;

            public void Connect()
            {
                socket = new TcpClient
                {
                    ReceiveBufferSize = dataBufferSize,
                    SendBufferSize = dataBufferSize
                };

                receiveBuffer = new byte[dataBufferSize];
                socket.BeginConnect(Client.ip, Client.port, ConnectCallback, socket);
            }

            private void ConnectCallback(IAsyncResult _result)
            {
                socket.EndConnect(_result);

                if (!socket.Connected)
                {
                    return;
                }

                stream = socket.GetStream();

                receivedData = new Package();

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

                    byte[] _data = new byte[_byteLength];
                    Array.Copy(receiveBuffer, _data, _byteLength);
                    ClientEventSystem.PackageRecieved(_data);
                   
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
                receivedData = null;
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
                endPoint = new IPEndPoint(IPAddress.Parse(Client.ip), Client.port);
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

            public void SendData(Package _package)
            {
                try
                {
                    _package.Write(myId);
                    if (socket != null)
                    {
                        socket.BeginSend(_package.ToBytes(), _package.ToBytes().Length, null, null);
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

                    ClientEventSystem.PackageRecieved(_data);
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

        public static void Disconnect()
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