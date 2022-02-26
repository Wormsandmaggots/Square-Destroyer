using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{
    [HideInInspector]
    public int hpAmount = 2;
    public float changeSpriteCooldown;
    private float tempChangeSpriteCooldown;
    public float hpLossCooldown;
    [HideInInspector]
    public float tempHPLossCooldown;

    public bool lossHP;

    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;
    public Sprite highlightedHeart;
    private Sprite currentSprite;

    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
        currentSprite = image.sprite;
        fullHeart = currentSprite;
        
        tempChangeSpriteCooldown = changeSpriteCooldown;
        
        tempHPLossCooldown = 0;
    }

    private void Update()
    {
        if (lossHP)
        {
            OnHPLoss();
        }
    }

    public void UpdateHeartsSprite()
    {
        switch (hpAmount)
        {
            case 0:
                currentSprite = emptyHeart;
                image.sprite = emptyHeart;
                break;
            case 1:
                currentSprite = halfHeart;
                image.sprite = halfHeart;
                break;
            case 2:
                currentSprite = fullHeart;
                image.sprite = fullHeart;
                break;
        }
    }

    private void OnHPLoss()
    {
        if (tempHPLossCooldown > 0)
        {
            if (tempChangeSpriteCooldown >= 0)
            {
                image.sprite = currentSprite;
            }
            else
            {
                image.sprite = highlightedHeart;
            }

            tempChangeSpriteCooldown -= Time.deltaTime;
            tempHPLossCooldown -= Time.deltaTime;

            if (tempChangeSpriteCooldown <= -changeSpriteCooldown)
            {
                tempChangeSpriteCooldown = changeSpriteCooldown;
            }
        }
        else
        {
            lossHP = false;
            image.sprite = currentSprite;
        }
        
    }
}
