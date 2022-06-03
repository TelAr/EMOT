using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{

    public int DamageValue = 0;
    public float ImmuneTime = 2;

    public bool isTeamKill;

    public bool IsEffected = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((!isTeamKill && collision.CompareTag(gameObject.tag)) || !IsEffected) {

            return;
        }

        if (collision.GetComponent<HealthDefault>()) {

            collision.GetComponent<HealthDefault>().Hurt(this);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((!isTeamKill && collision.collider.CompareTag(gameObject.tag)) || !IsEffected)
        {

            return;
        }

        if (collision.collider.GetComponent<HealthDefault>())
        {

            collision.collider.GetComponent<HealthDefault>().Hurt(this);
        }
    }

}
