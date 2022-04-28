using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�� ��ü ���� �� �⺻������ �۵��Ǵ� ��Ŀ����
public class EnemyDefault : UnitDefault
{
    public string statement;
    public bool pattern_running;
    public float global_delay;
    //���� ������ ��ü
    public class PatternController { 
    
        PatternDefault pattern;
        float cooldown = 10f;
        float timer = 0;
        float max_distance = 0;
        float min_distance = 0;
        float post_delay = 0;
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
            this.enemy = pattern.caster;
            this.max_distance = pattern.max_distance;
            this.min_distance = pattern.min_distance;
            this.post_delay = pattern.post_delay;
        }
        public void PatternReset()
        {
            this.timer = 0;
            this.stackCounter = 0;
            pattern.Setting();
        }
        public void Tick() {

            float distance = (enemy.gameObject.transform.position - GameController.GetPlayer().transform.position).magnitude;
//          Debug.Log(this.timer+"/"+this.cooldown+","+this.stack+": "+this.pattern);
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
            if (stackCounter > 0
                && !enemy.pattern_running
                && distance >= min_distance && distance <= max_distance
                && enemy.global_delay<=0) {

                pattern.Run();
                enemy.global_delay = post_delay;
                stackCounter--;
            }
        }
        public void ForcedRun() {

            pattern.Run();
        }
    }

    //���� ������ �����̳�
    public List<PatternController> patternList;

    public virtual void Update() {

        if (!pattern_running) {
            global_delay -= Time.deltaTime;
        }
        
    }

    public void Awake()
    {
        patternList= new List<PatternController>();
        foreach (PatternDefault pattern in gameObject.GetComponents<PatternDefault>()) {
            pattern.caster = this;
            PatternController PC = new PatternController(pattern);
            patternList.Add(PC);
        }
        Debug.Log(patternList.Count);
        statement = "normal";
        global_delay = 0;
    }

    public override void Start()
    {
        foreach (PatternController pattern in patternList) {

            pattern.PatternReset();
        }
        pattern_running = false;
    }
}
