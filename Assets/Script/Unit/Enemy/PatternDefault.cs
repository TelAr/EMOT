using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    //�ش� �Լ��� ���� ȣ�� �� �� ó���� ���� Ŭ������ �ʱⰪ���� ��������
    abstract public void Setting();
    
}
