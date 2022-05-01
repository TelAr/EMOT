using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitDefault : MonoBehaviour
{
    public float MinX, MinY, MaxX, MaxY;
    public Vector3 DefaultPos;

    protected bool is_fall;
    public virtual void Reset()
    {
        MinX = -40;
        MinY = -40;
        MaxX = 40;
        MaxY = 40;
        DefaultPos = Vector3.zero;
    }

    public void ReAwake() {

        if (!this.gameObject.activeSelf) {
            this.gameObject.SetActive(true);
            //시작 시 활동
            Start();
        }
        is_fall = true;


    }
    public abstract void Start();

    public void Sleep() { 
    

        //종료 시 활동

        this.gameObject.SetActive(false);
    }

    virtual public void Update() {

        if (transform.position.x < MinX || transform.position.x > MaxX || transform.position.y < MinY || transform.position.y > MaxY)
        {
            transform.position = DefaultPos;
            if (gameObject.GetComponent<Rigidbody2D>() != null) {

                gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
            is_fall = true;
        }
    }

}
