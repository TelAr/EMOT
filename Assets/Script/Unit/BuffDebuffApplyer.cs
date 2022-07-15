using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffDebuffApplyer : MonoBehaviour
{
    private HashSet<BuffDebuff> buffDebuffs=new();


    public void AddBuffDebuff(BuffDebuff input) {

        input.Attached = this.gameObject;
        buffDebuffs.Add(input);
    }

    // Update is called once per frame
    void LateUpdate()
    {

        List<BuffDebuff> destoryList = new List<BuffDebuff>();

        foreach (BuffDebuff bd in buffDebuffs) {
            if (!bd.Tick()) {

                destoryList.Add(bd);
            }
        }

        foreach(BuffDebuff bd in destoryList) 
        {

            buffDebuffs.Remove(bd);
        }

    }
}
