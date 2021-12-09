using MygaCross;
using UnityEngine;

namespace MygaClient
{
    public class Connection : MonoBehaviour
    {
        [SerializeField] private string ip = "127.0.0.1";
        [SerializeField] private int port = 7777;

        [SerializeField] private string nickname = "Zikkey";
        [SerializeField] private string password = "123321";

        private void Start()
        {
            Login();
        }

        private void Login()
        {
            MygaNetwork.Connect(ip, port);
        }
    }
}
