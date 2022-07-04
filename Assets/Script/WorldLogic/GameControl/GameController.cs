using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

/*
 * Pattern type: singleton
 * Main Controller of game
 * initiation player, enemy, and others
 * Also, set Level or Gravity
 */
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

    [SerializeField]
    private int TimeStopCounter;
    private float TimeScale = 1f;
    private int eventCounter = 0;

    void Awake()
    {
        if (gameConroller != null) {
            DestroyImmediate(gameObject);
            return;
        }
        gameConroller = this;
        Level = 0;

        foreach (GameObject enemy in EnemyModel) { //차후 로직 변경 필요 가능성 매우 높음
        
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

        TimeStopCounter = 0;
        eventCounter = 0;
        DontDestroyOnLoad(gameObject);
    }

    static public GameObject GetPlayer {

        get {
            return player;
        }
        
    }

    static public GameController GetGameController {

        get {
            return gameConroller;
        }
    }

    public bool IsEvent
    {
        get { 
        
            return eventCounter > 0;
        }
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

    }

    private void TimeSetting(float time) {

        Time.timeScale = time;
    }

    public void EventSituation(bool IsBegin) {

        if (IsBegin)
        {
            eventCounter++;
            TimeSetting(0);

        }
        else
        {
            eventCounter--;
            if (TimeStopCounter <= 0&& eventCounter<=0)
            {

                TimeSetting(TimeScale);
            }
        }
    }

    //to using option
    public void TimeStopStack(bool IsStop) {

        if (IsStop)
        {
            TimeStopCounter++;
            TimeSetting(0);
            if (player != null)
            {
                player.GetComponent<PlayerAction>().IsLimited = true;
            }
        }
        else { 
        
            TimeStopCounter--;
            if (TimeStopCounter <= 0) {

                if (TimeStopCounter <= 0 && eventCounter <= 0) {
                    TimeSetting(TimeScale);
                }
                if (player != null) {
                    player.GetComponent<PlayerAction>().IsLimited = false;
                }
            }
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
