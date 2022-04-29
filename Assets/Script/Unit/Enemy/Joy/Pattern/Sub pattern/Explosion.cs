using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    float timer=0;
    Color color;
    public float RemainTime;
    public float FadeOutTime; 
    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = "Enemy";
        color=gameObject.GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {
        timer+=Time.deltaTime;
        if (timer > RemainTime) {

            gameObject.tag = "Untagged";
            gameObject.GetComponent<SpriteRenderer>().color=new Color(color.r,color.g,color.b,(FadeOutTime+RemainTime-timer)/FadeOutTime);
            if (FadeOutTime + RemainTime < timer) { 
            
                Destroy(gameObject);
            }
        }
    }
}
