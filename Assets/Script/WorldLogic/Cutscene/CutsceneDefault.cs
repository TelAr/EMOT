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
    private bool passFirstFrame = true;
    private bool autoCutscene = false;
    private float autoTimer = 0;
    private string SavePath="";
    private List<string> logList = new();
    private QuestionController questionController = null;

    public class Dialog {

        public int id;
        public string Speaker = null, Contents = null;
        public Texture Left = null, Right = null;
        public string function = null;
        public int next = -1;
    }

    private class QuestionController {

        public Dictionary<string, List<KeyValuePair<string, int?>>> answers = new();
        public int Pointer = -1;
        public List<string> AnswerList = new();
        public List<int?> answerJumper = new();
        public bool IsQustionState = false;

        public bool TryAnswerJumpGet(out int? jumpPoint) { 
        
            jumpPoint = -1;
            if (Pointer < 0||Pointer>=answerJumper.Count)
            {

                return false;
            }
            else {

                jumpPoint = answerJumper[Pointer];
                return true;
            }
        }

        public void Clear() {

            Pointer = -1;
            AnswerList = new();
            answerJumper = new();
            IsQustionState = false;
        }

        public void PointerMove(int value) { 
        
            Pointer += value;
            if (Pointer < 0) { 
            
                Pointer = 0;
            }
            if (Pointer >= AnswerList.Count) {

                Pointer = AnswerList.Count - 1;
            }
            UpdateAnswers();
        }

        public void UpdateAnswers() {

            if (AnswerList.Count > 0) {

                IsQustionState = true;
            }

            for (int t = 0; t < CutsceneCanvas.instance.Answers.Count; t++) {

                if (t < AnswerList.Count)
                {

                    CutsceneCanvas.instance.Answers[t].text = (t == Pointer ? "¡ß" : '¡Þ') + AnswerList[t];
                }
                else {

                    CutsceneCanvas.instance.Answers[t].text = "";
                }
            }
        }

        public void QuestionReader(List<string> dataList)
        {

            if (!answers.ContainsKey(dataList[0]))
            {

                answers.Add(dataList[0], new List<KeyValuePair<string, int?>>());
            }

            if (int.TryParse(dataList[5], out int partitialIndex))
            {

                answers[dataList[0]].Add(new KeyValuePair<string, int?>(dataList[4], partitialIndex));
            }
            else
            {

                answers[dataList[0]].Add(new KeyValuePair<string, int?>(dataList[4], null));
            }
        }

        public void AnswerSetting(string keyValue)
        {
            bool isError = false;
            keyValue = 'Q' + keyValue;
            if (answers.ContainsKey(keyValue))
            {
                Clear();

                for (int t = 0; t < 4 && t < answers[keyValue].Count; t++)
                {
                    AnswerList.Add(answers[keyValue][t].Key);
                    answerJumper.Add(answers[keyValue][t].Value);
                }

            }
            //error
            else
            {
                isError = true;
                foreach (var answers in CutsceneCanvas.instance.Answers)
                {

                    answers.text = "ERROR: Can't Find "+ keyValue+" Index Answers";
                }
            }
            UpdateAnswers();
            if (isError) {

                IsQustionState = false;
            }
        }
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
            passFirstFrame = true;
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

        questionController=new QuestionController();

        string fullstring = FileRead();

        SetDialogList(fullstring);

        //Debug
        int pointer = startPoint;
        while (dialogs.ContainsKey(pointer)) {

            Debug.Log("ID: "+dialogs[pointer].id);
            Debug.Log(dialogs[pointer].Speaker+": "+dialogs[pointer].Contents);
            pointer =dialogs[pointer].next;
        }

        foreach (var qd in questionController.answers) { 
        
            foreach(var log in qd.Value){

                Debug.Log(qd.Key + ": " + log.Key + ", "+log.Value);
            }
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
            else if (flag == 0) {

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
            //not number=other function MAYBE
            else { 
            
                now.function = parsing[5];
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

    //return 0: done at inner process 
    //return 1: next process need
    //return -1: error
    private int IndexReader(List<string> dataList, ref Dialog now) {

        //no id=content add
        if (dataList[0] == "")
        {
            now.Contents += "\n" + dataList[4];
            return 0;
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
            return 1;
        }
        //remark case
        else 
        {
            switch (dataList[0][0]) {

                case 'Q'://question case
                    questionController.QuestionReader(dataList);
                    return 0;
                case '#'://remark
                    return 0;
                default://error case
                    return -1;
            }

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

        if (questionController.IsQustionState) {

            if (questionController.TryAnswerJumpGet(out int? jp)){

                if (jp != null) {

                    step = jp.Value;
                }
                questionController.Clear();
            }
            else
            {
                return;
            }
        }

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

        foreach (var answers in CutsceneCanvas.instance.Answers) {

            answers.text = "";
        }

        CallFunction(now.function);

        step = now.next;
    }

    void CallFunction(string commands) {

        string[] command = commands.Split('/', ' ', '\t', '\n', '\r');


        foreach (string cmd in command) {
            //QuestionCase
            if (FunctionCompare(cmd, "Q")) 
            {
                questionController.AnswerSetting(ParameterParsing(cmd));
            }
        }
    }

    bool FunctionCompare(string cmd, string functionName) {

        return cmd.Contains(')') && cmd.Contains('(') &&
                cmd.Substring(0, cmd.IndexOf('(')) == functionName &&
                cmd.IndexOf(')') > cmd.IndexOf('(') &&
                cmd.IndexOf(')') == cmd.Length - 1;
    }

    string ParameterParsing(string input) {

        return input.Substring(input.IndexOf('(') + 1, input.IndexOf(')') - input.IndexOf('(') - 1);
    }

    private bool IsUpdateable
    {
        get {

            return isRead && !GameController.GetGameController.IsSystemPause;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsUpdateable) {
            
            if (passFirstFrame)
            {
                passFirstFrame = false;
                return;
            }

            if (!questionController.IsQustionState)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    while (!questionController.IsQustionState && IsUpdateable)
                    {

                        UpdateCutscene();
                    }
                }
                if (Input.GetKeyDown(KeyCode.A))
                {

                    autoTimer = 0;
                    autoCutscene ^= true;
                }

                if (autoCutscene)
                {

                    autoTimer += Time.unscaledDeltaTime;
                    if (autoTimer > CutsceneCanvas.instance.AutoTime)
                    {

                        autoTimer = 0;
                        GetNextCutscene();
                    }
                }
            }
            else {

                if (Input.GetKeyDown(KeyCode.LeftArrow)) {

                    questionController.PointerMove(-1);
                }
                if (Input.GetKeyDown(KeyCode.RightArrow)) {

                    questionController.PointerMove(1);
                }
            }
            
            if (Input.GetKeyDown(KeyCode.L))
            {

                CutsceneCanvas.instance.LogTMP.text = "";
                foreach (var logs in logList)
                {

                    CutsceneCanvas.instance.LogTMP.text += logs + "\n";
                }
                CutsceneCanvas.instance.LogPanel.SetActive(CutsceneCanvas.instance.LogPanel.activeSelf ^ true);
            }

            if (CutsceneCanvas.instance.LogPanel.activeSelf)
            {

                if (Input.GetKey(KeyCode.UpArrow) && CutsceneCanvas.instance.LogScrollRect.verticalNormalizedPosition < 1f)
                {
                    CutsceneCanvas.instance.LogScrollRect.verticalNormalizedPosition += 1f * Time.unscaledDeltaTime;
                }
                if (Input.GetKey(KeyCode.DownArrow) && CutsceneCanvas.instance.LogScrollRect.verticalNormalizedPosition > 0f)
                {

                    CutsceneCanvas.instance.LogScrollRect.verticalNormalizedPosition -= 1f * Time.unscaledDeltaTime;
                }
            }
        }
    }


}
