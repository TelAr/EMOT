using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PatternDefault : MonoBehaviour
{

    public float cooldown = 0;
    public int stack = 0;
    public EnemyDefault enemy = null;

    virtual public void Run() {
        Setting();
        enemy.pattern_running = true;
    }

    virtual public void Stop() { 
    
        enemy.pattern_running = false;
    }
    abstract public void Setting();
}
