using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//적 개체 생성 시 기본적으로 작동되는 메커니즘
public class EnemyDefault : UnitDefault
{
    public string statement;
    public bool pattern_running;
    public float global_delay;

    //패턴 관리용 개체
    public class PatternController { 
    
        PatternDefault pattern;
        float cooldown = 10f;
        float timer = 0;
        float max_distance = 0;
        float min_distance = 0;
        float post_delay = 0;
        int stack = 1;
        int stackCounter = 0;
        public bool Is_Enabled;
        readonly EnemyDefault enemy;

        public PatternController(PatternDefault pattern)
        {
            this.pattern = pattern;
            this.cooldown = pattern.Cooldown;
            this.timer = 0;
            this.stack = pattern.Stack;
            this.stackCounter = 0;
            this.enemy = pattern.Caster;
            this.max_distance = pattern.MaxDistance;
            this.min_distance = pattern.MinDistance;
            this.post_delay = pattern.PatternPreDelay;
            Is_Enabled = true;
        }
        public void PatternReset()
        {
            this.timer = 0;
            this.stackCounter = 0;
            pattern.Setting();
        }
        public void Tick() {

            float distance = (enemy.gameObject.transform.position - GameController.GetPlayer().transform.position).magnitude;
            UpdatePatternInfo();
            if (!Is_Enabled) {

                return;
            }

            if (timer < cooldown)
            {
                this.timer += Time.deltaTime;

            }
            else
            {
                if (stackCounter < stack)
                {
                    enemy.PatternQueue.Enqueue(this);
                    stackCounter++;
                    this.timer = 0;
                }

            }
        }
        public void Run() {

            pattern.IsMain = true;
            pattern.Run();
            enemy.global_delay = post_delay;
            stackCounter--;
        }
        public void ForcedRun() {

            pattern.Run();
        }
        public void UpdatePatternInfo() {

            this.cooldown = pattern.Cooldown;
            this.stack = pattern.Stack;
            this.max_distance = pattern.MaxDistance;
            this.min_distance = pattern.MinDistance;
            this.post_delay = pattern.PatternPreDelay;
        }
        public string GetPatternName() {

            return pattern.ToString();
        }
    }

    //패턴 보관용 컨테이너
    public List<PatternController> PatternList;
    public Queue<PatternController> PatternQueue = new Queue<PatternController>();

    public virtual new void Update()
    {

        base.Update();
        if (!pattern_running) {
            global_delay -= Time.deltaTime;
            if (PatternQueue.Count > 0&&global_delay<0)
            {

                PatternQueue.Dequeue().Run();
            }
        }
        foreach (PatternController patternController in PatternList)
        {

            patternController.Tick();
        }

    }

    protected override void Awake()
    {
        base.Awake();
        PatternList= new List<PatternController>();
        foreach (PatternDefault pattern in gameObject.GetComponents<PatternDefault>()) {

            if (pattern.enabled) {
                pattern.Caster = this;
                PatternController PC = new(pattern);
                PatternList.Add(PC);
            }
        }
        statement = "normal";
        global_delay = 0;
        gameObject.transform.position = DefaultPos;
    }

    public override void Start()
    {
        foreach (PatternController pattern in PatternList) {
            
            pattern.PatternReset();
        }
        pattern_running = false;
    }
}
