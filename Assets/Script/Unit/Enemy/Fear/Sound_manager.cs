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
        sounds = Load_audio(path);
    }

    private AudioClip[] Load_audio(string obj_path)
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

    public AudioSource Play(string sound_name)
    {
        AudioSource result = null;

        Sound_Class.Sound_subClass subClass = GetSubClass(sound_name);
        if (subClass != null)
        {
            result = subClass.play();
        }
        return result;
    }

    public AudioSource PlayLoop(string sound_name)
    {
        AudioSource result = null;

        Sound_Class.Sound_subClass subClass = GetSubClass(sound_name);
        if (subClass != null)
        {
            result = subClass.playLoop();
        }
        return result;
    }

    public bool IsSound(string sound_name)
    {
        bool result = false;

        if (GetSubClass(sound_name))
        {
            result = true;
        }

        return result;
    }

    public Sound_Class.Sound_subClass[] GetSubClassList()
    {
        GameObject soundObject = gameObject.transform.Find("Sounds").gameObject;
        return soundObject.GetComponents<Sound_Class.Sound_subClass>();
    }

    public Sound_Class.Sound_subClass GetSubClass(string sound_name)
    {
        Sound_Class.Sound_subClass result = null;

        Sound_Class.Sound_subClass[] sound_list = GetSubClassList();

        foreach (Sound_Class.Sound_subClass sound in sound_list)
        {
            if (Equals(sound.Name, sound_name))
            {
                result = sound;
            }
        }

        return result;
    }
}
 