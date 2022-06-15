using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadNSave
{
    static private LoadNSave instance = null;

    static public LoadNSave Instance
    {

        get {

            if (instance == null) {

                instance = new LoadNSave();
            }
            return instance;
        }
    }

    private string SavePath = Application.dataPath + "/Save.json";

    struct DataSet {

        public string HealthInfo;
        public string PhysicalInfo;
        public string ItemGainedInfo;
        public string UniqueInfo;
        public string PlayerStateInfo;
    }


    public void Save() {

        DataSet dataSet = new DataSet();

        dataSet.HealthInfo = GameController.GetPlayer().GetComponent<PlayerHealth>().GetJsonData();
        dataSet.PhysicalInfo = GameController.GetPlayer().GetComponent<PlayerPhysical>().GetJsonData();
        dataSet.ItemGainedInfo = ItemController.Instance.GetListToString();

        File.WriteAllText(SavePath, JsonUtility.ToJson(dataSet));

    }

    public void Load() {

        DataSet dataSet = JsonUtility.FromJson<DataSet>(File.ReadAllText(SavePath));

        GameController.GetPlayer().GetComponent<PlayerHealth>().SetJsonData(dataSet.HealthInfo);
        GameController.GetPlayer().GetComponent<PlayerPhysical>().SetJsonData(dataSet.HealthInfo);
        ItemController.Instance.SetStringToList(dataSet.ItemGainedInfo);
    }
}
