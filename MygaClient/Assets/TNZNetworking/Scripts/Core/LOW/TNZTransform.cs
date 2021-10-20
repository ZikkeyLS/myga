using System.Collections;
using UnityEngine;

[RequireComponent(typeof(TNZObject))]
public class TNZTransform : MonoBehaviour
{
    public class ServerTransform
    {
        public ServerTransform(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            serverPosition = position;
            serverRotation = rotation;
            serverScale = scale;
        }

        public bool TEquals(ServerTransform serverTransform)
        {
            return serverTransform.serverPosition == serverPosition &&
                serverTransform.serverRotation == serverRotation && serverTransform.serverScale == serverScale;
        }

        public Vector3 serverPosition;
        public Quaternion serverRotation;
        public Vector3 serverScale;
    }

    public ServerTransform serverTransform;
    public ServerTransform localTransform;
    public ServerTransform lastTransform;

    public void Awake()
    {
        localTransform = new ServerTransform(transform.position, transform.rotation, transform.localScale);
        lastTransform = new ServerTransform(transform.position, transform.rotation, transform.localScale);
    }

    public void Start()
    {
       StartCoroutine(SendTransform());
    }

    private void Update()
    {
        ServerTransform currentTransform = new ServerTransform(transform.position, transform.rotation, transform.localScale);
        if (!currentTransform.TEquals(lastTransform))
        {
            localTransform = currentTransform;
            return;
        }

        if (serverTransform == null)
            return;

        transform.position = Vector3.Lerp(serverTransform.serverPosition, transform.position, 0.75f);
        transform.rotation = Quaternion.Lerp(serverTransform.serverRotation, transform.rotation, 0.75f);
        transform.localScale = Vector3.Lerp(serverTransform.serverScale, transform.localScale, 0.75f);

        lastTransform = new ServerTransform(transform.position, transform.rotation, transform.localScale);
    }

    private void SendTransformData()
    {
        ClientSend.SendTransformData(GetComponent<TNZObject>(), localTransform.serverPosition, localTransform.serverRotation, localTransform.serverScale);
    }

    private IEnumerator SendTransform()
    {
        yield return new WaitForSeconds(1 / (float)TNZData.Tickrate);

        if(serverTransform != null && localTransform != null && !localTransform.TEquals(serverTransform))
        {
            SendTransformData();   
        }
        if(serverTransform == null && localTransform != null)
        {
            SendTransformData();
        }


        StartCoroutine(SendTransform());
    }
}
