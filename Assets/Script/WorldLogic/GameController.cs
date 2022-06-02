using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    
    public GameObject PlayerModel;
    public List<GameObject> EnemyModel;
    public Canvas OptionWindow, MenuWindow;
    public Slider HealthGraph, StaminaGraph;
    public List<Vector2> GlobalLimitArea;

    static public int Level;
    static public List<GameObject> EnemyList = new();
    static private GameObject gameConroller = null;
    static private GameObject player;
    static private GameObject enemy;
    static public float GRAVITY = -35;

    void Awake()
    {
        gameConroller = gameObject;
        Level = 0;
        foreach (GameObject enemy in EnemyModel) { 
        
            GameObject input = Instantiate(enemy);
            input.SetActive(false);
            EnemyList.Add(input);
        }

        OptionWindow.gameObject.SetActive(false);
        MenuWindow.gameObject.SetActive(false);

        player = Instantiate(PlayerModel);
        enemy = EnemyList[0];
        enemy.SetActive(true);
    }

    static public GameObject GetPlayer() {

        return player;
    }

    static public GameObject GetGameController() {

        return gameConroller;
    }
    static public GameObject GetEnemy() { 
    
        return enemy;
    }

    static public void SetEnemy(GameObject Enemy) {

        enemy = Enemy;
    }
    // Update is called once per frame
    void Update()
    {

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

        if (Input.GetKeyDown(KeyCode.Escape)) {

            if (!MenuWindow.gameObject.activeSelf)
            {
                OpenMenu();
            }
            else {

                ClearUI();
            }
        }


        if ((OptionWindow.gameObject.activeSelf || MenuWindow.gameObject.activeSelf))
        {
            Time.timeScale = 0;
        }
        else { 
        
            Time.timeScale = 1;
        }

    }

    public void ClearUI() {

        OptionWindow.gameObject.SetActive(false);
        MenuWindow.gameObject.SetActive(false);
    }

    public void OpenMenu() {

        OptionWindow.gameObject.SetActive(false);
        MenuWindow.gameObject.SetActive(true);
    }

    public void OpenOption() {

        gameObject.GetComponent<OptionSetting>().IsOpen();
        OptionWindow.gameObject.SetActive(true);
        MenuWindow.gameObject.SetActive(false);
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
