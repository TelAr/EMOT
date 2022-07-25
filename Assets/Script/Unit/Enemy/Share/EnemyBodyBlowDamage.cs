using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBodyBlowDamage : Damage
{
    private void Awake()
    {
        gameObject.AddComponent<BoxCollider2D>();
        BoxColliderDeepCopyFromParent();
    }

    public void BoxColliderDeepCopyFromParent()
    {
        BoxCollider2D bc2d = gameObject.GetComponentInParent<BoxCollider2D>();
        System.Type type = bc2d.GetType();
        System.Reflection.FieldInfo[] fields = type.GetFields();
        foreach (var field in fields)
        {
            field.SetValue(gameObject.GetComponent<BoxCollider2D>(), field.GetValue(bc2d));
        }

        gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
    }
}
