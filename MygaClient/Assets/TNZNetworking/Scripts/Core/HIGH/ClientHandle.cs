using GameServer;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;
using static TNZTransform;

namespace TNZ
{
    public class ClientHandle : MonoBehaviour
    {
        public static void Welcome(Packet _packet)
        {
            string _msg = _packet.ReadString();
            int _myId = _packet.ReadInt();

            Debug.Log($"Message from server: {_msg}");
            Client.myId = _myId;
            ClientSend.WelcomeReceived();

            Client.udp.Connect(((IPEndPoint)Client.tcp.socket.Client.LocalEndPoint).Port);
        }

        public static void Instantiate(Packet _packet)
        {
            GameObject prefab = _packet.ReadGameObject();
            Vector3 position = _packet.ReadVector3();
            Quaternion rotation = _packet.ReadQuaternion();

            string name = _packet.ReadString();
            bool haveOwner = _packet.ReadBool();
            int id = _packet.ReadInt();
            bool owner = _packet.ReadBool();

            GameObject gameObject = Instantiate(prefab, position, rotation);
            TNZObject tnzObject = gameObject.GetComponent<TNZObject>();
            if (tnzObject != null)
            {
                TNZNetwork.TnzObjects.Add(tnzObject);
                tnzObject.Setup(id, name, owner, haveOwner);
                if (owner) TNZNetwork.PlayerTNZObject = tnzObject;
            }
            TNZNetwork.SpawnedObject = gameObject;
            TNZMonoBehaviour.OnInstantiated(tnzObject);
        }

        public static void Destroy(Packet _packet)
        {
            int id = _packet.ReadInt();
            Destroy(TNZObject.Find(id).gameObject);
        }

        public static void LoadScene(Packet _packet)
        {
            int index = _packet.ReadInt();
            string name = _packet.ReadString();
            if(index != -1) { SceneManager.LoadScene(index); }
            else if(name != string.Empty) { SceneManager.LoadScene(name); }
            
        }

        public static void GetTransformData(Packet _packet)
        {
            Packet readPacket = (Packet)_packet.Clone();
            int id = readPacket.ReadInt();
            Vector3 position = readPacket.ReadVector3();
            Quaternion rotation = readPacket.ReadQuaternion();
            Vector3 localScale = readPacket.ReadVector3();

            TNZObject tnzObject = TNZObject.Find(id);
            if (tnzObject == null)
                return;
            TNZTransform transform = tnzObject.GetComponent<TNZTransform>();

            transform.serverTransform = new ServerTransform(position, rotation, localScale);
            transform.localTransform = new ServerTransform(position, rotation, localScale);
        }

        public static void GetPhysicsData(Packet _packet)
        {
            Packet readPacket = (Packet)_packet.Clone();
            int id = readPacket.ReadInt();
            Vector2 velosity = readPacket.ReadVector3();

            TNZObject tnzObject = TNZObject.Find(id);
            TNZRigidbody tnzRigidbody = tnzObject.GetComponent<TNZRigidbody>();
            tnzRigidbody.serverVelosity = velosity;
        }

        public static void GetConnectRoomResponse(Packet _packet)
        {
            bool succesed = _packet.ReadBool();
            string errorCode = _packet.ReadString();
            int playerID = _packet.ReadInt();
            string roomName = _packet.ReadString();

            if (succesed)
            {
                if(playerID == Client.myId)
                    Debug.Log("You successfully connected to room: " + roomName);

                int maxPlayers = _packet.ReadInt();
                int currentPlayers = _packet.ReadInt();
                TNZNetwork.PlayerRoom = new Room();
                TNZNetwork.PlayerRoom.Initialise(roomName, maxPlayers, currentPlayers);

                TNZMonoBehaviour.OnRoomConnection(TNZNetwork.PlayerRoom);
            }
            else if(playerID == Client.myId)
            {
                Debug.LogError(errorCode);
            }

        }

        public static void GetDisconnectRoomRespose(Packet _packet)
        {
            int playerID = _packet.ReadInt();
            string name = _packet.ReadString();
            int maxPlayers = _packet.ReadInt();
            int currentPlayers = _packet.ReadInt();
            TNZNetwork.PlayerRoom.Initialise(name, maxPlayers, currentPlayers);

            TNZMonoBehaviour.OnRoomDisconnection(TNZNetwork.PlayerRoom);

            if(playerID == Client.myId)
                TNZNetwork.PlayerRoom = null;
        }
    }
}
