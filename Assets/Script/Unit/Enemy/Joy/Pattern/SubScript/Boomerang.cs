using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    public float timer = 0;
    public float oneWayTime;
    public GameObject casterObject;
    public Vector3 targetPos, initiatingPos;



    public void Initiating(GameObject caster, Vector3 target, float oneWayDelay, Vector3? offset = null)
    {
        Debug.Log("?");
        if (offset == null) { 
        
            offset = Vector3.zero;
        }

        timer = 0;
        oneWayTime = oneWayDelay;
        casterObject = caster;
        targetPos = target;
        gameObject.transform.position = initiatingPos = (Vector3)(caster.transform.position + offset);
    } 


    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        transform.rotation = Quaternion.Euler(0, 0, timer * 720);


        //
        

        if (timer < oneWayTime)
        {

            transform.position = initiatingPos * Mathf.Pow((timer - (oneWayTime)) / oneWayTime, 2)
                + targetPos * (1 - Mathf.Pow((timer - (oneWayTime)) / oneWayTime, 2));

        }
        else if (timer < oneWayTime * 2)
        {

            transform.position = targetPos * (1 - Mathf.Pow((timer - (oneWayTime)) / oneWayTime, 2))
            + casterObject.transform.position * Mathf.Pow((timer - (oneWayTime)) / oneWayTime, 2);
        }
        else{
        
            gameObject.SetActive(false);
        }

    }
}
