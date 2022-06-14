using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeparateGrenadeImpact : ExplosionImpactDefault
{
    public int SeparateCount;
    public override void Impact(GameObject target = null)
    {
        Debug.Log(gameObject.GetComponent<Rigidbody2D>().velocity);
        if (SeparateCount > 0) {

            for (int t = 0; t < 2; t++)
            {
                GameObject childGrenade = Instantiate(gameObject);
                //childGrenade.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                childGrenade.transform.position = transform.position - (Vector3)gameObject.GetComponent<Rigidbody2D>().velocity.normalized * transform.localScale.magnitude * 0.5f;
                childGrenade.transform.localScale = transform.localScale * 0.5f;
                childGrenade.GetComponent<GrenadeDefault>().ResetTimer();
                childGrenade.GetComponent<GrenadeDefault>().IsDestroy = true;
                childGrenade.GetComponent<SeparateGrenadeImpact>().SeparateCount = SeparateCount - 1;
                childGrenade.GetComponent<Rigidbody2D>().velocity = 
                    Ballistics.Ballistic(new Vector2(10*gameObject.transform.localScale.x*(t*2-1),0),20 * gameObject.transform.localScale.x, GameController.GetGameController().GRAVITY);
            }
            
        }
        base.Impact(target);
    }


}
