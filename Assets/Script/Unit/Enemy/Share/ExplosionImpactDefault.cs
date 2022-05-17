using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionImpactDefault : ImpactDefault
{
    public float ExplosionSize;
    public GameObject ExplosionModel;
    private float explosionSizeOffset = 2;
    public override void Impact(GameObject target = null)
    {
        GameObject ExplosioInstance = Instantiate(ExplosionModel);
        ExplosioInstance.transform.position = transform.position-new Vector3(0,0,1);
        if (ExplosionSize < 0)
        {

            ExplosioInstance.transform.localScale *= transform.localScale.x * explosionSizeOffset;
        }
        else {

            ExplosioInstance.transform.localScale *= ExplosionSize;
        }
    }
}
