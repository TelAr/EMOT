using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallFunctionOnTriggerEnter : MonoBehaviour
{
    public FieldFunctionDefault triggerDefault;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerPhysical>() != null)  {
            triggerDefault.Function();
        }

    }
}
