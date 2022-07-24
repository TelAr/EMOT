using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargetting : MonoBehaviour
{
    public static CameraTargetting MainCamera;
    public bool IsCameraPositionFix = false;

    public Vector3 Offset;
    public GameObject target;
    public float SmoothTime = 0.3f;

    private Vector3 vel = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        target = GameController.GetPlayer;
        MainCamera = this;
    }

    private void FixedUpdate()
    {
        if (!IsCameraPositionFix)
        {
            float zValue = transform.position.z;
            transform.position = Vector3.SmoothDamp(transform.position, Offset + (target.GetComponent<PlayerPhysical>() ?
                target.GetComponent<PlayerPhysical>().TargettingPos
                : target.transform.position), ref vel, SmoothTime);


            transform.position=new Vector3(transform.position.x, transform.position.y, zValue);
        }
    }

}
