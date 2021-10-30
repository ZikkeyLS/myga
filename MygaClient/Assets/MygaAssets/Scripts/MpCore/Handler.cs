﻿using MygaCross;
using UnityEngine;

namespace MygaClient
{
    public static class Handler
    {

        public static void ConnectEvents()
        {
            ClientEventSystem.OnPackageRecieved(OnPackage);
            ClientEventSystem.OnPackageRecieved(OnIntroducePackage, "IntroducePackage");
            ClientEventSystem.OnPackageRecieved(OnErrorPackage, "ErrorPackage");
            ClientEventSystem.OnPackageRecieved(OnConnectPackage, "ConnectPackage");
        }

        public static void OnPackage(byte[] data)
        {
            using (CheckerPackage checkerPackage = new CheckerPackage(data))
                Debug.Log(checkerPackage.packageType);
        }

        public static void OnIntroducePackage(byte[] data)
        {
            using (IntroducePackage package = new IntroducePackage(data))
                Debug.Log(package.message);
        }

        public static void OnErrorPackage(byte[] data)
        {
            using (ErrorPackage package = new ErrorPackage(data))
            {
                Debug.LogWarning(package.message);
                if (package.disconnect)
                    Client.Disconnect();
            }
        }

        public static void OnConnectPackage(byte[] data)
        {
            using(ConnectPackage package = new ConnectPackage(data))
            {
                switch (package.status)
                {
                    case ConnectStatus.connected:
                        Client.SetConnectStatus(true);
                        break;
                    case ConnectStatus.full:
                        Debug.LogWarning($"Can't connect to server: {Client.serverIp}:{Client.serverPort}. Server is full!");
                        break;
                }
            }
        }
    }
}