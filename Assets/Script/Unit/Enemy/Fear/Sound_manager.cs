using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class Sound_manager : MonoBehaviour
{
    public Object sound_path;

    public List<Sound_Class> sounds;

    // Start is called before the first frame update
    void Start()
    {
        string path = AssetDatabase.GetAssetPath(sound_path);
        sounds = load_audio(path);
    }

    private List<Sound_Class> load_audio(string obj_path)
    {
        string[] folders = Directory.GetFiles(obj_path);
        List<Sound_Class> list = new();

        foreach (string folder in folders)
        {
            int len = folder.Length - 4;
            if (!Equals(folder[len..], "meta"))
            {
                if (Equals(AssetDatabase.GetMainAssetTypeAtPath(folder).Name, "DefaultAsset"))
                {

                }
                else if (Equals(AssetDatabase.GetMainAssetTypeAtPath(folder).Name, "AudioClip"))
                {
                    string temp_path = folder.Replace("Assets/Resources/", "").Split(".")[0].Replace("\\", "/");
                    string[] split_path = temp_path.Split("/");
                    //Sound_Class clip = new Sound_Class(split_path[split_path.Length - 1], Resources.Load<AudioClip>(temp_path)); ;
                    Sound_Class clip = gameObject.AddComponent<Sound_Class>();
                    clip.Init(split_path[split_path.Length - 1], Resources.Load<AudioClip>(temp_path));
                    list.Add(clip);
                }
            }
        }
        return list;
    }

    public bool Play(string sound_name)
    {
        bool result = false;
        foreach(Sound_Class temp_sound_class in sounds)
        {
            if(Equals(temp_sound_class.GetName(), sound_name))
            {
                temp_sound_class.play();
                result = true;
            }
        }
        return result;
    }
}
 