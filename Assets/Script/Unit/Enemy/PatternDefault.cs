using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PatternDefault : MonoBehaviour
{

    public float cooldown = 0;
    public int stack = 0;
    public EnemyDefault enemy = null;
    public bool is_run = false;

    virtual public void Run() {
        Setting();
        is_run = true;
        enemy.pattern_running = true;
    }

    virtual public void Stop() {

        is_run = false;
        enemy.pattern_running = false;
        enemy.statement = "normal";
    }
    abstract public void Setting();
}
