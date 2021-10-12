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
            ClientEventSystem.OnPackageRecieved(OnErrorPackage, "ErrorPackage");
        }

        public static void OnPackage(byte[] data)
        {
        }

        public static void OnConnectPackage(byte[] data)
        {
            ServerIntroducePackage package = new ServerIntroducePackage(data);
            Debug.Log(package.message);
        }

        public static void OnErrorPackage(byte[] data)
        {
            ErrorPackage package = new ErrorPackage(data);
            Debug.LogWarning(package.message);
        }
    }
}
