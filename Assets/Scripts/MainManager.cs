using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;
using System;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public static int bricksDestroyed = 0;
    public int lineCount = 6;
    public Rigidbody Ball;
    public GameObject BallGameobj;

    public Text ScoreText;
    public Text BestScoreText;
    public GameObject GameOverText;
    public GameObject GameWinText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;
    private bool m_GameWin = false;


    void Awake()
    {
        BestScoreText.text = $"Best Score : {DataManager.parsedName} : {DataManager.parsedScore}";
    }

    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < lineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = UnityEngine.Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver||m_GameWin)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;

        if (bricksDestroyed == 36)
        {
            GameWin();
        }
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        CheckScore();
        m_GameOver = true;
        GameOverText.SetActive(true);

    }

    void GameWin()
    {
        CheckScore();
        Destroy(BallGameobj);
        m_GameWin = true;
        GameWinText.SetActive(true);
    }

    void CheckScore()
    {
        if (m_Points > DataManager.parsedScore)
        {
            if (string.IsNullOrEmpty(DataManager.inputName))
            {
                DataManager.saveDict["Name"] = "Name";
            }

            else
            {
                DataManager.saveDict["Name"] = DataManager.inputName;
            }
            
            DataManager.saveDict["Score"] = m_Points.ToString();

            string json = JsonConvert.SerializeObject(DataManager.saveDict);
            File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        }
    }
}
