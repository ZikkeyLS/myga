using MygaClient;
using System;
using System.Net.Sockets;
using UnityEngine;

public static class Socket
{
    private static TcpClient tcpClient;
    private static UdpClient udpClient;

    public static void Connect(string ip, int port)
    {
        try
        {
            string message = "Hi server";
            TcpClient client = new TcpClient(ip, port);

            NetworkStream stream = client.GetStream();

            byte[] data = new byte[4096];

            Package package = new Package(0);
            package.writer.Write(message);
            data = package.buffer;
            stream.Write(data, 0, data.Length);
            Debug.Log($"Sent: {message}");


            Int32 bytes = stream.Read(data, 0, data.Length);
            Package packageGet = new Package(data);
            packageGet.reader.ReadInt32();
            Debug.Log($"Recieved: {packageGet.reader.ReadString()}");

            stream.Close();
            client.Close();
        }
        catch (ArgumentNullException e)
        {
            Debug.Log($"ArgumentNullException: {e}");
        }
        catch (SocketException e)
        {
            Debug.Log($"SocketException: {e}");
        }
    }
}
