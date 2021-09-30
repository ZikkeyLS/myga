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

            Package package1 = new Package(0);
            package1.writer.Write("hi client");
            Send(package1);

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
                package.reader.ReadInt32();

                Console.WriteLine(package.reader.ReadString());

                tcpStream.BeginRead(data, 0, data.Length, RecieveCallback, null);
            }
            catch (Exception _ex)
            {
                Console.WriteLine($"Error receiving TCP data: {_ex}");
                tcpClient.Close();
                tcpStream.Close();
            }
        }


        public void Send(Package package)
        {
            byte[] data = package.buffer;
            tcpStream.Write(data, 0, data.Length);
        }
    }
}
