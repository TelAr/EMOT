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

        int deviceWidth = Screen.width; // ��� �ʺ� ����
        int deviceHeight = Screen.height; // ��� ���� ����


        Screen.SetResolution(SetWidth, (int)(((float)deviceHeight / deviceWidth) * SetWidth), IsFullScreen);
        Screen.fullScreen = IsFullScreen;

        if ((float)SetWidth / SetHeight < (float)deviceWidth / deviceHeight) // ����� �ػ� �� �� ū ���
        {
            float newWidth = ((float)SetWidth / SetHeight) / ((float)deviceWidth / deviceHeight); // ���ο� �ʺ�
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // ���ο� Rect ����
        }
        else // ������ �ػ� �� �� ū ���
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)SetWidth / SetHeight); // ���ο� ����
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // ���ο� Rect ����
        }

    }


}
