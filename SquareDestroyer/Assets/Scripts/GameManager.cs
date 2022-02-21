using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private Text pointsDisplay;

    private void Start()
    {
        pointsDisplay = GameObject.Find("Points").GetComponent<Text>();
    }

    public void UpdatePoints(int amount)
    {
        points += amount;
        pointsDisplay.text = points.ToString();
    }


}
