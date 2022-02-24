using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Square : MonoBehaviour
{
    public Spawner spawner;

    public ParticleSystem particlesPrefab;
    
    public float speed;

    private BoxCollider2D collider;

    private int nextAngel = 0;
    private float stopTime;
    private float tempStopTime;

    public int randomMovement;

    void Start()
    {
        spawner = GetComponentInParent<Spawner>();
        collider = GetComponent<BoxCollider2D>();

        GetComponent<SpriteRenderer>().color = new Color(Random.value, Random.value, Random.value);

        if (GameManager.instance.IsScreenActive())
        {
            collider.enabled = false;
        }
        else
        {
            collider.enabled = true;
        }
        
        float maxSpeed = (float)GameManager.instance.points / 50;
        if (GameManager.instance.gameMode == GameManager.GameMode.Chaos)
        {
            maxSpeed = 5.5f;
            randomMovement = Random.Range(0, 4);
        }
        else
        {
            randomMovement = 0;
            
            if (maxSpeed < 1)
            {
                maxSpeed = 1;
            }
            else if (maxSpeed > 5.5f)
            {
                maxSpeed = 5.5f;
            }

            if (spawner.xAxisMovement && maxSpeed > 4.5f)
            {
                maxSpeed = 4.5f;
            }

            if (spawner.randomSquareSpeed)
            {
                speed = Random.Range(0.6f, maxSpeed);
            }
        }
        
        stopTime = Random.Range(0.5f, 1f);
        tempStopTime = stopTime;
    }

    // Update is called once per frame
    void Update()
    {
        switch (randomMovement)
        {
            case 0:
            {
                MoveInDirection();
                break;
            }
            case 1:
            {
                MoveInDirection();
                MoveInCircle();
                break;
            }
            case 2:
            {
                MoveInDirection();
                MoveInSinus();
                break;
            } 
            case 3:
            {
                MoveAndStop();
                break; 
            }
        }
    }

    private void OnMouseDown()
    {
        ParticleSystem ps = Instantiate(particlesPrefab, transform.position,new Quaternion(0,0,180,0));
        ps.startColor = GetComponent<SpriteRenderer>().color;
        
        GameManager.instance.UpdatePoints(1);
        
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Collider"))
        {
            if (spawner.direction == col.gameObject.GetComponent<BoxDestroyer>().directionDestroy)
            {
                GameManager.instance.UpdateHP(1);
                Destroy(gameObject);
            }
        }
    }
    
    private void MoveInDirection()
    {
        Vector3 move = spawner.xAxisMovement ? Vector3.right : Vector3.up;
        
        move.x = move.x * (float)spawner.direction * speed * Time.deltaTime;
        move.y = move.y * (float)spawner.direction / 2f * speed * Time.deltaTime;
                
        transform.Translate(move);
    }
        
    private void MoveInCircle()
    {
        Vector3 move = new Vector3(math.sin(nextAngel) * speed * Time.deltaTime, math.cos(nextAngel) * speed * Time.deltaTime);
        nextAngel++;
        
        transform.Translate(move);
    }
        
    private void MoveInSinus()
    {
        Vector3 move;
        if (spawner.xAxisMovement)
        {
            move = new Vector3(0, math.sin(nextAngel));
            move.y *= Time.deltaTime;
        }
        else
        {
            move = new Vector3(math.sin(nextAngel), 0);
            move.x *= Time.deltaTime;
        }
        nextAngel++;
                
        transform.Translate(move);
    }

    private void MoveObliquely()
    {
        Vector3 move = new Vector3();
    }
        
    private void MoveAndStop()
    {
        if(tempStopTime <= 0)
        {
            tempStopTime += Time.deltaTime;
            MoveInDirection();
            Debug.Log("Move");
            return;
        }
        else if(tempStopTime >= stopTime)
        {
            Debug.Log("Stop");
            tempStopTime = -stopTime;
            return;
        }

        tempStopTime += Time.deltaTime;
    }
}
