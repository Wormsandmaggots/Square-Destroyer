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

    [Range(0,1)]
    public float timeScaleOnScreen;

    public int hp = 6;
    public Heart[] hearts;
    private GameObject hpDisplay;

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
        pointsDisplay.SetActive(false);
        
        startScreen = GameObject.Find("StartScreen");
        gameOverScreen = GameObject.Find("GameOverScreen");
        gameOverScreen.SetActive(false);
        startScreen.SetActive(true);
        
        hpDisplay = GameObject.Find("HP");
        hearts = hpDisplay.GetComponentsInChildren<Heart>();
        hpDisplay.SetActive(false);
        
        pointsAnim.SetBool("GainPoint", false);

        hp = hearts.Length * hearts[0].hpAmount;
        
        SetTimeScale(timeScaleOnScreen);
    }

    public void UpdatePoints(int amount)
    {
        points += amount;
        
        if (points % 10 == 0 && hp < 6)
        {
            UpdateHp(1);
        }

        pointsDisplay.GetComponent<Text>().text = points.ToString();
        pointsAnim.SetBool("GainPoint", true);
    }

    public void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
    }

    public bool IsScreenActive()
    {
        if (startScreen.activeSelf || 
            gameOverScreen.activeSelf)
        {
            return true;
        }

        return false;
    }

    public void UpdateHp(int amount)
    {
        hp += amount;
        
        if (amount < 0)
        {
            for (int i = hearts.Length - 1; i >= 0; i--)
            {
                if (hearts[i].hpAmount > 0)
                {
                    hearts[i].hpAmount += amount;
                    hearts[i].UpdateHeartsSprite();
                    break;
                }
            }
            
            foreach (Heart heart in hearts)
            {
                heart.lossHP = true;
                heart.tempHPLossCooldown = heart.hpLossCooldown;
            }
            
            if (hp <= 0)
            {
                GameOver();
            }
        }
        else
        {
            for (int i = 0; i < hearts.Length; i++)
            {
                if (hearts[i].hpAmount < 2)
                {
                    hearts[i].hpAmount += amount;
                    hearts[i].UpdateHeartsSprite();
                    break;
                }
            }
        }
    }

    private void GameOver()
    {
        Debug.Log("GAME OVER");
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
