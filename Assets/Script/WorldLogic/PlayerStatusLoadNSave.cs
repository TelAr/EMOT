using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusLoadNSave : MonoBehaviour
{
    //for scene change
    public void SaveStatus() { 
    
        PlayerPrefs.SetInt("PlayerHP", GameController.GetPlayer().GetComponent<PlayerHealth>().GetHealth());
        PlayerPrefs.SetInt("PlayerMAXHP", GameController.GetPlayer().GetComponent<PlayerHealth>().HealthMax);
        PlayerPrefs.SetInt("PlayerMaxStemina", GameController.GetPlayer().GetComponent<PlayerAction>().StaminaMax);
        PlayerPrefs.SetInt("PlayerStemina", GameController.GetPlayer().GetComponent<PlayerAction>().GetStemina());
    }

    public void LoadStatus() {

        GameObject player=GameController.GetPlayer();

        player.GetComponent<PlayerHealth>().SetMaxHealth(PlayerPrefs.GetInt("PlayerMAXHP"));
        player.GetComponent<PlayerHealth>().SetHealth(PlayerPrefs.GetInt("PlayerHP"));
        player.GetComponent<PlayerAction>().StaminaMax = PlayerPrefs.GetInt("PlayerMaxStemina");
        player.GetComponent<PlayerAction>().SetStemina(PlayerPrefs.GetInt("PlayerStemina"));

    }
}
