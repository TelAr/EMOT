using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Pattern Type: Obsever
 * This code is class which is dependenced by object that inherited EnemyDefault.
 * When PatternDefault is attached by Gameobject which has Class object that inherited EnemyDefault,
 * PatternDefault is added by patternList on EnemyDefault, and Run method which is in PatternDefault is called by EnemyDefault.
*/


public abstract class PatternDefault : MonoBehaviour
{
    [Header("* Pattern Default Value")]
    public float Cooldown = 0;
    [Tooltip("Intiger that this pattern can stack maximum")]
    public int Stack = 1;
    [Tooltip("Distance that Pattern is trigger")]
    public float MaxDistance, MinDistance;
    [Tooltip("Delay that pattern is over. In this delay, other pattern waited")]
    public float PatternPostDelay=0;
    [HideInInspector]
    public EnemyDefault Caster = null;
    [HideInInspector]
    public bool IsRun = false;
    [HideInInspector]
    public bool IsMain = true;
    [Tooltip("If this flag is false, this pattern is not called single. Must call other way")]
    public bool IsIndependentPattern = true;

    //pattern run
    virtual public void Run() {
        Setting();
        IsRun = true;
        if (IsMain) {
            Caster.PatternRunning = true;
        }
    }


    //pattern stop
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
