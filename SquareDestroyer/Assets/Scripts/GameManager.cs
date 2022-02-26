using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }
    
    [HideInInspector]
    public int points;
    private GameObject pointsDisplay;
    private Animator pointsAnim;

    private GameObject startScreen;
    private GameObject gameOverScreen;
    private GameObject modeScreen;
    
    [Range(0,1)]
    public float timeScaleOnScreen;

    public int hp;

    //[HideInInspector]
    public GameMode gameMode = 0;
    public enum GameMode
    {
        Relaxing,
        Progressive,
        Chaos
    }

    private void Start()
    {
        pointsDisplay = GameObject.Find("Points");
        pointsAnim = GameObject.Find("Points").GetComponent<Animator>();

        startScreen = GameObject.Find("StartScreen");
        gameOverScreen = GameObject.Find("GameOverScreen");
        modeScreen = GameObject.Find("ModeScreen");
        
        pointsDisplay.SetActive(false);
        
        gameOverScreen.SetActive(false);
        modeScreen.SetActive(false);
        startScreen.SetActive(true);

        pointsAnim.SetBool("GainPoint", false);
        
        SetTimeScale(timeScaleOnScreen);
    }

    public void UpdatePoints(int amount)
    {
        points += amount;
        pointsDisplay.GetComponent<Text>().text = points.ToString();
        pointsAnim.SetBool("GainPoint", true);
    }

    public void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
    }

    public bool IsScreenActive()
    {
        if (modeScreen.activeSelf || startScreen.activeSelf || gameOverScreen.activeSelf)
        {
            return true;
        }

        return false;
    }

    public void UpdateHP(int subtractionAmount)
    {
        hp -= subtractionAmount;
    }

    public void LoadScene(string sceneName)
    {
        pointsDisplay.SetActive(true);
        SceneManager.LoadScene(sceneName);
    }

    public void LoadScene(int sceneId)
    {
        pointsDisplay.SetActive(true);
        SceneManager.LoadScene(sceneId);
    }

    public void ChangeGameMode(int modeID)
    {
        if (modeID == (int)GameMode.Relaxing)
        {
            gameMode = GameMode.Relaxing;
        }
        else if (modeID == (int)GameMode.Progressive)
        {
            gameMode = GameMode.Progressive;
        }
        else
        {
            gameMode = GameMode.Chaos;
        }
    }

    public void ChangeGameMode(string name)
    {
        if (name == GameMode.Chaos.ToString())
        {
            gameMode = GameMode.Chaos;
        }
        else if(name == GameMode.Progressive.ToString())
        {
            gameMode = GameMode.Progressive;
        }
        else
        {
            gameMode = GameMode.Relaxing;
        }
    }
}
