using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitDefault : MonoBehaviour
{
    public void ReAwake() {

        if (!this.gameObject.activeSelf) {
            this.gameObject.SetActive(true);
            //���� �� Ȱ��
            Start();
        }
        
    }
    public abstract void Start();

    public void Sleep() { 
    

        //���� �� Ȱ��

        this.gameObject.SetActive(false);
    }

}
