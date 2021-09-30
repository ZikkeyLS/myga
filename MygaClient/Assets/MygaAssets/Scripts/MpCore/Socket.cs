using MygaClient;
using System;
using System.Net.Sockets;
using UnityEngine;

public static class Socket
{
    private static UdpClient udpClient;

    public static void Connect(string _ip, int _port)
    {
        TcpSocket.Connect(_ip, _port);
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
            catch (ArgumentNullException e)
            {
                Debug.Log($"ArgumentNullException: {e}");
            }
            catch (SocketException e)
            {
                Debug.Log($"SocketException: {e}");
                Disconnect();
            }
        }

        private static void RecieveCallback(IAsyncResult _result)
        {
<<<<<<< HEAD
            try
            {
                int _byteLength = stream.EndRead(_result);
                if (_byteLength <= 0)
                {
                    Disconnect();
                    return;
                }

                Package package = new Package(data);
                package.reader.ReadInt32();

                Debug.Log(package.reader.ReadString());

                Package package1 = new Package(0);
                package1.writer.Write("hi server");
                Send(package1);

                stream.BeginRead(data, 0, data.Length, RecieveCallback, null);
            }
            catch (Exception _ex)
            {
               Debug.Log($"Error receiving TCP data: {_ex}");
                Disconnect();
            }
=======

            int _byteLength = stream.EndRead(_result);
            if (_byteLength <= 0)
            {
                Disconnect();
                return;
            }

            Package package = new Package(data);
            Debug.Log(package.id);

            Package backPackage = new Package(5);
            SendData(backPackage);

            stream.BeginRead(data, 0, data.Length, RecieveCallback, null);
>>>>>>> ff48c148227118e622a5312f9fdf583388a5da0c
        }

        public static void Disconnect()
        {
            client.Close();
            client.Dispose();
            stream.Close();
            stream.Dispose();
        }

<<<<<<< HEAD
        public static void Send(Package package)
=======
        public static void SendData(Package package)
>>>>>>> ff48c148227118e622a5312f9fdf583388a5da0c
        {
            byte[] data = package.buffer;
            stream.Write(data, 0, data.Length);
        }
    }

    private class UdpSocket
    {

    }
}
