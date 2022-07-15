using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffDebuff
{
    public float SpeedVariance = 0;
    public float DamagePerSec = 0;
    public float Timer = 0;
    public bool IsBuff = false;
    public GameObject Attached;

    public delegate void DelegateFunc(GameObject Target);
    public List<DelegateFunc> DelegateContainers = new();

    private float DamageCalculator = 0;

    //Must to call LateUpdate()
    public bool Tick() { 
    
        Timer-=Time.deltaTime;
        //cycle is over, return false
        if (Timer < 0) return false;

        DamageCalculator+=DamagePerSec*Time.deltaTime;
        if (DamageCalculator > 1) { 
        
            DamageCalculator -= 1;
            if (Attached.GetComponent<HealthDefault>() != null) {
                Attached.GetComponent<HealthDefault>().UnimmuneHurt(1);
            }
        }

        if (Attached.GetComponent<Rigidbody2D>() != null) {
            Vector2 originVal = Attached.GetComponent<Rigidbody2D>().velocity;
            Attached.GetComponent<Rigidbody2D>().velocity = new Vector2(originVal.x * (1 + SpeedVariance), originVal.y);
        }

        foreach (var container in DelegateContainers) {

            container(Attached);
        }

        return true;
    }

}
