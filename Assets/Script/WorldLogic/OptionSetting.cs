using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionSetting : MonoBehaviour
{


    public Toggle FullScreen;
    public TMP_Dropdown ResolutionDropDown;

    private int Width, Height;
    private int GraphicLevel;
    private bool IsFullScreen;
    private float MasterVolume, EffectVolume, BGMVolume;

    private void Start()
    {
        Width = ResolutionFixed.SetWidth;
        Height = ResolutionFixed.SetHeight;
        IsFullScreen = ResolutionFixed.IsFullScreen;


    }

    void Update()
    {
        
        int ResolutionIndex = ResolutionDropDown.value;
        switch (ResolutionIndex) { 
        
            case 0:
                Width = 1280;
                Height = 720;
                break;
            case 1:
                Width = 1920;
                Height= 1080;
                break;
            default:
                break;
        }

        if (FullScreen != null) {
            IsFullScreen = FullScreen.isOn;
        }

    }


    

    public void SaveOption() {
        //resolution
        ResolutionFixed.SetWidth=Width;
        ResolutionFixed.SetHeight=Height;
        ResolutionFixed.IsFullScreen=IsFullScreen;
        ResolutionFixed.FixedGameResolution();
    }
}
