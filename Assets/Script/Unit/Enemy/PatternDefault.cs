using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PatternDefault : MonoBehaviour
{

    public float Cooldown = 0;
    public int Stack = 0;
    public float MaxDistance, MinDistance;
    public float PatternPreDelay=0;
    public EnemyDefault Caster = null;
    public bool IsRun = false;
    public bool IsMain = true;
    public bool IsIndependentPattern = true;

    virtual public void Run() {
        Setting();
        IsRun = true;
        if (IsMain) {
            Caster.PatternRunning = true;
        }
    }


    virtual public void Stop() {

        IsRun = false;

        if (IsMain) {
            Caster.PatternRunning = false;
            Caster.Statement = "normal";
        }

    }
    //해당 함수는 패턴 호출 시 맨 처음에 패턴 클래스를 초기값으로 세팅해줌
    abstract public void Setting();
    
}
