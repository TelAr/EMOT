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
    public List<Texture> sprites = new();
    public Texture Transparency;

    public bool IsUniqueCutscene = true;

    private bool isRead = false;
    private bool wasRead = false;
    private int step = 0;
    private Dictionary<int, Dialog> dialogs = new();
    private int startPoint = 0;
    private bool passNextFrame = true;
    private bool autoCutscene = false;
    private float autoTimer = 0;
    private string SavePath="";
    private List<string> logList = new();

    public class Dialog {

        public int id;
        public string Speaker = null, Contents = null;
        public Texture Left = null, Right = null;
        public int next = -1;
    }
    public bool IsCanRead
    {
        get {

            return !((IsUniqueCutscene && wasRead));
        }
    }

    public void GetNextCutscene() {

        if (CutsceneCanvas.instance.LogPanel.activeSelf) {

            return;
        }

        if (!isRead) {
            logList.Clear();
            step = startPoint;
            passNextFrame = true;
            isRead = true;
            wasRead = true;
            CutsceneCanvas.instance.gameObject.SetActive(true);
            CutsceneCanvas.instance.Left.texture = CutsceneCanvas.instance.Right.texture = Transparency;
        }
        UpdateCutscene();
    }

    public void EndCutscene() {

        isRead = false;
        CutsceneCanvas.instance.gameObject.SetActive(false);
        enabled = false;
    }


    private void Awake()
    {

        string fullstring = FileRead();

        SetDialogList(fullstring);

        //Debug
        int pointer = startPoint;
        while (dialogs.ContainsKey(pointer)) {

            Debug.Log("ID: "+dialogs[pointer].id);
            Debug.Log(dialogs[pointer].Speaker+": "+dialogs[pointer].Contents);
            Debug.Log(dialogs[pointer].id + " sprite: " + dialogs[pointer].Left + ", " + dialogs[pointer].Right);
            pointer =dialogs[pointer].next;

        }
    }

    private string FileRead() {

        string fullstring;
        SavePath = Application.dataPath + "/DebugingScript.csv";
        if (script != null)
        {
            fullstring = script.text;
        }
        else
        {
            try
            {
                fullstring = File.ReadAllText(SavePath);
            }
            catch
            {

                fullstring = "";
            }
        }

        return fullstring;
    }

    private void SetDialogList(string fullstring) {

        string[] words = fullstring.Split('\n');
        Dialog now = null;
        bool passFirst = false;

        if (fullstring == "")
        {
            ErrorDialog("ERROR: \"" + SavePath + "\" File is not exist");
            return;
        }

        foreach (string word in words)
        {
            if (!passFirst)
            {
                passFirst = true;
                continue;
            }
            else if (word == "")
            {
                continue;
            }

            List<string> parsing = ParsingWord(word);

            //if parsing size is different
            if (parsing.Count != 6)
            {
                ErrorDialog("ERROR: The number of elements in a particular row is determined to be different\n");
                break;
            }


            int flag = IndexReader(parsing, ref now);
            //if false, error code
            if (flag == -1)
            {
                ErrorDialog("ERROR: ID VALUE IS NOT INTEGER OR \'#\': ID VALUE IS " + parsing[0] + "\n");
                break;
            }
            else if (flag == 0 || flag == 1) {

                continue;
            }

            //sprite read
            SpriteReader(parsing, ref now);

            //SPEAKER info
            now.Speaker = parsing[3];

            //Contents info
            now.Contents = parsing[4];

            //Jump info
            now.next = now.id + 1;
            if (int.TryParse(parsing[5], out int tempNext))
            {
                now.next = tempNext;
            }
        }

        if (now != null)
        {
            dialogs[now.id] = now;
        }
    }

    private List<string> ParsingWord(string word) {

        List<string> parsing = new();
        string[] split = word.Split(',');
        bool isDQM = false;
        for (int t = 0; t < split.Length; t++)
        {
            if (split[t].Length > 0 && split[t][0] == '\"')
            {

                parsing.Add(split[t][1..]);
                isDQM = true;
                continue;
            }

            if (isDQM)
            {
                if (split[t].Length > 0 && split[t][^1] == '\"')
                {
                    isDQM = false;
                    split[t] = split[t][0..^1];
                }
                parsing[^1] += "," + split[t];
            }
            else
            {
                parsing.Add(split[t]);
            }
        }
        return parsing;
    }

    private int IndexReader(List<string> dataList, ref Dialog now) {

        //no id=content add
        if (dataList[0] == "")
        {
            now.Contents += "\n" + dataList[4];
            return 0;
        }
        //remark case
        else if (dataList[0][0] == '#'|| dataList[0][0] == 'Q')
        {
            return 1;
        }
        //next dialog
        else if (int.TryParse(dataList[0], out int tempId))
        {
            if (now != null)
            {
                dialogs.Add(now.id, now);
            }
            else
            {
                startPoint = tempId;
            }
            now = new();
            now.id = tempId;
            return 2;
        }
        else
        {
            return -1;
        }
    }

    private void SpriteReader(List<string> dataList, ref Dialog now)
    {

        //t==0:left, t==1:right
        for (int t = 0; t < 2; t++)
        {
            if (dataList[t + 1] != "")
            {
                //if index error, no sprite
                if (int.TryParse(dataList[t + 1], out int indexValue))
                {

                    if (indexValue < sprites.Count && indexValue >= 0)
                    {
                        if (t == 0) now.Left = sprites[indexValue];
                        else now.Right = sprites[indexValue];                    
                    }
                    else
                    {
                        if (t == 0) now.Left = null;
                        else now.Right = null;
                    }
                }
            }
        }
    }


    private void ErrorDialog(string message) {

        Dialog now = new();
        now.id = startPoint;
        now.Contents = message;
        dialogs.Add(now.id, now);
    }

    private void UpdateCutscene() {
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

        string log = now.Speaker + ": " + now.Contents;
        logList.Add(log);

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

                if (Input.GetKeyDown(KeyCode.Q)) {

                    EndCutscene();
                }
                if (Input.GetKeyDown(KeyCode.A)) {

                    autoTimer = 0;
                    autoCutscene ^= true;
                }
                if (Input.GetKeyDown(KeyCode.L)) {

                    CutsceneCanvas.instance.LogTMP.text = "";
                    foreach (var logs in logList) {

                        CutsceneCanvas.instance.LogTMP.text+=logs+"\n";
                    }
                    CutsceneCanvas.instance.LogPanel.SetActive(CutsceneCanvas.instance.LogPanel.activeSelf ^ true);
                }

                if (CutsceneCanvas.instance.LogPanel.activeSelf) {

                    if (Input.GetKey(KeyCode.UpArrow) && CutsceneCanvas.instance.LogScrollRect.verticalNormalizedPosition < 1f) 
                    {
                        CutsceneCanvas.instance.LogScrollRect.verticalNormalizedPosition+=1f*Time.unscaledDeltaTime;
                    }
                    if (Input.GetKey(KeyCode.DownArrow) && CutsceneCanvas.instance.LogScrollRect.verticalNormalizedPosition > 0f) {

                        CutsceneCanvas.instance.LogScrollRect.verticalNormalizedPosition -= 1f * Time.unscaledDeltaTime;
                    }
                }

                if (autoCutscene) {

                    autoTimer+=Time.unscaledDeltaTime;
                    if (autoTimer > CutsceneCanvas.instance.AutoTime) {

                        autoTimer=0;
                        GetNextCutscene();
                    }
                }

            }
        }
    }


}
