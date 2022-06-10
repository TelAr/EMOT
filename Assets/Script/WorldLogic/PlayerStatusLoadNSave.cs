using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusLoadNSave : MonoBehaviour
{
    //for scene change
    static public int SaveStatus() {

        GameObject player = GameController.GetPlayer();
        if (player != null)
        {
            PlayerPrefs.SetInt("PlayerHP", GameController.GetPlayer().GetComponent<PlayerHealth>().GetHealth());
            PlayerPrefs.SetInt("PlayerMAXHP", GameController.GetPlayer().GetComponent<PlayerHealth>().HealthMax);
            PlayerPrefs.SetInt("PlayerMaxStemina", GameController.GetPlayer().GetComponent<PlayerAction>().StaminaMax);
            PlayerPrefs.SetInt("PlayerStemina", GameController.GetPlayer().GetComponent<PlayerAction>().GetStemina());

            PlayerPrefs.SetInt("IsStatusInstantSave", 1);
            return 0;
        }
        else {

            return -1;
        }
    }

    static public int LoadStatus() {

        GameObject player=GameController.GetPlayer();
        if (player != null && PlayerPrefs.GetInt("IsStatusInstantSave") == 1) 
        {

            player.GetComponent<PlayerHealth>().SetMaxHealth(PlayerPrefs.GetInt("PlayerMAXHP"));
            player.GetComponent<PlayerHealth>().SetHealth(PlayerPrefs.GetInt("PlayerHP"));
            player.GetComponent<PlayerAction>().StaminaMax = PlayerPrefs.GetInt("PlayerMaxStemina");
            player.GetComponent<PlayerAction>().SetStemina(PlayerPrefs.GetInt("PlayerStemina"));

            PlayerPrefs.SetInt("IsStatusInstantSave", 0);

            return 0;
        }
        else {

            return -1;
        }
    }
}
