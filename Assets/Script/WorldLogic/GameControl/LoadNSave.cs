using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadNSave : MonoBehaviour
{
    public LoadNSave loadNSave = null;

    private string ItemPath = Application.dataPath + "/item.json";
    private string PlayerPath = Application.dataPath + "/player.json";


    private void Awake()
    {
        if (loadNSave != null) { 
        
            DestroyImmediate(this);
            return;
        }
        loadNSave = this;
    }

    public void Save() { 
    
        string PlayerInfo=GameController.GetPlayer().GetComponent<PlayerHealth>().GetJsonData();
        
        PlayerInfo+= "\n"+GameController.GetPlayer().GetComponent<PlayerPhysical>().GetJsonData();

        File.WriteAllText(PlayerPath, PlayerInfo);

    }
}
