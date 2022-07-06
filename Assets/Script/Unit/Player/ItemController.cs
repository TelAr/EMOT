using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ItemController
{
    private static ItemController itemController = null;

    public static ItemController Instance
    {

        get
        {

            if (itemController == null)
            {

                itemController = new ItemController();
            }
            return itemController;
        }
    }

    private HashSet<int> ItemGained = new();

    public bool IsGained(int ID) { 
    
        return ItemGained.Contains(ID);
    }

    public void ItemGain(int ID) {

        ItemGained.Add(ID);
    }

    public string GetListToString() {

        string output="";
        foreach (int i in ItemGained) {

            output += i + "/";
        }
        return output;
    }

    public void SetStringToList(string Input) {

        string[] words = Input.Split('/');

        foreach (string word in words) {
            int value;
            if (Int32.TryParse(word, out value))
            {
                ItemGain(Convert.ToInt32(word));
            }
            else {

                //EXCEPTION
                Debug.Log("ERROR: SAVE DATA HAS PROBLEM");
            }
        }

    }
}
