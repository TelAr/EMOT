using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyDefault : UnitDefault
{
    public string Statement;
    public bool PatternRunning;
    public bool DefaultPhysicalForcedEnable;
    [Tooltip("Grobal Delay when Pattern is over")]
    public float GlobalDelay;
    [Tooltip("Front value: HP condition, Second value: Order'\n'" +
        "Format must be split whitespace, like \"0 1 4 10 7\"... \n" +
        "Pattern number statr from 0")]
    public List<KeyValuePair<int, string>> PatternOrderList = new List<KeyValuePair<int, string>>();

    //Class to controller pattern
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

        //init
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
            this.post_delay = pattern.PatternPostDelay;
            Is_Enabled = true;
        }

        public void PatternReset()
        {
            this.timer = 0;
            this.stackCounter = 0;
            pattern.Setting();
        }

        public void Tick() {

            float distance = (enemy.gameObject.transform.position - GameController.GetPlayer.transform.position).magnitude;

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

        public bool IsRunable() {

            float distance = (enemy.transform.position - GameController.GetPlayer.GetComponent<PlayerPhysical>().TargettingPos).magnitude;
            if (distance > max_distance ||
                distance < min_distance) { 
            
                return false;
            }
            return true;
        }

        public void Run() {

            pattern.IsMain = true;
            pattern.Run();
            enemy.GlobalDelay = post_delay;
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
            this.post_delay = pattern.PatternPostDelay;
        }
        public string GetPatternName() {

            return pattern.ToString();
        }
    }

    //Container for pattern
    public List<PatternController> PatternList;
    public Queue<PatternController> PatternQueue = new Queue<PatternController>();

    protected override void Update()
    {

        base.Update();
        if (!PatternRunning) {
            GlobalDelay -= Time.deltaTime;
            //call Run() method which is pattern in queue
            if (PatternQueue.Count > 0&&GlobalDelay<0)
            {
                PatternController controller = PatternQueue.Dequeue();
                if (controller.IsRunable())
                {
                    controller.Run();
                }
                else {

                    PatternQueue.Enqueue(controller);
                }
                
            }
        }

        foreach (PatternController patternController in PatternList)
        {

            patternController.Tick();
        }

    }

    private void Awake()
    {
        PatternList= new List<PatternController>();
        foreach (PatternDefault pattern in gameObject.GetComponents<PatternDefault>()) {

            if (pattern.enabled) {

                pattern.Caster = this;
                if (!pattern.IsIndependentPattern) {

                    continue;
                }
                
                PatternController PC = new(pattern);
                PatternList.Add(PC);
            }
        }
        Statement = "normal";
        GlobalDelay = 0;
        gameObject.transform.position = DefaultPos;
    }


    protected virtual void Start()
    {
        foreach (PatternController pattern in PatternList) {
            
            pattern.PatternReset();
        }
        PatternRunning = false;
    }
}
