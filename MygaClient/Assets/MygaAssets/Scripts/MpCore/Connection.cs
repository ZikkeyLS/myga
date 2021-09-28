using UnityEngine;

namespace MygaClient
{
    public class Connection : MonoBehaviour
    {
        [SerializeField] private string ip = "127.0.0.1";
        [SerializeField] private int port = 25565;

        void Start()
        {
            Socket.Connect(ip, port);
        }
    }
}
