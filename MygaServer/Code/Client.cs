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

<<<<<<< HEAD
            Package package1 = new Package(0);
            package1.writer.Write("hi client");
            Send(package1);

            tcpStream.BeginRead(data, 0, data.Length, RecieveCallback, null);
=======
            Package package = new Package(55);
            SendData(package);

            tcpStream.BeginRead(data, 0, data.Length, ReceiveCallback, null);
>>>>>>> ff48c148227118e622a5312f9fdf583388a5da0c
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
<<<<<<< HEAD
                package.reader.ReadInt32();

                Console.WriteLine(package.reader.ReadString());

                tcpStream.BeginRead(data, 0, data.Length, RecieveCallback, null);
=======
                Console.WriteLine(package.reader.ReadInt32());

                tcpStream.BeginRead(data, 0, data.Length, ReceiveCallback, null);
>>>>>>> ff48c148227118e622a5312f9fdf583388a5da0c
            }
            catch (Exception _ex)
            {
                Console.WriteLine($"Error receiving TCP data: {_ex}");
                tcpClient.Close();
                tcpStream.Close();
            }
        }


<<<<<<< HEAD
        public void Send(Package package)
        {
            byte[] data = package.buffer;
            tcpStream.Write(data, 0, data.Length);
=======
        public void SendData(Package package)
        {
            byte[] bytes = package.buffer;
            tcpStream.Write(bytes, 0, bytes.Length);
>>>>>>> ff48c148227118e622a5312f9fdf583388a5da0c
        }
    }
}
