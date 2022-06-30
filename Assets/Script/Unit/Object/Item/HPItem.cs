using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPItem : ItemDefault
{
    public int HealValue = 5;
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.GetComponent<HealthDefault>() != null) {

            collision.GetComponent<HealthDefault>().HealthChange(HealValue);
            gameObject.SetActive(false);
        }
    }
}
