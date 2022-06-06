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
            Caster.pattern_running = true;
        }
    }


    virtual public void Stop() {

        IsRun = false;

        if (IsMain) {
            Caster.pattern_running = false;
            Caster.statement = "normal";
        }

    }
    //�ش� �Լ��� ���� ȣ�� �� �� ó���� ���� Ŭ������ �ʱⰪ���� ��������
    abstract public void Setting();
    
}
