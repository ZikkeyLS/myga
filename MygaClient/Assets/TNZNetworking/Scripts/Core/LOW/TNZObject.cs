using TNZ;
using UnityEngine;

public class TNZObject : MonoBehaviour
{
    public int ID { get; private set; } = 10000;
    public string Nickname { get; private set; } = string.Empty;
    public bool Mine { get; private set; } = false;
    public bool HaveOwner { get; private set; } = false;

    public void Setup(int id, string name, bool mine, bool haveOwner)
    {
        ID = id;
        Nickname = name;
        Mine = mine;
        HaveOwner = haveOwner;
    }

    public static TNZObject Find(int id)
    {
        for(int i = 0; i < TNZNetwork.TnzObjects.Count; i++)
        {
            if(TNZNetwork.TnzObjects[i].ID == id)
                return TNZNetwork.TnzObjects[i];
        }
        return null;
    }
}
