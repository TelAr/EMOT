using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    
    public GameObject PlayerModel;
    public List<GameObject> EnemyModel;
    public Canvas OptionWindow, MenuWindow;
    public Slider HealthGraph, StaminaGraph;
    public List<Vector2> GlobalLimitArea;
    public float GRAVITY = -35;

    static public int Level;
    static public List<GameObject> EnemyList = new();
    static private GameController gameConroller = null;
    static private GameObject player;


    void Awake()
    {
        if (gameConroller != null) {
            DestroyImmediate(gameObject);
            return;
        }
        gameConroller = this;
        Level = 0;
        foreach (GameObject enemy in EnemyModel) { 
        
            GameObject input = Instantiate(enemy);
            EnemyList.Add(input);
        }

        OptionWindow.gameObject.SetActive(false);
        MenuWindow.gameObject.SetActive(false);

        if (PlayerHealth.Player == null)
        {
            player = Instantiate(PlayerModel);
        }
        else {

            player = PlayerHealth.Player;
        }

        //for test
        if (gameObject.GetComponent<TestMode>() != null) {

            gameObject.GetComponent<TestMode>().Awake();
        }


        DontDestroyOnLoad(gameObject);
    }

    static public GameObject GetPlayer() {

        return player;
    }

    static public GameController GetGameController() {

        return gameConroller;
    }

    // Update is called once per frame
    void Update()
    {

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
