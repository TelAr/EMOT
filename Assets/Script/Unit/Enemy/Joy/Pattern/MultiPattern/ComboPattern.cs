using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class ComboPattern : PatternDefault
{
    public List<PatternDefault> PatternList;
    public List<float> PreDelay;
    public float DefaultDelay;

    
    private int pointer;
    private bool is_going;
    private float timer;
    public override void Setting()
    {
        foreach (var pattern in PatternList) {
            pattern.IsMain = false;
        }
        pointer = 0;
        is_going = false;
    }

    public override void Run()
    {
        base.Run();
        string text="";
        foreach (var pattern in PatternList) {

            text+=pattern.ToString();
        }
        Debug.Log(text);   
        Caster.GetComponent<EnemyDefault>().statement = text;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsRun) {

            if (pointer >= PatternList.Count)
            {
                pointer = 0;
                Stop();
                return;
            }

            if (!is_going)
            {

                timer += Time.deltaTime;
                if (timer > (pointer >= PreDelay.Count ? DefaultDelay : PreDelay[pointer]))
                {
                    Debug.Log(pointer);
                    is_going = true;
                    PatternList[pointer].Run();
                }
            }
            else {

                if (!PatternList[pointer].IsRun)
                {
                    pointer++;
                    
                    is_going =false;
                    timer = 0;
                }
            }


        }
    }
}
