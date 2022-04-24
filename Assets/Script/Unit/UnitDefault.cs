using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitDefault : MonoBehaviour
{
    public void ReAwake() {

        if (!this.gameObject.activeSelf) {
            this.gameObject.SetActive(true);
            //시작 시 활동
            Awake();
        }
        
    }

    public abstract void Awake();

    public void Sleep() { 
    

        //종료 시 활동

        this.gameObject.SetActive(false);
    }

}
