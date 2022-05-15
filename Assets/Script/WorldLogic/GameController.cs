using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    
    public GameObject Player_prefabs;
    public List<GameObject> EnemyModel;
    public TextMeshProUGUI SkillText, CharaText;
    public Canvas OptionWindow;

    private string text;
    static public int Level;
    static public List<GameObject> EnemyList = new(); 
    static private GameObject player;
    static private GameObject enemy;
    static public float GRAVITY = -20;
    static public bool is_stop;
    // Start is called before the first frame update
    void Awake()
    {
        Level = 0;
        is_stop = false;
        foreach (GameObject enemy in EnemyModel) { 
        
            GameObject input = Instantiate(enemy);
            input.SetActive(false);
            EnemyList.Add(input);
        }

        player = Instantiate(Player_prefabs);
        enemy = EnemyList[0];
        enemy.SetActive(true);
    }

    static public GameObject GetPlayer() {

        return player;
    }

    static public GameObject GetEnemy() { 
    
        return enemy;
    }
    // Update is called once per frame
    void Update()
    {
        //스킬 토글
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (enemy.GetComponent<EnemyDefault>().PatternList.Count > 0) {
                enemy.GetComponent<EnemyDefault>().PatternList[0].Is_Enabled ^= true;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {

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
        if (Input.GetKeyDown(KeyCode.F1)) {

            if (EnemyList.Count > 0&&enemy!=EnemyList[0]) {

                enemy.SetActive(false);
                enemy=EnemyList[0];
                enemy.SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            if (EnemyList.Count > 1 && enemy != EnemyList[1])
            {

                enemy.SetActive(false);
                enemy = EnemyList[1];
                enemy.SetActive(true);
            }
        }


        if (Input.GetKeyDown(KeyCode.KeypadPlus)) {

            Level++;
        }
        if (Input.GetKeyDown(KeyCode.KeypadMinus)) {

            if (Level > 0) { 
            
                Level--;
            }
        }
        if (Input.GetKeyDown(KeyCode.P)) {

            is_stop ^= true;
            if (is_stop)
            {

                gameObject.GetComponent<OptionSetting>().IsOpen();
            }
        }

        //디버깅용 텍스트
        text = "";
        if (enemy.GetComponent<EnemyDefault>() != null)
        {
            foreach (EnemyDefault.PatternController pc in enemy.GetComponent<EnemyDefault>().PatternList)
            {

                text += pc.GetPatternName() + ":" + (pc.Is_Enabled ? "O" : "X") + "\n";
            }
        }
        text += "Level: " + Level;
        SkillText.text = text;

        text = "";
        foreach (GameObject chara in EnemyList) {

            if (enemy.name == chara.name) {

                text+="->";
            }
            text += chara.name + "\n";
        }
        CharaText.text = text;

        if (Input.GetKeyDown(KeyCode.Escape)) {

            ExitGame();
        }


        OptionWindow.gameObject.SetActive(is_stop);
        if (is_stop)
        {

            Time.timeScale = 0;
        }
        else { 
        
            Time.timeScale = 1;
        }

    }

    public void Again() {

        is_stop = false;
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }

}
