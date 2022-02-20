using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private Spawner spawner;
    
    public float speed;
    
    void Start()
    {
        spawner = GetComponentInParent<Spawner>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveSquare();
    }

    private void MoveSquare()
    {
        Vector3 move = spawner.xAxisMovement ? Vector3.right : Vector3.up;

        move.x = move.x * (float)spawner.direction * speed * Time.deltaTime;
        move.y = move.y * (float)spawner.direction * speed * Time.deltaTime;
        
        transform.Translate(move);
    }
}
