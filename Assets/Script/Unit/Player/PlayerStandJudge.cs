using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStandJudge : MonoBehaviour
{

    public bool GetStuckState
    {
        get { return StuckCount!=0; }
    }

    [SerializeField]
    private int StuckCount = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Block") && collision.GetComponent<PlatformEffector2D>() == null)  {

            StuckCount++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Block") && collision.GetComponent<PlatformEffector2D>() == null)
        {

            StuckCount--;
        }
    }

}
