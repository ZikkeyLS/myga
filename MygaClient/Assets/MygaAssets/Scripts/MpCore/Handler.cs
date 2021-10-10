using MygaCross;
using UnityEngine;

namespace MygaClient
{
    public static class Handler
    {

        public static void ConnectEvents()
        {
            ClientEventSystem.OnPackageRecieved(OnPackage);
            ClientEventSystem.OnPackageRecieved(OnConnectPackage, "ServerIntroducePackage");
        }

        public static void OnPackage(byte[] data)
        {
        }

        public static void OnConnectPackage(byte[] data)
        {
            ServerIntroducePackage package = new ServerIntroducePackage(data);
            Debug.Log(package.message);
        }
    }
}
