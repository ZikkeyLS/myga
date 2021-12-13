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

        public List<IMPAddon> addons { get; private set; } = new List<IMPAddon>();

        public virtual void Initialize(int id, int clientID = -1, IMPAddon[] addons = null)
        {
            this.addons.AddRange(addons);
            Initialize(id, clientID);
        }

        public virtual void Initialize(int id, int clientID = -1, List<IMPAddon> addons = null)
        {
            this.addons.AddRange(addons);
            Initialize(id, clientID);
        }

        public virtual void Initialize(int id, int clientID = -1)
        {
            this.id = id;
            if (clientID >= 0)
                this.clientID = clientID;
        }

        public T GetAddon<T>()
        {
            foreach (IMPAddon addon in addons)
                if (addon is T)
                    return (T)addon;

            return default(T);
        }
    }
}


