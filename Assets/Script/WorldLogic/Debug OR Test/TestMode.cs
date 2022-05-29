using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestMode : MonoBehaviour
{

    public TextMeshProUGUI SkillText, CharaText;

    private string text;

    // Update is called once per frame
    void Update()
    {
        //스킬 토글
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (GameController.GetEnemy().GetComponent<EnemyDefault>().PatternList.Count > 0)
            {
                GameController.GetEnemy().GetComponent<EnemyDefault>().PatternList[0].Is_Enabled ^= true;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {

            if (GameController.GetEnemy().GetComponent<EnemyDefault>().PatternList.Count > 1)
            {
                GameController.GetEnemy().GetComponent<EnemyDefault>().PatternList[1].Is_Enabled ^= true;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (GameController.GetEnemy().GetComponent<EnemyDefault>().PatternList.Count > 2)
            {
                GameController.GetEnemy().GetComponent<EnemyDefault>().PatternList[2].Is_Enabled ^= true;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {

            if (GameController.GetEnemy().GetComponent<EnemyDefault>().PatternList.Count > 3)
            {
                GameController.GetEnemy().GetComponent<EnemyDefault>().PatternList[3].Is_Enabled ^= true;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (GameController.GetEnemy().GetComponent<EnemyDefault>().PatternList.Count > 4)
            {
                GameController.GetEnemy().GetComponent<EnemyDefault>().PatternList[4].Is_Enabled ^= true;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {

            if (GameController.GetEnemy().GetComponent<EnemyDefault>().PatternList.Count > 5)
            {
                GameController.GetEnemy().GetComponent<EnemyDefault>().PatternList[5].Is_Enabled ^= true;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            if (GameController.GetEnemy().GetComponent<EnemyDefault>().PatternList.Count > 6)
            {
                GameController.GetEnemy().GetComponent<EnemyDefault>().PatternList[6].Is_Enabled ^= true;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {

            if (GameController.GetEnemy().GetComponent<EnemyDefault>().PatternList.Count > 7)
            {
                GameController.GetEnemy().GetComponent<EnemyDefault>().PatternList[7].Is_Enabled ^= true;
            }
        }

        //캐릭터 변경
        if (Input.GetKeyDown(KeyCode.F1))
        {

            if (GameController.EnemyList.Count > 0 && GameController.GetEnemy() != GameController.EnemyList[0])
            {

                GameController.GetEnemy().SetActive(false);
                GameController.SetEnemy(GameController.EnemyList[0]);
                GameController.GetEnemy().SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {

            if (GameController.EnemyList.Count > 1 && GameController.GetEnemy() != GameController.EnemyList[0])
            {

                GameController.GetEnemy().SetActive(false);
                GameController.SetEnemy(GameController.EnemyList[1]);
                GameController.GetEnemy().SetActive(true);
            }
        }


        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {

            GameController.Level++;
        }
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {

            if (GameController.Level > 0)
            {

                GameController.Level--;
            }
        }


        //디버깅용 텍스트
        text = "";
        if (GameController.GetEnemy().GetComponent<EnemyDefault>() != null)
        {
            foreach (EnemyDefault.PatternController pc in GameController.GetEnemy().GetComponent<EnemyDefault>().PatternList)
            {

                text += pc.GetPatternName() + ":" + (pc.Is_Enabled ? "O" : "X") + "\n";
            }
        }
        text += "Level: " + GameController.Level;
        SkillText.text = text;

        text = "";
        foreach (GameObject chara in GameController.EnemyList)
        {

            if (GameController.GetEnemy().name == chara.name)
            {

                text += "->";
            }
            text += chara.name + "\n";
        }
        CharaText.text = text;

    }
}
