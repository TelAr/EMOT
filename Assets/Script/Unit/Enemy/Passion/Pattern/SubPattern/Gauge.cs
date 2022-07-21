using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gauge : MonoBehaviour
{
    public GameObject Target;

    private float gaugevalue;


    public float GaugeValue {

        set {

            gaugevalue = value > 0 ? (value < 1 ? value : 1) : 0;
        }
    }
    private void Update()
    {
        Target.transform.localScale = new Vector3(gaugevalue, 1, 1);
    }
}
