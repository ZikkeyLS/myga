using MygaCross;
using System;
using System.Net.Sockets;

namespace MygaServer
{
    public class Client
    {
        public TcpClient tcpClient;
        public NetworkStream tcpStream;
        public byte[] data = new byte[4096];

        public Client() { }

        public Client(TcpClient tcpClient)
        {
            Run(tcpClient);
        }

        public void Run(TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;
            tcpStream = tcpClient.GetStream();
            tcpStream.BeginRead(data, 0, data.Length, RecieveCallback, null);
        }

        private void RecieveCallback(IAsyncResult _result)
        {
            try
            {
                int _byteLength = tcpStream.EndRead(_result);
                if (_byteLength <= 0)
                {
                    Disconnect();
                    return;
                }

                ServerEventSystem.PackageRecieved(this, data);
                tcpStream.BeginRead(data, 0, data.Length, RecieveCallback, null);
            }
            catch (Exception _ex)
            {
                Console.WriteLine($"Error receiving TCP data: {_ex}");
                Disconnect();
            }
        }

        public void SendData(Package package)
        {
            byte[] bytes = package.ToBytes();
            tcpStream.Write(bytes, 0, bytes.Length);
        }

        public void Disconnect()
        {
            tcpClient.Close();
            tcpStream.Close();
            Server.clients.Remove(this);
        }
    }
}
