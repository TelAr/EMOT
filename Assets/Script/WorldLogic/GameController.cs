using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject player_prefabs;
    private GameObject player;
    // Start is called before the first frame update
    void Awake()
    {
        
        player = Instantiate(player_prefabs);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
