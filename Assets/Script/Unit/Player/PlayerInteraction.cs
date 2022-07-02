using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInteraction : MonoBehaviour
{
    private HashSet<GameObject> InteractionableUnits=new HashSet<GameObject>();
    private GameObject nowInteraction = null;

    public void OnInteraction(InputValue value) {

        if (value.isPressed)
        {
            if (nowInteraction == null) {
                
                foreach (var unit in InteractionableUnits)
                {
                    if (unit.GetComponent<InteractionableUnit>().IsInteractive) {
                        nowInteraction = unit;
                        break;
                    }
                }
            }

            if (nowInteraction != null) {

                nowInteraction.GetComponent<InteractionableUnit>().GetInteractiveDown();
            }
        }
        else
        {
            if (nowInteraction != null)
            {

                nowInteraction.GetComponent<InteractionableUnit>().GetInteractiveUp();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<InteractionableUnit>() != null
            && !InteractionableUnits.Contains(collision.gameObject)) {
            InteractionableUnits.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (InteractionableUnits.Contains(collision.gameObject))
        {

            InteractionableUnits.Remove(collision.gameObject);
        }
    }
}
