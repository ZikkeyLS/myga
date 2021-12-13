using MygaCross;
using System.Collections.Generic;
using UnityEngine;

namespace MygaClient
{
    public delegate void ClientEventFunction(int id);
    public delegate void PackageRecieved(byte[] data);

    public class ClientEventData
    {
        public ClientEventFunction clientEvent;
        public bool once = false;

        public ClientEventData(ClientEventFunction clientEvent, bool once = false)
        {
            this.clientEvent = clientEvent;
            this.once = once;
        }
    }


    public class PackageRecievedData
    {
        public PackageRecieved packageRecieved;
        public string type;

        public PackageRecievedData(PackageRecieved packageRecieved, string type)
        {
            this.packageRecieved = packageRecieved;
            this.type = type;
        }
    }

    public enum ClientEvent
    {
        ClientConnected,
        ClientDisconnected,
    }


    public static class ClientEventSystem
    {
        public static Dictionary<ClientEvent, List<ClientEventData>> ClientEvents = new Dictionary<ClientEvent, List<ClientEventData>>()
        {
            { ClientEvent.ClientConnected, emptyEventList },
            { ClientEvent.ClientDisconnected, emptyEventList },
        };

        private static List<ClientEventData> emptyEventList => new List<ClientEventData>() { new ClientEventData(new ClientEventFunction((target) => { })) };


        public static void On(ClientEvent eventType, ClientEventFunction action)
        {
            ClientEvents[eventType].Add(new ClientEventData(action));
        }

        public static void Once(ClientEvent eventType, ClientEventFunction action)
        {
            ClientEvents[eventType].Add(new ClientEventData(action, true));
        }

        public static void DisOn(ClientEvent eventType, ClientEventFunction action)
        {
            ClientEvents[eventType].Remove(new ClientEventData(action));
        }

        public static void DisOn(ClientEvent eventType, int id)
        {
            ClientEvents[eventType].RemoveAt(id);
        }

        public static void StartEvent(ClientEvent eventType)
        {
            List<ClientEventData> handlers = ClientEvents[eventType];

            for (int i = 0; i < handlers.Count; i++)
            {
                handlers[i].clientEvent(i);
                if (handlers[i].once)
                    DisOn(eventType, i);
            }
        }

        public static HashSet<PackageRecievedData> packageEvents = new HashSet<PackageRecievedData>();

        public static void OnPackageRecieved(PackageRecieved packageRecieved, string packageType = "Any")
        {
            packageEvents.Add(new PackageRecievedData(packageRecieved, packageType));
        }

        public static void PackageRecieved(byte[] data)
        {
            foreach (PackageRecievedData recievedData in packageEvents)
            {
                CheckerPackage package = new CheckerPackage(data);

                if (package.typeOf(recievedData.type) || recievedData.type == "Any")
                    recievedData.packageRecieved(data);
            }
        }
    }
}
