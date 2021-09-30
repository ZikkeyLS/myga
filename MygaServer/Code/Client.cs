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

            Package package = new Package(55);
            SendData(package);

            tcpStream.BeginRead(data, 0, data.Length, RecieveCallback, null);
        }

        private void RecieveCallback(IAsyncResult _result)
        {
            try
            {
                int _byteLength = tcpStream.EndRead(_result);
                if (_byteLength <= 0)
                {
                    tcpClient.Close();
                    tcpStream.Close();
                    return;
                }

                ServerEventSystem.StartEvent(ServerEvent.DataHandled);

                Package package = new Package(data);
                Console.WriteLine(package.id);

                tcpStream.BeginRead(data, 0, data.Length, RecieveCallback, null);
            }
            catch (Exception _ex)
            {
                Console.WriteLine($"Error receiving TCP data: {_ex}");
                tcpClient.Close();
                tcpStream.Close();
            }
        }
        public void SendData(Package package)
        {
            byte[] bytes = package.buffer;
            tcpStream.Write(bytes, 0, bytes.Length);
        }
    }
}
