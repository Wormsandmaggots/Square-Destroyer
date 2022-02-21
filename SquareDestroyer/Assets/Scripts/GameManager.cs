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

    private int points;
    private GameObject pointsDisplay;
    private Animator pointsAnim;

    private GameObject startScreen;
    private GameObject gameOverScreen;
    private GameObject modeScreen;

    [Range(0,1)]
    public float timeScaleOnScreen;

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
        pointsAnim.SetBool("GainPoint", false);
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


}
