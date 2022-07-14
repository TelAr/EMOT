using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;

public class Sound_manager : MonoBehaviour
{
    public AudioClip[] sound_list;
    public Object[] sound_path_list;

    private GameObject soundObject;
    private Sound_Class sound_Class;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    private void Init()
    {
        soundObject = new("Sounds");
        soundObject.transform.parent = gameObject.transform;

        sound_Class = soundObject.AddComponent<Sound_Class>();
            
        sound_Class.Add(sound_list);

        foreach (Object sound_path in sound_path_list)
        {
            Load_audio(AssetDatabase.GetAssetPath(sound_path));
        }
    }

    private void Load_audio(string obj_path)
    {
        Match match = Regex.Match(obj_path, "Assets/Resources/.*");
        if (match.Success)
        {
            obj_path = obj_path[17..];
        }

        sound_Class.Add(Resources.LoadAll<AudioClip>(obj_path));

        return;
    }

    public AudioSource Play(string sound_name)
    {
        AudioSource result = null;

        Sound_Class.Sound_subClass subClass = GetSubClass(sound_name);
        if (subClass != null)
        {
            result = subClass.Play();
        }
        return result;
    }

    public AudioSource PlayLoop(string sound_name)
    {
        AudioSource result = null;

        Sound_Class.Sound_subClass subClass = GetSubClass(sound_name);
        if (subClass != null)
        {
            result = subClass.PlayLoop();
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
 