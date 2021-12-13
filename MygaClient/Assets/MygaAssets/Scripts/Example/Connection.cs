using MygaCross;
using UnityEngine;

namespace MygaClient
{
    public class Connection : MonoBehaviour
    {
        [SerializeField] private string ip = "127.0.0.1";
        [SerializeField] private int port = 7777;

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
