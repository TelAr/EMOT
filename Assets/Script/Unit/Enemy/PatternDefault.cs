using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PatternDefault : MonoBehaviour
{

    public float cooldown = 0;
    public int stack = 0;
    public float max_distance, min_distance;
    public float post_delay=0;
    public EnemyDefault caster = null;
    public bool is_run = false;
    public bool is_main = true;

    virtual public void Run() {
        Setting();
        is_run = true;
        if (is_main) {
            caster.pattern_running = true;
        }
        
    }


    virtual public void Stop() {

        is_run = false;

        if (is_main) {
            caster.pattern_running = false;
            caster.statement = "normal";
        }

    }
    abstract public void Setting();
}
