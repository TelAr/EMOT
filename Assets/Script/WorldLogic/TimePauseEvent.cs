using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimePauseEvent : MonoBehaviour
{
    private void OnEnable()
    {
        if (GameController.GetGameController != null)
        {
            GameController.GetGameController.EventSituation(true);
        }
    }

    private void OnDisable()
    {

        if (GameController.GetGameController != null)
        {
            GameController.GetGameController.EventSituation(false);
        }
    }
}
