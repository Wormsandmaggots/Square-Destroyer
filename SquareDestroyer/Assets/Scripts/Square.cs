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
    
    private float stopTime;
    private float tempStopTime;
    
    private float difference;
    
    public float sinusMovementSpeedRange;
    private float randomMagnitude;
    private float randomHeight;

    public Movement randomMovement;
    public enum Movement
    {
        Straight,
        ForwardAndBackward,
        Sinus,
        StopAndGo,
        Obliquely,
        Bow,
        Fall,
        Circle
    }

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
        float minSpeed = 0.6f;
        if (GameManager.instance.gameMode == GameManager.GameMode.Chaos)
        {
            maxSpeed = 4.5f;
            if (spawner.xAxisMovement)
            {
                maxSpeed = 4f;
            }
            
            randomMovement = (Movement)Random.Range(0, 6);
            if (randomMovement == Movement.StopAndGo)
            {
                minSpeed = (int)Movement.StopAndGo;
            }

            speed = Random.Range(minSpeed, maxSpeed);
        }
        else
        {
            randomMovement = Movement.Straight;
            
            if (maxSpeed < 1)
            {
                maxSpeed = 1;
            }
            else if (maxSpeed > 4.5f)
            {
                maxSpeed = 4.5f;
            }

            if (spawner.xAxisMovement && maxSpeed > 4f)
            {
                maxSpeed = 4f;
            }

            if (spawner.randomSquareSpeed)
            {
                speed = Random.Range(minSpeed, maxSpeed);
            }
        }

        stopTime = Random.Range(0.5f, 1.5f);
        tempStopTime = stopTime;
        
        if (randomMovement == Movement.Obliquely)
        {
            if (spawner.xAxisMovement)
            {
                difference = spawner.transform.position.y - gameObject.transform.position.y;
                difference /= 2.5f;
            }
            else
            {
                difference = spawner.transform.position.x - gameObject.transform.position.x;
                difference /= 2;
            }
        }
        else if (randomMovement == Movement.Sinus ||
                 randomMovement == Movement.Bow || 
                 randomMovement == Movement.ForwardAndBackward ||
                 randomMovement == Movement.Circle)
        {
            randomMagnitude = Random.Range(sinusMovementSpeedRange / 2, sinusMovementSpeedRange);
            randomHeight = Random.Range(sinusMovementSpeedRange / 2, sinusMovementSpeedRange);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        switch (randomMovement)
        {
            case Movement.Straight:
            {
                MoveInDirection();
                break;
            }
            case Movement.ForwardAndBackward:
            {
                MoveInDirection();
                MoveForwardAndBackward();
                break;
            }
            case Movement.Sinus:
            {
                MoveInDirection();
                MoveInSinus();
                break;
            } 
            case Movement.StopAndGo:
            {
                MoveAndStop();
                break; 
            }
            case Movement.Obliquely:
            {
                MoveObliquely();
                MoveInDirection();
                break;
            }
            case Movement.Bow:
            {
                MoveInBow();
                MoveInDirection();
                break;
            }
            case Movement.Fall:
            {
                MoveInFall();
                MoveInDirection();
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
        
    private void MoveForwardAndBackward()
    {
        Vector3 move;
        if (spawner.xAxisMovement)
        {
            move = new Vector3(math.sin(Time.time * randomMagnitude) * math.cos(Time.time * randomMagnitude) * Time.deltaTime * randomHeight,0);
        }
        else
        {
            move = new Vector3(0, math.sin(Time.time * randomMagnitude) * math.cos(Time.time * randomMagnitude) * Time.deltaTime * randomHeight);
        }

        transform.Translate(move);
    }
        
    private void MoveInSinus()
    {
        Vector3 move;
        if (spawner.xAxisMovement)
        {
            move = new Vector3(0, math.sin(Time.time * sinusMovementSpeedRange) * sinusMovementSpeedRange);
            move.y *= Time.deltaTime;
        }
        else
        {
            move = new Vector3(math.sin(Time.time * randomMagnitude) * randomHeight, 0);
            move.x *= Time.deltaTime;
        }

        transform.Translate(move);
    }

    private void MoveObliquely()
    {
        Vector3 move;
        if (spawner.xAxisMovement)
        {
            move = new Vector3(0, difference);
            move.y *= Time.deltaTime;
        }
        else
        {
            move = new Vector3(difference, 0);
            move.x *= Time.deltaTime;
        }
        
        transform.Translate(move);
    }
        
    private void MoveAndStop()
    {
        if(tempStopTime <= 0)
        {
            tempStopTime += Time.deltaTime;
            MoveInDirection();
            return;
        }
        else if(tempStopTime >= stopTime)
        {
            tempStopTime = -stopTime;
            return;
        }

        tempStopTime += Time.deltaTime;
    }

    private void MoveInBow()
    {
        Vector3 move;
        move = new Vector3(math.sin(Time.time * randomMagnitude) * randomHeight * Time.deltaTime,
            math.cos(Time.time * randomMagnitude) * randomHeight * Time.deltaTime);
        
        transform.Translate(move);
    }

    private void MoveInFall()
    {
        Vector3 move;
        move = new Vector3(math.sin(Time.time * randomMagnitude) * Time.deltaTime * randomHeight, math.sin(Time.time * randomMagnitude) * Time.deltaTime * randomHeight);
        
        transform.Translate(move);
    }
}
