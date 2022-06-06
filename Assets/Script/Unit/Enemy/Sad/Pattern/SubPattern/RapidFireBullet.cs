using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RapidFireBullet : MonoBehaviour
{

    public float FlyTimer;
    public float BoomTimer;
    public Vector3 EndPos;


    public float timer;
    private Vector3 startPos;


    private void OnEnable()
    {
        startPos = transform.position;
        timer = 0;
    }


    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;


        if (timer < FlyTimer)
        {

            float x = Mathf.Pow(timer / FlyTimer, 2);
            transform.position = startPos * (1 - x) + EndPos * x;
        }
        else if (timer < FlyTimer + BoomTimer)
        {
            //Æø¹ß ÀÌÆåÆ® Ãß°¡ ÇÊ¿ä

        }
        else {
            if (GetComponent<ImpactDefault>() != null)
            {
                GetComponent<ImpactDefault>().Impact();
            }
            gameObject.SetActive(false);
        }
    }
}
