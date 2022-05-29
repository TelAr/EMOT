using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    
    public GameObject PlayerModel;
    public List<GameObject> EnemyModel;
    public Canvas OptionWindow;

    private string text;
    static public int Level;
    static public List<GameObject> EnemyList = new(); 
    static private GameObject player;
    static private GameObject enemy;
    static public float GRAVITY = -35;
    static public bool is_stop;

    void Awake()
    {
        Level = 0;
        is_stop = false;
        foreach (GameObject enemy in EnemyModel) { 
        
            GameObject input = Instantiate(enemy);
            input.SetActive(false);
            EnemyList.Add(input);
        }

        player = Instantiate(PlayerModel);
        enemy = EnemyList[0];
        enemy.SetActive(true);
    }

    static public GameObject GetPlayer() {

        return player;
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


        if (Input.GetKeyDown(KeyCode.P)) {

            is_stop ^= true;
            if (is_stop)
            {

                gameObject.GetComponent<OptionSetting>().IsOpen();
            }
        }

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
