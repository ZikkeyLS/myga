using MygaCross;
using UnityEngine;

namespace MygaClient
{
    public class Connection : MonoBehaviour
    {
        [SerializeField] private string ip = "127.0.0.1";
        [SerializeField] private int port = 25565;

        [SerializeField] private string nickname = "Zikkey";
        [SerializeField] private string password = "123321";

        void Start()
        {
            Login();
        }

        private void Login()
        {
            Socket.Connect(ip, port);
            PlayerLoginData loginPackage = new PlayerLoginData(nickname, password);
            Socket.SendTCPData(loginPackage);
        }
    }
}
