using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class to fixed layer order
//-: front, +:back
public class LayerOrder : MonoBehaviour
{

    [Tooltip("Smaller value is front, value must be bigger than (-100)"), Range(-100f, 100f)]
    public float value = 0f;

    private void LateUpdate()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, value*0.05f);
    }
}
