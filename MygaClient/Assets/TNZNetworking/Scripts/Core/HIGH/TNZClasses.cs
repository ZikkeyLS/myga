using System.Collections.Generic;
using UnityEngine;

namespace TNZ
{
    [RequireComponent(typeof(TNZObject))]
    public class TNZMonoBehaviour : MonoBehaviour, ITNZCallbacks
    {
        static List<TNZMonoBehaviour> callbacks = new List<TNZMonoBehaviour>();
        public delegate void OnSpawned(GameObject gameObject);
        [HideInInspector] public TNZObject tnzObject;

        private void Start()
        {
            callbacks.Add(this);
            tnzObject = GetComponent<TNZObject>();
        }

        public static void OnInstantiated(TNZObject tnzObject)
        {
            foreach(ITNZCallbacks target in callbacks)
            {
                target.OnObjectInstantiated(tnzObject);
            }
        }

        public static void OnRoomConnection(Room room) 
        {
            foreach (ITNZCallbacks target in callbacks)
            {
                target.OnConnectedToRoom(room);
            }
        }
        
        public static void OnRoomDisconnection(Room room)
        {
            foreach (ITNZCallbacks target in callbacks)
            {
                target.OnDisconnectedFromRoom(room);
            }
        }



        public virtual void OnObjectInstantiated(TNZObject tnzObject) 
        {
        }

        public virtual void OnConnectedToRoom(Room room)
        {
        }

        public virtual void OnDisconnectedFromRoom(Room room)
        {

        }
    }

    interface ITNZCallbacks
    {
        void OnObjectInstantiated(TNZObject tnzObject);
        void OnConnectedToRoom(Room room);
        void OnDisconnectedFromRoom(Room room);
    }
}

