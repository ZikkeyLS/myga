using MygaCross;
using UnityEngine;

namespace MygaClient
{
    public static class Network
    {
        public static MygaThreading mygaConnection;

        public static void Connect(string ip, int port)
        {
            Client.Connect(ip, port);
            AddNetworkObject();
        }

        private static void AddNetworkObject()
        {
            GameObject connection = GameObject.Instantiate(new GameObject("MygaConnection"));
            GameObject.DontDestroyOnLoad(connection);
            mygaConnection = connection.AddComponent<MygaThreading>();
        }
    }
}
