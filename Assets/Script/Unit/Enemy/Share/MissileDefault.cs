using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileDefault : MonoBehaviour
{
    public bool IsPlayerPanetrate;
    public bool IsCollsionDisappear;
    public bool IsDestroy;
    public float MinX, MinY, MaxX, MaxY;
    public ImpactDefault impact;

    public void Reset()
    {
        MinX = -40;
        MinY = -40;
        MaxX = 40;
        MaxY = 40;
        IsPlayerPanetrate = false;
        IsCollsionDisappear = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (impact != null)
        {

            impact.Impact(collision.gameObject);
        }
        if (IsCollsionDisappear) {
            if (IsDestroy) { 
            
                Destroy(gameObject);
            }
            gameObject.SetActive(false);
        }


    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (transform.position.x < MinX || transform.position.x > MaxX || transform.position.y < MinY || transform.position.y > MaxY) {

            if (IsDestroy) { 
            
                Destroy(gameObject);
            }
            gameObject.SetActive(false);
        }
    }
}