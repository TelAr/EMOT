using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionImpactDefault : ImpactDefault
{
    public float ExplosionSize;

    private float explosionSizeOffset = 8;
    public override void Impact(GameObject target = null)
    {
        GameObject ExplosioInstance = EffectPoolingController.EffectObjectController.GetComponent<EffectPoolingController>().GetExplosion();
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
