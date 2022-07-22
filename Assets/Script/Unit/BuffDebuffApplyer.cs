using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffDebuffApplyer : MonoBehaviour
{
    private HashSet<BuffDebuff> buffDebuffs = new();

    public HashSet<BuffDebuff> GetBuffDebuffs
    {
        get { return buffDebuffs; }
    }

    public void AddBuffDebuff(BuffDebuff input)
    {
        input.Attached = this.gameObject;
        buffDebuffs.Add(input);
    }

    private void Update()
    {
        float speedRatio = 1,
            jumpRatio = 1;
        foreach (BuffDebuff input in buffDebuffs)
        {
            speedRatio *= 1 + input.SpeedRatio;
            jumpRatio *= 1 + input.JumpRatio;
        }
        if (gameObject.GetComponent<UnitDefault>() != null)
        {
            gameObject.GetComponent<UnitDefault>().SetActualSpeed =
                speedRatio * gameObject.GetComponent<UnitDefault>().DefaultSettingSpeed;
            gameObject.GetComponent<UnitDefault>().SetActualJumpPower =
                jumpRatio * gameObject.GetComponent<UnitDefault>().DefaultSettingJumpPower;
        }
    }

    void LateUpdate()
    {
        List<BuffDebuff> destoryList = new List<BuffDebuff>();

        foreach (BuffDebuff bd in buffDebuffs)
        {
            if (!bd.Tick())
            {
                destoryList.Add(bd);
            }
        }

        foreach (BuffDebuff bd in destoryList)
        {
            buffDebuffs.Remove(bd);
        }
    }
}
