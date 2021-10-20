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
            PlayerLoginData loginPackage = new PlayerLoginData(nickname, password);
          
            StartCoroutine("test");
        }

        private IEnumerator test()
        {
            PlayerLoginData loginPackage = new PlayerLoginData("erere", "123");

            for(int i = 0; i < 10; i++)
            {
                Client.SendTCPData(loginPackage);
                yield return new WaitForSeconds(1);
            }
        }
    }
}
