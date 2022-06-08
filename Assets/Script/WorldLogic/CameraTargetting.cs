using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargetting : MonoBehaviour
{
    public static GameObject MainCamera;
    public static bool IsCameraPositionFix = false;

    public Vector3 Offset;
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        target = GameController.GetPlayer();
        MainCamera = gameObject;
    }

    private void Update()
    {
        if (!IsCameraPositionFix) {

            transform.position = Offset + target.transform.position;
        }
    }

}
