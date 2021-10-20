using TNZ;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerUI : TNZMonoBehaviour
{
    public static PlayerUI instance;

    public GameObject startMenu;
    public InputField usernameField;
    public string nextSceneName = "TNZExapmple2";

    public GameObject testPrefab;

    [SerializeField] private string ip = "127.0.0.1";
    [SerializeField] private int port = 25565;
   

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    public void ConnectToServer()
    {
        startMenu.SetActive(false);
        usernameField.interactable = false;
        TNZNetwork.ConnectToServer(ip, port);
    }


    public void InstantiateTest(GameObject button)
    {
        TNZNetwork.InstantiateWithResponse(testPrefab, Vector3.zero, Quaternion.identity, (response) => {
        /* LAMBDA FUNCTION THAT RETURNS SPAWNED OBJECT FROM SERVER 
         * EXAMPLE CODE
            print(response.GetComponent<TNZObject>().Nickname); 
         * EXAMPLE CODE
        */
        }, "YOUR_NAME", true);
        button.SetActive(false);
    }

    public void ConnectToRoomTest(InputField input)
    {
        TNZNetwork.ConnectToRoom(input.text);
    }

    public void LoadSceneTest()
    {
        TNZNetwork.LoadScene(nextSceneName);
    }

    public override void OnObjectInstantiated(TNZObject tnzObject)
    {
        // base.OnObjectInstantiated(tnzObject);
        /* EXAMPLE OF USING TNZ EVENT SYSTEM
         * EXAMPLE CODE
              print(tnzObject.ID);
         * EXAMPLE CODE
        */
    }

    public override void OnConnectedToRoom(Room room)
    {
        // base.OnConnectedToRoom();
        /* EXAMPLE OF USING TNZ EVENT SYSTEM
         * EXAMPLE CODE
                print("YEY. HELLO ROOM! WE HAVE " + room.currentPlayers + "/" + room.maxPlayers + " PLAYERS!");
         * EXAMPLE CODE
        */
    }

    public override void OnDisconnectedFromRoom(Room room)
    {
        // base.OnDisconnectedFromRoom(room);
        /* EXAMPLE OF USING TNZ EVENT SYSTEM
         * EXAMPLE CODE
            print("GOODBYE MY FRIEND! NOW WE HAVE " + room.currentPlayers + "/" + room.maxPlayers + " PLAYERS!");
         * EXAMPLE CODE
        */
    }

    private void OnApplicationQuit()
    {
        if (TNZNetwork.PlayerTNZObject != null)
            TNZNetwork.Destroy(TNZNetwork.PlayerTNZObject.ID);
    }
}
