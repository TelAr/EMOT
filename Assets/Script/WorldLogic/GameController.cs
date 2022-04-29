using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public GameObject Player_prefabs;
    public List<GameObject> Enemy;
    public TextMeshProUGUI textMeshProUGUI;
    private string text;
    static private GameObject player;
    static private GameObject enemy;
    static public float GRAVITY = -20;
    // Start is called before the first frame update
    void Awake()
    {
        
        player = Instantiate(Player_prefabs);
        enemy = Instantiate(Enemy[0]);
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
        text = "";
        foreach (EnemyDefault.PatternController pc in enemy.GetComponent<EnemyDefault>().PatternList) {

            text += pc.GetPatternName() + ":" + (pc.Is_Enabled ? "O" : "X")+"\n";
        }
        textMeshProUGUI.text = text;




        if (Input.GetKeyDown(KeyCode.Escape)) {

            ExitGame();
        }

    }


    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // ���ø����̼� ����
#endif
    }

}
