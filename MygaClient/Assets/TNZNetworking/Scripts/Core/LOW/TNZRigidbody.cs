using GameServer;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(TNZObject), typeof(Rigidbody))]
public class TNZRigidbody : MonoBehaviour
{
    private Rigidbody physics;

    public Vector3 clientVelosity;
    public Vector3 serverVelosity;
    public Vector3 lastVelosity;

    public Vector3 clientAngularVelosity;
    public Vector3 serverAngularVelosity;
    public Vector3 lastAngularVelosity;

    public void Awake()
    {
        physics = GetComponent<Rigidbody>();
        clientVelosity = physics.velocity;
        lastVelosity = physics.velocity;
    }

    public void Start()
    {
        StartCoroutine(SendTransform());
    }

    private void Update()
    {
        Vector3 currentVelosity = physics.velocity;
        if (currentVelosity != lastVelosity)
        {
            clientVelosity = currentVelosity;
            return;
        }

        if (serverVelosity == null)
            return;

        physics.velocity = serverVelosity;
        lastVelosity = physics.velocity;
    }

    private void SendPhysicsData()
    {
        ClientSend.SendPhysicsData(GetComponent<TNZObject>(), clientVelosity, clientAngularVelosity) ;
    }

    private IEnumerator SendTransform()
    {
        yield return new WaitForSeconds(1 / (float)TNZData.Tickrate);

        if (serverVelosity != null && clientVelosity != null && clientVelosity != serverVelosity)
        {
            SendPhysicsData();
        }
        if (serverVelosity == null && clientVelosity != null)
        {
            SendPhysicsData();
        }

        StartCoroutine(SendTransform());
    }
}
