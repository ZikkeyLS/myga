using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TNZ.TNZMonoBehaviour;

namespace TNZ
{
    public static class TNZNetwork
    {
        public static GameObject SpawnedObject = null;
        public static TNZObject PlayerTNZObject;
        public static List<TNZObject> TnzObjects = new List<TNZObject>();

        public static Room PlayerRoom = null;

        public static void ConnectToServer(string _ip, int _port)
        {
            Client.ConnectToServer(_ip, _port);
        }

        public static void Instantiate(GameObject gameObject, string name = "", bool mine = false)
        {
            ClientSend.Instantiate(gameObject, Vector3.zero, Quaternion.identity, name, mine);
        }

        public static void Instantiate(GameObject gameObject, Vector3 position, Quaternion rotation, string name = "", bool mine = false)
        {
            ClientSend.Instantiate(gameObject, position, rotation, name, mine);
        }

        public static void InstantiateWithResponse(GameObject gameObject, OnSpawned onSpawned, string name = "", bool mine = false)
        {
            Instantiate(gameObject, name, mine);
            ThreadManager.instance.StartCoroutine(WaitForGameObject(onSpawned));
        }

        public static void CreateRoom(string name, int maxPlayers)
        {
            ClientSend.SendCreateRoom(name, maxPlayers);
        }

        public static void ConnectToRoom(string name)
        {
            ClientSend.ConnectPlayerToRoom(name);
        }

        public static void DisconnectFromRoom(string name)
        {
            ClientSend.DisconnectPlayerFromRoom(name);
        }

        public static void InstantiateWithResponse(GameObject gameObject, Vector3 position, Quaternion rotation, OnSpawned onSpawned, string name = "", bool mine = false)
        {
            Instantiate(gameObject, position, rotation, name, mine);
            ThreadManager.instance.StartCoroutine(WaitForGameObject(onSpawned));
        }

        private static IEnumerator WaitForGameObject(OnSpawned onSpawned)
        {
            yield return new WaitUntil(() => SpawnedObject != null);
            onSpawned(SpawnedObject);
        }

        public static void LoadScene(int index)
        {
            ClientSend.LoadScene(index, "");
        }

        public static void LoadScene(string name)
        {
            ClientSend.LoadScene(-1, name);
        }

        public static void Destroy(int id)
        {
            ClientSend.Destroy(id);
        }
    }
}
