using MygaClient;
using UnityEngine;

public class Connect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MygaNetwork.Connect("127.0.0.1", 7777);
        ClientEventSystem.On(ClientEvent.ClientConnected, (id) => { Debug.Log("43434343"); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
