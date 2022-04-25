using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�� ��ü ���� �� �⺻������ �۵��Ǵ� ��Ŀ����
public class EnemyDefault : UnitDefault
{

    public bool pattern_running;
    //���� ������ ��ü
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

            if (timer >= cooldown) {
                this.timer += Time.deltaTime;
                if (stack > stackCounter) {

                    stackCounter++;
                    this.timer = 0;
                }
            }
            if (stack > 0|| !enemy.pattern_running) {

                pattern.Run();
                stackCounter--;
            }
        }
    }

    //���� ������ �����̳�
    public List<PatternController> patternList;

    public void Awake()
    {
        patternList= new List<PatternController>();
        foreach (PatternDefault pattern in gameObject.GetComponents<PatternDefault>()) {
            pattern.enemy = this;
            PatternController PC = new PatternController(pattern);
            patternList.Add(PC);
        }
    }

    public override void Start()
    {
        foreach (PatternController pattern in patternList) {

            pattern.PatternReset();
        }
        pattern_running = false;
    }
}
