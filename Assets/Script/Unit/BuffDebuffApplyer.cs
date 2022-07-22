using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffDebuffApplyer : MonoBehaviour
{
    private Dictionary<string, BuffDebuff> buffDebuffs = new();

    //    private HashSet<BuffDebuff> buffDebuffs = new();

    public void AddBuffDebuff(BuffDebuff input)
    {
        input.Attached = this.gameObject;
        if (buffDebuffs.ContainsKey(input.BuffName))
        {
            if (!buffDebuffs[input.BuffName].AddStack())
            {
                buffDebuffs[input.BuffName] = input;
            }
        }
        else
        {
            buffDebuffs.Add(input.BuffName, input);
        }
    }

    private void Update()
    {
        float speedRatio = 1,
            jumpRatio = 1;
        foreach (var input in buffDebuffs)
        {
            speedRatio *= 1 + input.Value.SpeedRatio;
            jumpRatio *= 1 + input.Value.JumpRatio;
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
        List<string> destoryList = new();

        foreach (var bd in buffDebuffs)
        {
            if (!bd.Value.Tick())
            {
                destoryList.Add(bd.Key);
            }
        }

        foreach (var bd in destoryList)
        {
            buffDebuffs.Remove(bd);
        }
    }
}
