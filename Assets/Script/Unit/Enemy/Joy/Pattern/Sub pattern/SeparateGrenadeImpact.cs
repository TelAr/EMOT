using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeparateGrenadeImpact : ImpactDefault
{
    public int SeparateCount;
    public float ExplosionSize;
    public GameObject ExplosionModel;
    private float explosionSizeOffset = 2;
    public override void Impact(GameObject target = null)
    {
        if (SeparateCount > 0) {

            for (int t = 0; t < 2; t++)
            {
                GameObject childGrenade = Instantiate(gameObject);
                //childGrenade.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                childGrenade.transform.position = transform.position;
                childGrenade.transform.localScale = transform.localScale * 0.5f;
                childGrenade.GetComponent<GrenadeDefault>().IsDestroy = true;
                childGrenade.GetComponent<SeparateGrenadeImpact>().SeparateCount = SeparateCount - 1;
                childGrenade.GetComponent<Rigidbody2D>().velocity = 
                    Ballistics.Ballistic(new Vector2(10*gameObject.transform.localScale.x*(t*2-1),0),20 * gameObject.transform.localScale.x, GameController.GRAVITY);
            }
            
        }
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
