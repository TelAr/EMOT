using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{

    public int DamageValue = 0;
    public float ImmuneTime = 2;
    [Tooltip("This object can damage same team")]
    public bool isTeamKill;
    [Tooltip("Damage working flag")]
    public bool IsEffected = true;


    private void HurtFunction(Collider2D collision) {

        if ((!isTeamKill && collision.CompareTag(gameObject.tag)) || !IsEffected)
        {

            return;
        }

        if (collision.GetComponent<HealthDefault>())
        {

            collision.GetComponent<HealthDefault>().Hurt(this);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        HurtFunction(collision);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HurtFunction(collision.collider);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        HurtFunction(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        HurtFunction(collision.collider);
    }

}
