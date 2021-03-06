using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WarpNextScene : FieldFunctionDefault
{
    public string SceneName;
    public Vector3 WarpScenePosition;
    public override void Function() {

        if (SceneManager.GetSceneByName(SceneName) != null)
        {
            SceneManager.LoadSceneAsync(SceneName);
        }
        else {
            //오류화면 씬 전환
            Debug.LogError("Scene Change Fail");
        }
    }
 
}
