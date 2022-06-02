using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitDefault : MonoBehaviour
{
    public float MinX, MinY, MaxX, MaxY;
    public bool IsUniqueLimit = false;
    public Vector3 DefaultPos;

    protected bool isFall;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Block")) {

            isFall = false;
        }

    }


    public virtual void Reset()
    {
        MinX = -40;
        MinY = -40;
        MaxX = 40;
        MaxY = 40;
        DefaultPos = Vector3.zero;
    }

    protected virtual void Awake()
    {
        if (!IsUniqueLimit && GameController.GetGameController() != null) {

            MinX = GameController.GetGameController().GetComponent<GameController>().GlobalLimitArea[0].x;
            MinY = GameController.GetGameController().GetComponent<GameController>().GlobalLimitArea[0].y;
            MaxX = GameController.GetGameController().GetComponent<GameController>().GlobalLimitArea[1].x;
            MaxY = GameController.GetGameController().GetComponent<GameController>().GlobalLimitArea[1].y;
        }
    }

    public abstract void Start();

    public void Sleep() { 
    

        //���� �� Ȱ��

        this.gameObject.SetActive(false);
    }

    virtual public void Update() {

        if (transform.position.x < MinX || transform.position.x > MaxX || transform.position.y < MinY || transform.position.y > MaxY)
        {
            transform.position = DefaultPos;
            if (gameObject.GetComponent<Rigidbody2D>() != null) {

                gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
            isFall = true;
        }
    }

    public bool GetFall() { 
    
        return isFall;
    }

    public void SetNotFall()
    {
        isFall = false;
    }
}
