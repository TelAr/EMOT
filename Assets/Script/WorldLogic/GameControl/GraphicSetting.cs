using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicSetting : MonoBehaviour
{

    static public int SetWidth, SetHeight;
    static public bool IsFullScreen;
    static public bool isGraphicEffect;
    static public int SetGraphicLevel;

    // Start is called before the first frame update
    void Start()
    {
        SetWidth = 1920;
        SetHeight = 1080;
        IsFullScreen = true;
        isGraphicEffect = true;
        GraphicSettingUpdate();
    }

    static public void GraphicSettingUpdate() {

        FixedGameResolution();
        if (GlobalVolume.GlobalVolumeObject != null) {
            GlobalVolume.GlobalVolumeObject.SetActive(isGraphicEffect);
        }
        

    }

    static public void FixedGameResolution()
    {

        int deviceWidth = Screen.width; // 기기 너비 저장
        int deviceHeight = Screen.height; // 기기 높이 저장


        Screen.SetResolution(SetWidth, (int)(((float)deviceHeight / deviceWidth) * SetWidth), IsFullScreen);
        Screen.fullScreen = IsFullScreen;

        if ((float)SetWidth / SetHeight < (float)deviceWidth / deviceHeight) // 기기의 해상도 비가 더 큰 경우
        {
            float newWidth = ((float)SetWidth / SetHeight) / ((float)deviceWidth / deviceHeight); // 새로운 너비
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // 새로운 Rect 적용
        }
        else // 게임의 해상도 비가 더 큰 경우
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)SetWidth / SetHeight); // 새로운 높이
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // 새로운 Rect 적용
        }

    }


}
