using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MygaServer
{
    public class Socket
    {
        private TcpListener tcpServer;
        private UdpClient udpServer;
        private static TcpClient client;

        public void Run(string ip, int port, bool start = true)
        {
            if (!start)
                return;

            tcpServer = new TcpListener(IPAddress.Parse(ip), port);
            tcpServer.Start();

            try
            {
                while (true)
                {
                    DoBeginAcceptTcpClient(tcpServer);
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                tcpServer.Stop();
            }
        }

        public  void DoBeginAcceptTcpClient(TcpListener listener)
        {

            listener.BeginAcceptTcpClient(
                new AsyncCallback(DoAcceptTcpClientCallback),
                listener);
        }

        public void DoAcceptTcpClientCallback(IAsyncResult ar)
        {
            TcpListener listener = (TcpListener)ar.AsyncState;

            TcpClient client = listener.EndAcceptTcpClient(ar);
            Server.StartEvent("ClientConnected");
            OnClientConnection(client);
        }

        private void OnClientConnection(TcpClient client)
        {

            Byte[] bytes = new Byte[256];
            String data = null;

            NetworkStream stream = client.GetStream();

            int i;

            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
            {

                data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                Console.WriteLine("Received: {0}", data);

                data = data.ToUpper();

                byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                stream.Write(msg, 0, msg.Length);
                Console.WriteLine("Sent: {0}", data);
            }

            client.Close();
        }
    }
}
