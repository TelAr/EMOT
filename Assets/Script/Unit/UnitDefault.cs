using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitDefault : MonoBehaviour
{
    public void ReAwake() {

        if (!this.gameObject.activeSelf) {
            this.gameObject.SetActive(true);
            //���� �� Ȱ��
            Awake();
        }
        
    }

    public abstract void Awake();

    public void Sleep() { 
    

        //���� �� Ȱ��

        this.gameObject.SetActive(false);
    }

}
