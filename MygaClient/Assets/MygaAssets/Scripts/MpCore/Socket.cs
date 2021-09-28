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
            string message = "Test";
            TcpClient client = new TcpClient(ip, port);

            Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

            NetworkStream stream = client.GetStream();

            stream.Write(data, 0, data.Length);

            Debug.Log($"Sent: {message}");

            data = new Byte[256];

            Int32 bytes = stream.Read(data, 0, data.Length);
            String responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            Debug.Log($"Recieved: {responseData}");

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
