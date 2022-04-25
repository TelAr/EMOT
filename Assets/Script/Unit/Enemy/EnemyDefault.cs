using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//적 개체 생성 시 기본적으로 작동되는 메커니즘
public class EnemyDefault : UnitDefault
{
    public string statement;
    public bool pattern_running;
    //패턴 관리용 개체
    public class PatternController { 
    
        PatternDefault pattern;
        float cooldown = 10f;
        float timer = 0;
        int stack = 1;
        int stackCounter = 0;
        readonly EnemyDefault enemy;

        public PatternController(PatternDefault pattern)
        {
            this.pattern = pattern;
            this.cooldown = pattern.cooldown;
            this.timer = 0;
            this.stack = pattern.stack;
            this.stackCounter = 0;
            this.enemy = pattern.enemy;
        }
        public void PatternReset()
        {
            this.timer = 0;
            this.stackCounter = 0;
            pattern.Setting();
        }
        public void Tick() {

            if (timer < cooldown)
            {
                this.timer += Time.deltaTime;
            }
            else {
                if (stackCounter < stack)
                {
                    stackCounter++;
                    this.timer = 0;
                }
            }
            if (stackCounter > 0&& !enemy.pattern_running) {

                pattern.Run();
                stackCounter--;
            }
        }
    }

    //패턴 보관용 컨테이너
    public List<PatternController> patternList;

    public void Awake()
    {
        patternList= new List<PatternController>();
        foreach (PatternDefault pattern in gameObject.GetComponents<PatternDefault>()) {
            pattern.enemy = this;
            PatternController PC = new PatternController(pattern);
            patternList.Add(PC);
        }
        Debug.Log(patternList.Count);
        statement = "normal";
    }

    public override void Start()
    {
        foreach (PatternController pattern in patternList) {

            pattern.PatternReset();
        }
        pattern_running = false;
    }
}
