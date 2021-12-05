using MygaCross;
using System.Collections.Generic;
using UnityEngine;

namespace MygaClient
{
    public class MygaObject : MonoBehaviour
    {
        [ReadOnly] [SerializeField] private int id = 0;
        public int ID => id;

        [ReadOnly] [SerializeField] private int clientID = -1;
        public int ClientID => id;

        [ReadOnly] [SerializeField] private bool mine = false;
        public int Mine => id;

        public readonly List<Package> data = new List<Package>();

        public void Connect(int _id, int _clientID = -1, bool _mine = false)
        {
            id = _id;
            clientID = _clientID;
            mine = _mine ? _clientID >= 0 : false;

            if (_mine && _clientID < 0)
                    Debug.Log("You can't have a controllable object without having your client id on it.");
        }

        public void LinkData(Package _data)
        {
            data.Add(_data);
        }
    }
}


