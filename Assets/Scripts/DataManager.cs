using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    public TMP_InputField NameInputField;
    public static Dictionary<string, string> saveDict;
    public static string inputName;
    public static string parsedName;
    public static int parsedScore;
    public static int currentScore;
    

    void Awake()
    {
        CheckScript();
    }
    void CheckScript()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
