using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject Player_prefabs;
    public List<GameObject> Enemy;
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
        
    }
}
