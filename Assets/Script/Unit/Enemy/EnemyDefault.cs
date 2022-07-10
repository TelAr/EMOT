using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyDefault : UnitDefault
{
    public int SetMaxHealth = 100;
    public string Statement;
    public bool PatternRunning;
    public bool DefaultPhysicalForcedEnable;
    [Tooltip("Grobal Delay when Pattern is over")]
    public float GlobalDelay;
    [Tooltip("Front value: HP condition, Second value: Order'\n'" +
        "Pattern number start from 0\n")]
    public List<PatternFormulationContainer> PatternOrderList = new();

    private HealthDefault health = null;
    private int listPointer = 0;
    private int patternPointer = 0;

    [System.Serializable]
    public struct PatternFormulationContainer {

        public int MinimalHP;
        public List<int> PatternOrder;
    }

    //Class to controller pattern
    public class PatternController { 
    
        PatternDefault pattern;
        float max_distance = 0;
        float min_distance = 0;
        float post_delay = 0;
        public bool Is_Enabled;
        readonly EnemyDefault enemy;

        //init
        public PatternController(PatternDefault pattern)
        {
            this.pattern = pattern;
            this.enemy = pattern.Caster;
            this.max_distance = pattern.MaxDistance;
            this.min_distance = pattern.MinDistance;
            this.post_delay = pattern.PatternPostDelay;
            Is_Enabled = true;
        }

        public void PatternReset()
        {
            pattern.Setting();
        }

        public bool IsRunable {
            get
            {
                float distance = (enemy.transform.position - GameController.GetPlayer.GetComponent<PlayerPhysical>().TargettingPos).magnitude;
                if (distance > max_distance ||
                    distance < min_distance)
                {

                    return false;
                }
                return true;
            }
        }

        public void Run() {

            pattern.IsMain = true;
            pattern.Run();
            enemy.GlobalDelay = post_delay;
        }


        public void ForcedRun() {
            pattern.Run();
        }

        public void UpdatePatternInfo() {

            this.max_distance = pattern.MaxDistance;
            this.min_distance = pattern.MinDistance;
            this.post_delay = pattern.PatternPostDelay;
        }
        public string GetPatternName() {
            return pattern.ToString();
        }
    }

    //Container for pattern
    public List<PatternController> PatternControllerList;
    public Queue<PatternController> PatternQueue = new ();

    protected override void Update()
    {

        base.Update();
        if (!PatternRunning) {
            GlobalDelay -= Time.deltaTime;
            if (GlobalDelay > 0) {

                return;
            }

            if (PatternOrderList.Count == 0) {

                Debug.LogError("ERROR: NO PATTERN ORDER");
                return;
            }

            while (listPointer < PatternOrderList.Count && 
                PatternOrderList[listPointer].MinimalHP < health.GetHealth) { 
            
                listPointer++;
                patternPointer = 0;
            }
            if (listPointer >= PatternOrderList.Count) {

                listPointer = PatternOrderList.Count - 1;
            }

            if (PatternQueue.Count > 0 && GlobalDelay < 0)
            {
                PatternController controller = PatternQueue.Dequeue();
                if (controller.IsRunable)
                {
                    controller.Run();
                }
                else
                {

                    PatternQueue.Enqueue(controller);
                }
            }
            else {
                //under 0, OR over list size, set 0
                PatternControllerList[
                    PatternOrderList[listPointer].PatternOrder[patternPointer]< PatternControllerList.Count?
                    (PatternOrderList[listPointer].PatternOrder[patternPointer]<0?0: PatternOrderList[listPointer].PatternOrder[patternPointer]) : 0].Run();
                patternPointer++;
                if (patternPointer >= PatternOrderList[listPointer].PatternOrder.Count) { 
                
                    patternPointer = 0;
                }
            }
            
        }
    }

    private void Awake()
    {
        PatternControllerList= new List<PatternController>();
        foreach (PatternDefault pattern in gameObject.GetComponents<PatternDefault>()) {

            if (pattern.enabled) {

                pattern.Caster = this;
                if (!pattern.IsIndependentPattern) {

                    continue;
                }
                
                PatternController PC = new(pattern);
                PatternControllerList.Add(PC);
            }
        }
        Statement = "normal";
        GlobalDelay = 5;
        gameObject.transform.position = DefaultPos;

        health = gameObject.AddComponent<HealthDefault>();
        health.HealthMax = SetMaxHealth;
        health.FullHealth();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        listPointer = 0;
        patternPointer = 0;
    }

    protected virtual void Start()
    {
        foreach (PatternController pattern in PatternControllerList) {
            
            pattern.PatternReset();
        }
        PatternRunning = false;
    }
}
