using TNZ;
using UnityEngine;

public class Movement : TNZMonoBehaviour
{
    [SerializeField] private int speed = 5;

    private void Update()
    {
        if (!tnzObject.Mine)
            return;

        int MoveA = Input.GetKey(KeyCode.A) ? -1 : 0;
        int MoveD = Input.GetKey(KeyCode.D) ? 1 : 0;

        transform.position += new Vector3((MoveA + MoveD) * speed * Time.deltaTime, 0, 0);
    }
}
