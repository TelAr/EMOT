using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundJedge : MonoBehaviour
{

    public bool IsGround
    {
        get {

            return groundCounter > 0;
        }
    }

    private int groundCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        groundCounter = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Block")) groundCounter++;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Block")) groundCounter--;
    }

}
