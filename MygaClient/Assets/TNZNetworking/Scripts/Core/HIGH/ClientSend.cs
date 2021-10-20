using GameServer;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.tcp.SendData(_packet);
    }

    private static void SendUDPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.udp.SendData(_packet);
    }

    #region Packets
    public static void WelcomeReceived()
    {
        using (Packet _packet = new Packet((int)PacketsID.welcome))
        {
            _packet.Write(Client.myId);
            _packet.Write(PlayerUI.instance.usernameField.text);

            SendTCPData(_packet);
        }
    }

    public static void Instantiate(GameObject gameObject, Vector3 position, Quaternion rotation, string name, bool mine)
    {
        using (Packet _packet = new Packet((int)PacketsID.instantiate))
        {
            _packet.Write(gameObject);
            _packet.Write(position);
            _packet.Write(rotation);

            _packet.Write(name);
            _packet.Write(mine);

            SendTCPData(_packet);
        }
    }


    public static void Destroy(int id)
    {
        using (Packet _packet = new Packet((int)PacketsID.destroy))
        {
            _packet.Write(id);
            SendTCPData(_packet);
        }
    }

    public static void LoadScene(int index, string name)
    {
        using (Packet _packet = new Packet((int)PacketsID.SceneData))
        {
            _packet.Write(index);
            _packet.Write(name);
            SendTCPData(_packet);
        }
    }

    public static void GetActions()
    {
        using (Packet _packet = new Packet((int)PacketsID.Packets))
        {
            _packet.Write("GetActions");
            SendTCPData(_packet);
        }
    }


    public static void SendTransformData(TNZObject tnzObject, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        using (Packet _packet = new Packet((int)PacketsID.TransformData))
        {
            _packet.Write(tnzObject.ID);
            _packet.Write(position);
            _packet.Write(rotation);
            _packet.Write(scale);

            SendTCPData(_packet);
        }
    }

    public static void SendPhysicsData(TNZObject tnzObject, Vector2 velosity, Vector2 angularVelosity)
    {
        using (Packet _packet = new Packet((int)PacketsID.PhysicsData))
        {
            _packet.Write(tnzObject.ID);
            _packet.Write(velosity);
            // TODO : Sync angular velosity

            SendTCPData(_packet);
        }
    }

    public static void SendCreateRoom(string name, int maxPlayers)
    {
        using(Packet _packet = new Packet((int)PacketsID.CreateRoom))
        {
            _packet.Write(name);
            _packet.Write(maxPlayers);
            SendTCPData(_packet);
        }

        ConnectPlayerToRoom(name);
    }

    public static void ConnectPlayerToRoom(string name)
    {
        using (Packet _packet = new Packet((int)PacketsID.PlayerAddRoom))
        {
            _packet.Write(name);
            SendTCPData(_packet);
        }
    }

    public static void DisconnectPlayerFromRoom(string name)
    {
        using (Packet _packet = new Packet((int)PacketsID.PlayerRemoveRoom))
        {
            _packet.Write(name);
            SendTCPData(_packet);
        }
    }
    #endregion
}
