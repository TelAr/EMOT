using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WarpNextScene : FieldTriggerDefault
{
    public string SceneName;
    public Vector3 WarpScenePosition;
    public override void Trigger() {

        if (SceneManager.GetSceneByName(SceneName) != null)
        {
            SceneManager.LoadSceneAsync(SceneName);
        }
        else {
            //����ȭ�� �� ��ȯ
            Debug.LogError("Scene Change Fail");
        }
    }
 
}
