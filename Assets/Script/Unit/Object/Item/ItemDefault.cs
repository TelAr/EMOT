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
        //����ũ�� �������̰� �̹� ȹ���� ���
        if (IsUnique && ItemController.Instance.IsGained(ItemID))
        {
            gameObject.SetActive(false);
        }
    }
}