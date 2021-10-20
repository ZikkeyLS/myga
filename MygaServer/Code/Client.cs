using MygaCross;
using System;
using System.Net;
using System.Net.Sockets;

namespace MygaServer
{
    [Serializable]
    public class Client
    {
        public static int dataBufferSize = 4096;

        public int id;
        public TCP tcp;
        public UDP udp;

        public Client(int _clientId)
        {
            id = _clientId;
            tcp = new TCP(id, this);
            udp = new UDP(id);
        }

        public bool HandleData(byte[] _data)
        {
            ThreadManager.ExecuteOnMainThread(() =>
            {
                ServerEventSystem.PackageRecieved(this, _data);
            });

            return true;
        }


        public class TCP
        {
            public Client client;
            public TcpClient socket;

            private readonly int id;
            private NetworkStream stream;
            private Package receivedData;
            private byte[] receiveBuffer;

            public TCP(int _id, Client client)
            {
                id = _id;
                this.client = client;
            }

            public void Connect(TcpClient _socket)
            {
                socket = _socket;
                socket.ReceiveBufferSize = dataBufferSize;
                socket.SendBufferSize = dataBufferSize;

                stream = socket.GetStream();

                receivedData = new Package();
                receiveBuffer = new byte[dataBufferSize];

                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);

                // ServerSend.Welcome(id, "Welcome to the server!");
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
                    Console.WriteLine($"Error sending data to player {id} via TCP: {_ex}");
                }
            }

            private void ReceiveCallback(IAsyncResult _result)
            {
                try
                {
                    int _byteLength = stream.EndRead(_result);
                    if (_byteLength <= 0)
                    {
                        client.Disconnect();
                        return;
                    }

                    byte[] _data = new byte[_byteLength];
                    Array.Copy(receiveBuffer, _data, _byteLength);
                    client.HandleData(_data);

                    stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
                }
                catch (Exception _ex)
                {
                    Console.WriteLine($"Error receiving TCP data: {_ex}");
                    Disconnect();
                }
            }

            public void Disconnect()
            {
                socket.Close();
                stream = null;
                receivedData = null;
                receiveBuffer = null;
                socket = null;
            }
        }

        public class UDP
        {
            public IPEndPoint endPoint;

            private int id;

            public UDP(int _id)
            {
                id = _id;
            }

            public void Connect(IPEndPoint _endPoint)
            {
                endPoint = _endPoint;
            }

            public void SendData(Package _package)
            {
                Socket.SendUDPData(endPoint, _package);
            }

            public void Disconnect()
            {
                endPoint = null;
            }
        }

        private void Disconnect()
        {
            Console.WriteLine(tcp.socket.Client.RemoteEndPoint + " has disconnected.");

            tcp.Disconnect();
            udp.Disconnect();
        }
    }
}
