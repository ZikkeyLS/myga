using MygaCross;
using System;
using System.Net;

namespace MygaServer
{
    [Serializable]
    public class Client
    {
        public static readonly int dataBufferSize = 4096;
        public int id;

        public Client(int _clientId)
        {
            id = _clientId;
        }

        public void HandleData(byte[] _data)
        {
            ServerEventSystem.PackageRecieved(_data);
        }

    }
}
