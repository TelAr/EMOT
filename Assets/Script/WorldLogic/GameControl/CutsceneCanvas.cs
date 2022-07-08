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
    public GameObject LogPanel;
    public ScrollRect LogScrollRect;
    public TextMeshProUGUI LogTMP;
    public float AutoTime = 3f;
    private void Awake()
    {
        
        if (instance != null) { 
        
            Destroy(instance);
            return;
        }
        instance = this;
        LogPanel.SetActive(false);
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
