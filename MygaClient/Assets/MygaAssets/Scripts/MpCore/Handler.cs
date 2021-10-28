using MygaCross;
using UnityEngine;

namespace MygaClient
{
    public static class Handler
    {

        public static void ConnectEvents()
        {
            ClientEventSystem.OnPackageRecieved(OnPackage);
            ClientEventSystem.OnPackageRecieved(OnConnectPackage, "IntroducePackage");
            ClientEventSystem.OnPackageRecieved(OnErrorPackage, "ErrorPackage");
        }

        public static void OnPackage(byte[] data)
        {
        }

        public static void OnConnectPackage(byte[] data)
        {
            IntroducePackage package = new IntroducePackage(data);
            Debug.Log(package.message);
            IntroducePackage returnedpa = new IntroducePackage("ere");
           // Client.SendUDPData(returnedpa);
        }

        public static void OnErrorPackage(byte[] data)
        {
            ErrorPackage package = new ErrorPackage(data);
            Debug.LogWarning(package.message);
        }
    }
}
