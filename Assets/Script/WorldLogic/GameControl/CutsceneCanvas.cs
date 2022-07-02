using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CutsceneCanvas : MonoBehaviour
{
    static public CutsceneCanvas instance = null;
    public TextMeshProUGUI Speaker, contents;
    public RawImage Left, Right;
    private void Awake()
    {
        
        if (instance != null) { 
        
            Destroy(instance);
            return;
        }
        instance = this;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
