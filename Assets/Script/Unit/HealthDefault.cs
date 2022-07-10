using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDefault : MonoBehaviour
{
    public int HealthMax = 100;

    protected int health;
    protected float immunTimer = 0;

    public int GetHealth
    {
        get { return health; }
    }
    public int SetHealth
    {

        set { health = value < HealthMax ? value : HealthMax; }
    }

    public void Reset()
    {
        HealthMax = 100;
    }


    protected virtual void Awake()
    {
        FullHealth();
    }


    public virtual void SetMaxHealth(int value)
    {
        HealthMax = value;
    }

    public void HealthChange(int value)
    {
        health += value;

        if (health > HealthMax)
        {

            health = HealthMax;
        }
    }

    public void FullHealth()
    {

        health = HealthMax;
    }

    public void SetImmuneTime(float value)
    {

        immunTimer = value;
    }

    public void Hurt(Damage damage)
    {

        int damagevalue = damage.DamageValue;
        float immuneTime = damage.ImmuneTime;
        Hurt(damagevalue, immuneTime);
    }

    public virtual void Hurt(int damage = 0, float immuneTime = 2f)
    {
        if (immunTimer <= 0) {
            HealthChange(-damage);
            SetImmuneTime(immuneTime);
        }
        
    }

}
