using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;

public class Sound_manager : MonoBehaviour
{
    public Object sound_path;

    public AudioClip[] sounds;

    // Start is called before the first frame update
    void Start()
    {
        string path = AssetDatabase.GetAssetPath(sound_path);
        sounds = load_audio(path);
    }


    private AudioClip[] load_audio(string obj_path)
    {
        Match match = Regex.Match(obj_path, "Assets/Resources/.*");
        if (match.Success)
        {
            obj_path = obj_path[17..];
        }
        GameObject soundObject = new GameObject("Sounds");
        soundObject.transform.parent = gameObject.transform;

        Sound_Class sound_Class = soundObject.AddComponent<Sound_Class>();
        sound_Class.Init(Resources.LoadAll<AudioClip>(obj_path));

        return Resources.LoadAll<AudioClip>(obj_path);
    }

    public bool Play(string sound_name)
    {
        bool result = false;

        GameObject soundObject = gameObject.transform.Find("Sounds").gameObject;

        Sound_Class.Sound_subClass[] sound_list = soundObject.GetComponents<Sound_Class.Sound_subClass>();
        foreach (Sound_Class.Sound_subClass sound in sound_list)
        {
            if(Equals(sound.Name, sound_name))
            {
                sound.play();
                result = true;
            }
        }

        return result;
    }
}
 