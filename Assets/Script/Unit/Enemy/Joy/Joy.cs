using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joy : EnemyDefault
{
    public int health;
    const int MAX_HEALTH = 5;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        health = MAX_HEALTH;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0) { 
        
            //보스 패턴 종료
        }
        foreach (PatternController patternController in patternList) {

            patternController.Tick();
        }
    }
}
