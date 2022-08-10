using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;
using Newtonsoft.Json;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class MenuUIHandler : MonoBehaviour
{
    public TextMeshProUGUI BestScoreText;
    public TMP_InputField NameInputField;
    public static int parsedScore;
    public static string parsedName;


    void Awake()
    {
        string jsonFilePath = Application.persistentDataPath + "/savefile.json";

        GetData(jsonFilePath);
    }

    void GetData(string jsonFilePath)
    {
        if (!File.Exists(jsonFilePath))
        {
            var tempDict = new Dictionary<string, string>();
            tempDict.Add("Name", "Name");
            tempDict.Add("Score", "00");

            string json = JsonConvert.SerializeObject(tempDict);
            File.WriteAllText(jsonFilePath, json);

            DataManager.saveDict = tempDict;
            DataManager.parsedName = tempDict["Name"];
            DataManager.parsedScore = int.Parse(tempDict["Score"]);
        }

        else if (File.Exists(jsonFilePath))
        {
            string jsonFile = File.ReadAllText(jsonFilePath);
            var parsedDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonFile);

            parsedName = parsedDict["Name"];
            parsedScore = int.Parse(parsedDict["Score"]);

            if (parsedScore != 0)
            {
                BestScoreText.text = $"Best Score : {parsedName} : {parsedScore}";
            }

            DataManager.saveDict = parsedDict;
            DataManager.parsedName = parsedName;
            DataManager.parsedScore = parsedScore;
        }
    }

    void ReadInputField()
    {
        DataManager.inputName = NameInputField.text;
    }

    void StartNew()
    {
        SceneManager.LoadScene(1);
    }

    void Exit()
    {
        
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
# endif
    }

}
