using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class WarningObject : MonoBehaviour
{
    [InfoBox("DefaultTrancparency is 0~1. 0 is perfect Trancparency, and 1 is not Trancparency\n" +
        "Blink cycle is COS function, so if you want to set end parts disapeard is continuously then set valye n.5.", EInfoBoxType.Normal)]
    [Range(0,1)]
    public float DefaultTrancparency;
    public float SolidTime, BlinkTime;
    public float BlinkCount;
    public Color color;

    private float timer;

    private void OnEnable()
    {
        timer = 0;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer < SolidTime)
        {

            gameObject.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, DefaultTrancparency);
        }
        else if(timer <SolidTime+BlinkTime) {

            float radian = (BlinkCount) * (timer - SolidTime) / BlinkTime;

            gameObject.GetComponent<SpriteRenderer>().color = 
                new Color(color.r, color.g, color.b, DefaultTrancparency * 0.5f*Mathf.Cos(Mathf.PI*2* radian) + DefaultTrancparency*0.5f);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
