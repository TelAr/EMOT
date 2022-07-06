using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemDefault : MonoBehaviour
{
    public int ItemID;
    public bool IsUnique;
    protected virtual void OnTriggerEnter2D(Collider2D collision) {

        if (IsUnique) {

            ItemController.Instance.ItemGain(ItemID);
        }
    }
    protected void OnEnable()
    {
        //유니크한 아이템이고 이미 획득한 경우
        if (IsUnique && ItemController.Instance.IsGained(ItemID))
        {
            gameObject.SetActive(false);
        }
    }
}