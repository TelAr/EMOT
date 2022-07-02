using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using UnityEngine;
using System.IO;

public class CutsceneDefault : MonoBehaviour
{
    [Tooltip("Must to use CSV Format, UTF-8 encoding")]
    public TextAsset script = null;
    public List<Texture> sprites = new List<Texture>();
    public Texture Transparency;


    public bool IsUniqueCutscene = true;

    private bool isRead = false;
    private bool wasRead = false;
    private int step = 0;
    private Dictionary<int, Dialog> dialogs = new Dictionary<int, Dialog>();
    private int startPoint = 0;
    private bool passNextFrame = true;

    private string SavePath="";


    public class Dialog {

        public int id;
        public string Speaker = null, Contents = null;
        public Texture Left = null, Right = null;
        public int next = -1;
    }
    public bool IsCanRead
    {
        get {

            return !((IsUniqueCutscene && wasRead) || isRead);
        }
    }

    public void InitiateCutscene() {
        step = startPoint;
        passNextFrame = true;
        isRead = true;
        wasRead = true;
        CutsceneCanvas.instance.gameObject.SetActive(true);
        CutsceneCanvas.instance.Left.texture = CutsceneCanvas.instance.Right.texture = Transparency;
        updateCutscene();
    }

    public void EndCutscene() {

        isRead = false;
        CutsceneCanvas.instance.gameObject.SetActive(false);
    }


    private void Awake()
    {

        string fullstring;

        SavePath = Application.dataPath + "/DebugingScript.csv";
        if (script != null) {
            fullstring = script.text;
        }
        else {
            try
            {
                fullstring = File.ReadAllText(SavePath);
            }
            catch {

                fullstring = "";
            }
        }
        string[] words = fullstring.Split('\n');
        Dialog now = null;
        bool passFirst = false;

        if (fullstring == "")
        {
            now = new Dialog();
            now.id = startPoint;
            now.Contents = "ERROR: \"DebugingScript.csv\" File is not exist";
            dialogs.Add(now.id, now);
            return;
        }

        foreach (string word in words)
        {
            if (!passFirst) {

                passFirst = true;
                continue;
            }

            if (word == "") {

                continue;
            }

            List<string> parsing = new List<string> ();
            string[] split = word.Split(',');
            bool isDQM = false;
            for (int t = 0; t < split.Length; t++) {
                if (split[t].Length > 0 && split[t][0] == '\"') {

                    parsing.Add(split[t].Substring(1));
                    isDQM = true;
                    continue;
                }

                if (isDQM)
                {

                    
                    if (split[t].Length > 0 && split[t][split[t].Length - 1] == '\"')
                    {
                        isDQM = false;
                        split[t] = split[t].Substring(0, split[t].Length - 1);
                    }
                    parsing[parsing.Count - 1] += "," + split[t];

                }
                else {

                    parsing.Add(split[t]);
                }
                
            }

            //if parsing size is different
            if (parsing.Count != 6) {

                now = new Dialog();
                now.id = startPoint;
                now.Contents = "ERROR: The number of elements in a particular row is determined to be different\n";
                dialogs.Add(now.id, now);
                break;
            }

            //ID info
            int tempId;
            //no id=content add
            if (parsing[0] == "")
            {

                now.Contents += "\n" + parsing[4];
                continue;
            }
            //remark case
            else if (parsing[0][0] == '#')
            {
                continue;
            }
            //next dialog
            else if (int.TryParse(parsing[0], out tempId))
            {
                if (now != null)
                {
                    dialogs.Add(now.id, now);
                }
                else {

                    startPoint = tempId;
                }
                now = new Dialog();
                now.id = tempId;
            }
            //if false, error code
            else {

                now = new Dialog();
                now.id = startPoint;
                now.Contents = "ERROR: ID VALUE IS NOT INTEGER OR \'#\': ID VALUE IS " + parsing[0] + "\n";
                dialogs.Add(now.id, now);
                break;
            }

            //LEFT SPRITE INFO
            if (parsing[1] != "") {

                //if index error, no sprite
                int indexValue;
                if (int.TryParse(parsing[1], out indexValue)) {

                    if (indexValue < sprites.Count && indexValue >= 0)
                    {

                        now.Left = sprites[indexValue];
                    }
                    else {

                        now.Left = null;
                    }
                }
            }

            //RIGHT SPRITE INFO
            if (parsing[2] != "")
            {

                //if index error, no sprite
                int indexValue;
                if (int.TryParse(parsing[2], out indexValue))
                {

                    if (indexValue < sprites.Count && indexValue >= 0)
                    {

                        now.Right = sprites[indexValue];
                    }
                    else
                    {

                        now.Right = null;
                    }
                }
            }

            //SPEAKER info
            now.Speaker = parsing[3];

            //Contents info
            now.Contents = parsing[4];

            //Jump info
            int tempNext;
            now.next = now.id + 1;
            if (int.TryParse(parsing[5], out tempNext)) {

                now.next = tempNext;
            }
            
        }

        if (now != null)
        {
            dialogs[now.id] = now;
        }

        //Debug
        int pointer = startPoint;
        while (dialogs.ContainsKey(pointer)) {

            Debug.Log("ID: "+dialogs[pointer].id);
            Debug.Log(dialogs[pointer].Speaker+": "+dialogs[pointer].Contents);
            pointer=dialogs[pointer].next;
        }
    }

    private void updateCutscene() {
        Dialog now;
        if (dialogs.ContainsKey(step))
        {
            now = dialogs[step];
        }
        else {

            EndCutscene();
            return;
        }

        CutsceneCanvas.instance.Speaker.text = now.Speaker;
        CutsceneCanvas.instance.contents.text = now.Contents;
        if (now.Left != null)
        {
            CutsceneCanvas.instance.Left.texture = now.Left;
        }

        if (now.Right != null)
        {
            CutsceneCanvas.instance.Right.texture = now.Right;
        }


        step = now.next;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRead) {
            if (passNextFrame)
            {

                passNextFrame = false;
            }
            else {

                if (Input.GetKeyDown(KeyCode.Space))
                {

                    updateCutscene();
                }
            }
        }
    }


}
