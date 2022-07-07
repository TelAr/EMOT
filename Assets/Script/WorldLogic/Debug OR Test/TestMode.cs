using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestMode : MonoBehaviour
{

    public TextMeshProUGUI SkillText, CharaText;
    public bool IsTest = true;

    static private GameObject enemy = null;

    private string text;

    public void Awake()
    {
        if (!IsTest) { 
        
            gameObject.SetActive(false);
            return;
        }
        if (GameController.EnemyList.Count > 0)
        {

            enemy = GameController.EnemyList[0];
        }
        else {

            return;
        }
        foreach (GameObject go in GameController.EnemyList)
        {
            go.SetActive(false);
        }
        if (enemy != null)
        {
            enemy.SetActive(true);
        }

    }

    // Update is called once per frame
    void Update()
    {

        //스킬 토글
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (enemy.GetComponent<EnemyDefault>().PatternList.Count > 0)
            {
                enemy.GetComponent<EnemyDefault>().PatternList[0].Is_Enabled ^= true;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {

            if (enemy.GetComponent<EnemyDefault>().PatternList.Count > 1)
            {
                enemy.GetComponent<EnemyDefault>().PatternList[1].Is_Enabled ^= true;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (enemy.GetComponent<EnemyDefault>().PatternList.Count > 2)
            {
                enemy.GetComponent<EnemyDefault>().PatternList[2].Is_Enabled ^= true;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {

            if (enemy.GetComponent<EnemyDefault>().PatternList.Count > 3)
            {
                enemy.GetComponent<EnemyDefault>().PatternList[3].Is_Enabled ^= true;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (enemy.GetComponent<EnemyDefault>().PatternList.Count > 4)
            {
                enemy.GetComponent<EnemyDefault>().PatternList[4].Is_Enabled ^= true;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {

            if (enemy.GetComponent<EnemyDefault>().PatternList.Count > 5)
            {
                enemy.GetComponent<EnemyDefault>().PatternList[5].Is_Enabled ^= true;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            if (enemy.GetComponent<EnemyDefault>().PatternList.Count > 6)
            {
                enemy.GetComponent<EnemyDefault>().PatternList[6].Is_Enabled ^= true;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {

            if (enemy.GetComponent<EnemyDefault>().PatternList.Count > 7)
            {
                enemy.GetComponent<EnemyDefault>().PatternList[7].Is_Enabled ^= true;
            }
        }

        //캐릭터 변경
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (GameController.EnemyList.Count > 0 && enemy != GameController.EnemyList[0])
            {

                enemy.SetActive(false);
                enemy = (GameController.EnemyList[0]);
                enemy.SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            if (GameController.EnemyList.Count > 1 && enemy != GameController.EnemyList[1])
            {
                
                enemy.SetActive(false);
                enemy = (GameController.EnemyList[1]);
                enemy.SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            if (GameController.EnemyList.Count > 2 && enemy != GameController.EnemyList[2])
            {

                enemy.SetActive(false);
                enemy = (GameController.EnemyList[2]);
                enemy.SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            if (GameController.EnemyList.Count > 3 && enemy != GameController.EnemyList[3])
            {

                enemy.SetActive(false);
                enemy = (GameController.EnemyList[3]);
                enemy.SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            if (GameController.EnemyList.Count > 4 && enemy != GameController.EnemyList[4])
            {

                enemy.SetActive(false);
                enemy = (GameController.EnemyList[4]);
                enemy.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.P)) {

            GameController.GetPlayer.GetComponent<PlayerHealth>().FullHealth();
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

        if (Input.GetKeyDown(KeyCode.Home)) {

            GameController.GetPlayer.GetComponent<PlayerHealth>().FullHealth();
        }


        //디버깅용 텍스트
        text = "";
        if (enemy != null)
        {
            foreach (EnemyDefault.PatternController pc in enemy.GetComponent<EnemyDefault>().PatternList)
            {

                text += pc.GetPatternName() + ":" + (pc.Is_Enabled ? "O" : "X") + "\n";
            }
        }
        text += "Level: " + GameController.Level;
        SkillText.text = text;

        text = "";
        foreach (GameObject chara in GameController.EnemyList)
        {

            if (enemy.name == chara.name)
            {

                text += "->";
            }
            text += chara.name + "\n";
        }
        text += Screen.width + ":"+Screen.height+", "+FPSShow.FPS;
        CharaText.text = text;

    }
}
