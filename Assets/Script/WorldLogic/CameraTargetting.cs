using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargetting : MonoBehaviour
{

    public Vector3 Offset;
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        target = GameController.GetPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Offset+target.transform.position;
    }
}
