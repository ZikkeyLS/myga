using MygaCross;
using System;
using System.Net;
using System.Threading;

namespace MygaServer
{
    public class Client
    {
        private static readonly int maxHeartbeatDelay = 10 * 60 * 1000;
        private DateTime hearthBeat;

        public readonly int id;
        public EndPoint endPoint { get; private set; }

        public Client(int _clientId, EndPoint _endPoint)
        {
            id = _clientId;
            endPoint = _endPoint;
            hearthBeat = DateTime.Now;

            Timer timer = new Timer(new TimerCallback(DisconnectCheck), null, 0, maxHeartbeatDelay);
        }

        public void Send(Package _package)
        {
            ServerSocket.Send(this, _package);
        }

        public void UpdateHeartbeat()
        {
            hearthBeat = DateTime.Now;
        }

        private void DisconnectCheck(object obj)
        {
            float msDelay = (DateTime.Now.Ticks - hearthBeat.Ticks) / 10000;

            if (msDelay > maxHeartbeatDelay)
            {
                Server.clients.Remove(this);
                hearthBeat = DateTime.MinValue;
                endPoint = null;

                return;
            }

            Timer timer = new Timer(new TimerCallback(DisconnectCheck), null, 0, maxHeartbeatDelay);
        }
    }
}
