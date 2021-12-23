using UnityEngine;

namespace MygaClient
{
    public class MPAddon : MonoBehaviour
    {
        protected MygaObject mygaObject;

        private void Start()
        {
            if(TryGetComponent(out MygaObject mygaObject))
            {
                mygaObject.AddAddon(this);
                this.mygaObject = mygaObject;
            }
        }
    }
}
