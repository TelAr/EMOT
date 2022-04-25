using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject player_prefabs;
    static private GameObject player;
    // Start is called before the first frame update
    void Awake()
    {
        
        player = Instantiate(player_prefabs);
    }

    static public GameObject GetPlayer() {

        return player;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
