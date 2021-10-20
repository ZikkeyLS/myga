using MygaCross;
using System.Collections;
using UnityEngine;

namespace MygaClient
{
    public class Connection : MonoBehaviour
    {
        [SerializeField] private string ip = "127.0.0.1";
        [SerializeField] private int port = 25565;

        [SerializeField] private string nickname = "Zikkey";
        [SerializeField] private string password = "123321";

        private void Start()
        {
            Login();
        }

        private void Update()
        {
            // PlayerLoginData loginPackage = new PlayerLoginData("erere", "123");
            // Client.SendTCPData(loginPackage);
        }

        private void Login()
        {
            Client.Connect(ip, port);
        }
    }
}
