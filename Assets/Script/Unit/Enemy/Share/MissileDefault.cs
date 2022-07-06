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
    public float MinimumJegdeLoseTime;

    private float timer;


    protected void OnDisable()
    {
        if (gameObject.GetComponent<Rigidbody2D>() != null) {

            gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }
    }

    public void Reset()
    {
        MinX = -40;
        MinY = -40;
        MaxX = 40;
        MaxY = 40;
        IsPlayerPanetrate = false;
        IsCollsionDisappear = true;
        MinimumJegdeLoseTime = 0.1f;
    }

    public void ResetTimer() {

        timer = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (timer > MinimumJegdeLoseTime) {
            if (impact != null)
            {

                impact.Impact(collision.gameObject);
            }
            if (collision.gameObject.GetComponent<HealthDefault>() != null)
            {


                if (!(IsPlayerPanetrate&&collision.gameObject.CompareTag("Player")))
                {
                    if (IsDestroy)
                    {
                        Destroy(gameObject);
                    }
                    gameObject.SetActive(false);

                }
                
            }
        

            if (IsCollsionDisappear)
            {
                if (IsDestroy)
                {

                    Destroy(gameObject);
                }
                gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        timer += Time.fixedDeltaTime;
        if (transform.position.x < MinX || transform.position.x > MaxX || transform.position.y < MinY || transform.position.y > MaxY) {

            if (IsDestroy) { 
            
                Destroy(gameObject);
            }
            gameObject.SetActive(false);
        }
    }
}
