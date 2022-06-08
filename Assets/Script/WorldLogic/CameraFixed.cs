using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFixed : MonoBehaviour
{
    public float ZDistance = -10;
    public bool IsFixed = true;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) {

            if (IsFixed)
            {
                CameraTargetting.IsCameraPositionFix = true;
                CameraTargetting.MainCamera.transform.position = gameObject.transform.position + new Vector3(0, 0, ZDistance);
            }
            else {

                CameraTargetting.IsCameraPositionFix = false;
            }
        }
    }

}
