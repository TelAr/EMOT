using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimePauseObject : MonoBehaviour
{
    private void OnEnable()
    {
        if (GameController.GetGameController != null) {
            GameController.GetGameController.TimeStopStack(true);
        }
    }

    private void OnDisable()
    {

        if (GameController.GetGameController != null)
        {
            GameController.GetGameController.TimeStopStack(false);
        }
    }
}
