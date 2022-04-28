using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileDefault : MonoBehaviour
{
    public bool is_player_panetrate;
    public float MinX, MinY, MaxX, MaxY;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < MinX || transform.position.x > MaxX || transform.position.y < MinY || transform.position.y > MaxY) { 
        
            gameObject.SetActive(false);
        }
    }
}
