using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnMouseOverButton : MonoBehaviour
{
    public Sprite changeSprite;
    private Sprite baseSprite;
    private Image image;

    private Text text;
    public string gameMode;

    private void Start()
    {
        image = GetComponent<Image>();
        text = GetComponentInChildren<Text>();

        text.text = gameMode;

        baseSprite = image.sprite;
    }

    private void Update()
    {
        ChangeSprite();
    }

    private void ChangeSprite()
    {
        if (Vector3.Distance(gameObject.transform.position, Input.mousePosition) <= 55 ||
            (Input.touchCount > 0 && (Input.GetTouch(0).phase == TouchPhase.Began && Vector3.Distance(gameObject.transform.position,Input.GetTouch(0).position) <= 100)))
        {
            image.sprite = changeSprite;
        }
        else
        {
            image.sprite = baseSprite;
        }
    }
}
