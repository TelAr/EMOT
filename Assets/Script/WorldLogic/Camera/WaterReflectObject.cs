using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterReflectObject : MonoBehaviour
{

    public Camera WaterCamera;

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(transform.localScale.y * 16 / 9, transform.localScale.y, 1);
        WaterCamera.backgroundColor = Camera.main.backgroundColor;
        WaterCamera.orthographicSize = transform.localScale.y * 0.5f;
        WaterCamera.transform.position = new Vector3(transform.position.x, transform.position.y+transform.localScale.y*0.5f+WaterCamera.orthographicSize, WaterCamera.transform.position.z);

    }
}
