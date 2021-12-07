using MygaCross;
using System.Collections.Generic;

namespace MygaServer
{
    public class MPObject
    {
        public int? clientID { get; private set; }
        public int id { get; private set; } = 0;
        public List<IMPAddon> addons { get; private set; } = new List<IMPAddon>();

        public virtual void Initialize(int id, int clientID = -1, in IMPAddon[] addons = null)
        {
            this.addons.AddRange(addons);
            Initialize(id, clientID);
        }

        public virtual void Initialize(int id, int clientID = -1, in List<IMPAddon> addons = null)
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
            foreach(IMPAddon addon in addons)
                if (addon is T)
                    return (T)addon;

            return default(T);
        }
    }
}
