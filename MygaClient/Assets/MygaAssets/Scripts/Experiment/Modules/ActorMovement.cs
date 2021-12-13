using MygaCross;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorMovement : IMPAddon
{
    public string error { get => error; set => error = value; }

    private float speed = 1; 

    public void Intitialize(params object[] parametres)
    {
        
    }
}
