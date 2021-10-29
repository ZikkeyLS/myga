using MygaCross;
using System.Net;

namespace MygaServer
{
    public class Client
    {
        public readonly int id;
        public readonly EndPoint endPoint;

        public Client(int _clientId, EndPoint _endPoint)
        {
            id = _clientId;
            endPoint = _endPoint;
        }

        public void Send(Package _package)
        {
            ServerSocket.Send(this, _package);
        }
    }
}
