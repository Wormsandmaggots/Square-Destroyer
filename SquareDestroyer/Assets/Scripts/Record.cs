using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Record : MonoBehaviour
{
    public Text record;
    public Text recordName;
    public string name;

    private void Start()
    {
        recordName.text = name;
    }

    public void SetRecord(int points)
    {
        record.text = points.ToString();
    }
}
