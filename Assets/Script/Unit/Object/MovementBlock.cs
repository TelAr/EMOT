using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBlock : MonoBehaviour
{
    public List<Vector3> points = new List<Vector3>();
    public float speed;
    public bool IsPlayerMoveTogether = false;

    private int pointer;
    private bool reverse = false;
    private float timer;
    private float ratio;
    private void OnEnable()
    {
        reverse = false;
        pointer = 0;
        timer = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && IsPlayerMoveTogether)
        {

            collision.collider.transform.SetParent(gameObject.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.transform.SetParent(null);
        }
    }


    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        ratio = (speed/(points[pointer] - points[pointer + (reverse ? -1 : 1)]).magnitude)*timer;
        if (ratio < 1)
        {
            transform.position = points[pointer] * (1 - ratio) + points[pointer + (reverse ? -1 : 1)] * ratio;
        }
        else {

            if (reverse)
            {

                pointer--;
            }
            else { 
            
                pointer++;
            }

            if (pointer == 0)
            {

                reverse = false;
            }
            else if (pointer == points.Count - 1) { 
            
                reverse = true;
            }
            timer = 0;
        }
        
    }
}
