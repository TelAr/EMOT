using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionSetting : MonoBehaviour
{


    public Toggle FullScreen;
    public TMP_Dropdown ResolutionDropDown;
    private int savedGraphicNum;
    private bool savedGraphicToggle;

    public Slider Master, Effect, BGM;
    public Toggle MasterMT, EffectMT, BGMMT;
    private float savedMS, savedES, savedBS;
    private bool savedMMT, savedEMT, savedBMT;

    private int Width, Height;
    private int GraphicLevel;
    private bool IsFullScreen;

    private void Start()
    {
        ReadValue();
        IsOpen();
        SaveOption();
    }

    private void SaveValue() {

        savedGraphicNum = ResolutionDropDown.value;
        savedGraphicToggle = FullScreen.isOn;

        Master.value = savedMS = AudioDefault.MasterVolume;
        Effect.value = savedES = AudioDefault.EffectVolume;
        BGM.value = savedBS = AudioDefault.BGMVolume;
        savedMMT = MasterMT.isOn;
        savedEMT = EffectMT.isOn;
        savedBMT = BGMMT.isOn;

        //레지스트리에 저장
        PlayerPrefs.SetInt("savedGraphicNum", savedGraphicNum);
        PlayerPrefs.SetInt("savedGraphicToggle", savedGraphicToggle ? 1 : 0);
        PlayerPrefs.SetFloat("savedMS", savedMS);
        PlayerPrefs.SetFloat("savedES", savedES);
        PlayerPrefs.SetFloat("savedBS", savedBS);
        PlayerPrefs.SetInt("savedMMT", savedMMT ? 1 : 0);
        PlayerPrefs.SetInt("savedEMT", savedEMT ? 1 : 0);
        PlayerPrefs.SetInt("savedBMT", savedBMT ? 1 : 0);
    }

    private void ReadValue() {

        if (PlayerPrefs.HasKey("savedGraphicNum")) savedGraphicNum = PlayerPrefs.GetInt("savedGraphicNum");
        else savedGraphicNum = 0;

        if (PlayerPrefs.HasKey("savedGraphicToggle")) savedGraphicToggle = PlayerPrefs.GetInt("savedGraphicToggle") == 1;
        else savedGraphicToggle = false;

        if (PlayerPrefs.HasKey("savedMS")) savedMS = PlayerPrefs.GetFloat("savedMS");
        else savedMS = 1;

        if (PlayerPrefs.HasKey("savedES")) savedES = PlayerPrefs.GetFloat("savedES");
        else savedES = 1;

        if (PlayerPrefs.HasKey("savedBS")) savedBS = PlayerPrefs.GetFloat("savedBS");
        else savedBS = 1;

        if (PlayerPrefs.HasKey("savedMMT")) savedMMT = PlayerPrefs.GetInt("savedMMT") == 1;
        else savedMMT = true;

        if (PlayerPrefs.HasKey("savedEMT")) savedEMT = PlayerPrefs.GetInt("savedEMT") == 1;
        else savedEMT = true;

        if (PlayerPrefs.HasKey("savedBMT")) savedBMT = PlayerPrefs.GetInt("savedBMT") == 1;
        else savedBMT = true;
    }

    public void IsOpen() {

        ResolutionDropDown.value = savedGraphicNum;
        FullScreen.isOn = savedGraphicToggle;

        Master.value = savedMS;
        Effect.value = savedES;
        BGM.value = savedBS;
        MasterMT.isOn = savedMMT;
        EffectMT.isOn = savedEMT;
        BGMMT.isOn = savedBMT;
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
            case 2:
                Width = 1920*2;
                Height = 1080*2;
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
        Update();

        ResolutionFixed.SetWidth=Width;
        ResolutionFixed.SetHeight=Height;
        ResolutionFixed.IsFullScreen=IsFullScreen;
        ResolutionFixed.FixedGameResolution();

        AudioDefault.MasterVolume = Master.value;
        AudioDefault.EffectVolume = Effect.value;
        AudioDefault.BGMVolume = BGM.value;


        SaveValue();

        //음소거 버튼은 ON일때가 정상 재생, off일때가 음소거
        if (!MasterMT.isOn)
        {

            AudioDefault.MasterVolume = 0;
        }
        if (!EffectMT.isOn)
        {

            AudioDefault.EffectVolume = 0;
        }
        if (!BGMMT.isOn)
        {

            AudioDefault.BGMVolume = 0;
        }

    }

}
