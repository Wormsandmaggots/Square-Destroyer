using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

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
    public GameObject hpDisplay;

    public Record[] records;
    public Text currentScoreDisplay;
    public GameObject newRecord;

    public bool start;

    //[HideInInspector]
    public GameMode gameMode = 0;
    public enum GameMode
    {
        Relax,
        Progress,
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
        
        records = gameOverScreen.GetComponentsInChildren<Record>();
        foreach (Record record in records) 
        {
            record.SetRecord(PlayerPrefs.GetInt(record.name,0));
        }
        newRecord.SetActive(false);
        
        hpDisplay = GameObject.Find("HP");
        hearts = hpDisplay.GetComponentsInChildren<Heart>();
        hpDisplay.SetActive(false);

        pointsAnim.SetBool("GainPoint", false);

        hp = hearts.Length * hearts[0].hpAmount;

        start = true;
        
        AudioManager.instance.PlayDelayed("StartScreen", 1);
        
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
        
        if (amount == -1)
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
            
            AudioManager.instance.Play("HPLost");
            
            if (hp <= 0)
            {
                GameOver();
            }
            
            hpDisplay.GetComponent<Animator>().SetBool("Lost", true);
        }
        else if(amount == 1)
        {
            for (int i = 0; i < hearts.Length; i++)
            {
                if (hearts[i].hpAmount < 2)
                {
                    hearts[i].hpAmount++;
                    hearts[i].UpdateHeartsSprite();
                    hpDisplay.GetComponent<Animator>().SetBool("Gain", true);
                    AudioManager.instance.Play("HPGain");
                    break;
                }
            }
        }
        else
        {
            foreach (Heart heart in hearts)
            {
                heart.hpAmount = 2;
                heart.UpdateHeartsSprite();
            }
        }

        if (hp > 6)
        {
            hp = 6;
        }
    }

    private void GameOver()
    {
        AudioManager.instance.Play("GameOver");
        
        Save();
        
        currentScoreDisplay.text = points.ToString();
        points = 0;
        pointsDisplay.GetComponent<Text>().text = points.ToString();

        hpDisplay.SetActive(false);
        pointsDisplay.SetActive(false);
        gameOverScreen.SetActive(true);
        
        UpdateHp(6);

        start = false;
        
        InterstitialAd.ad.ShowAd();
    }

    private void Save()
    {
        for (int i = 0; i < records.Length; i++)
        {
            if (records[i].name == gameMode.ToString())
            {
                if (points > Convert.ToInt32(records[i].record.text))
                {
                    newRecord.SetActive(true);
                    records[i].SetRecord(points);
                    PlayerPrefs.SetInt(gameMode.ToString(),points);
                }
                else
                {
                    newRecord.SetActive(false);
                }
                break;
            }
        }
    }

    public void LoadScene(string sceneName)
    {
        pointsDisplay.SetActive(true);
        start = true;
        AudioManager.instance.Play("Click");
        AudioManager.instance.Stop("StartScreen");
        SceneManager.LoadScene(sceneName);
    }

    public void LoadScene(int sceneId)
    {
        pointsDisplay.SetActive(true);
        start = true;
        AudioManager.instance.Play("Click");
        AudioManager.instance.Stop("StartScreen");
        SceneManager.LoadScene(sceneId);
    }

    public void ChangeGameMode(int modeID)
    {
        if (modeID == (int)GameMode.Relax)
        {
            gameMode = GameMode.Relax;
        }
        else if (modeID == (int)GameMode.Progress)
        {
            gameMode = GameMode.Progress;
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
        else if(name == GameMode.Progress.ToString())
        {
            gameMode = GameMode.Progress;
        }
        else
        {
            gameMode = GameMode.Relax;
        }
    }
}
