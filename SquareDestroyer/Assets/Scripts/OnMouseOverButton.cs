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

    private void Start()
    {
        image = GetComponent<Image>();

        baseSprite = image.sprite;
    }

    private void Update()
    {
        ChangeSprite();
    }

    private void ChangeSprite()
    {
        if (Vector3.Distance(gameObject.transform.position, Input.mousePosition) <= 55 || (Input.touchCount > 0 && (Input.GetTouch(0).phase == TouchPhase.Began && Vector3.Distance(gameObject.transform.position,Input.GetTouch(0).position) <= 120)))
        {
            image.sprite = changeSprite;
        }
        else
        {
            image.sprite = baseSprite;
        }
    }
}
