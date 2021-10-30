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

        private int i = 0;

        private void Start()
        {
            print(new Vector3(0, 1, 2));
            Login();
        }

        private void Login()
        {
            Network.Connect(ip, port);
        }
    }
}
