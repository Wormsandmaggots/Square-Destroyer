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
    private float maxSpeed;
    private float minSpeed = 0.6f;

    public float destroyCooldown;

    private BoxCollider2D collider;
    
    private float stopTime;
    private float tempStopTime;
    
    //defines the position relative to spawner
    private float difference;

    public float sinusMovementSpeedRange;
    private float randomMagnitude;
    private float randomHeight;
    
    public float rotationSpeed;

    private bool isFriend;
    private bool isHeart;

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
        RotationYAxis,
        RotationXAxis,
    }

    void Start()
    {
        //spawner = GetComponentInParent<Spawner>();
        collider = GetComponent<BoxCollider2D>();

        isFriend = CompareTag("Friend");
        isHeart = CompareTag("Heart");

        if (!isFriend)
        {
            GetComponent<SpriteRenderer>().color = new Color(Random.value, Random.value, Random.value);
        }
        
        maxSpeed = (float)GameManager.instance.points / 50;
        
        TurnOffCollider();
        MakeChangesOnGameMode();
        MakeChangesOnMovement();
        MakeChangesOnCollider();

        MakeChangesOnAxis();
    }
    
    void Update()
    {
        if (GameManager.instance.start)
        {
            Move();
            OnRaycast();
            
            TurnOffCollider();

            if (destroyCooldown <= 0)
            {
                Destroy(gameObject);
            }

            destroyCooldown -= Time.deltaTime;
        }
    }

    private void DestroySquare()
    {
        Destroy(gameObject);
    }

    private void OnRaycast()
    {
#if UNITY_EDITOR
        DetectMouseClickOnSquare();
#endif
        DetectTouchWithSquare();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!isFriend)
        {
            if (col.collider.CompareTag("Collider"))
            { 
                if (col.collider.GetComponent<BoxDestroyer>().directionDestroy == spawner.direction) 
                {
                            GameManager.instance.UpdateHp(-1);
                            DestroySquare(); 
                }
            }
        }
        
    }

    private void GenerateParticles()
    {
        ParticleSystem ps = Instantiate(particlesPrefab, transform.position,new Quaternion(0,0,180,0));
        ps.startColor = GetComponent<SpriteRenderer>().color;
    }

    private void AddPoint()
    {
        GameManager.instance.UpdatePoints(1);
    }

    private void TurnOffCollider()
    {
        if (GameManager.instance.IsScreenActive())
        {
            collider.enabled = false;
        }
        else
        {
            collider.enabled = true;
        }
    }

    private void Move()
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
                    case Movement.RotationYAxis:
                    {
                        MoveInDirection();
                        MoveAndRotateInYAxis();
                        break;
                    }
                    case Movement.RotationXAxis:
                    {
                        MoveInDirection();
                        MoveAndRotateInXAxis();
                        break;
                    }
                }
    }

    #region MakeChanges
    private void MakeChangesOnAxis()
    {
        if (spawner.xAxisMovement)
        {
            if (randomMovement == Movement.RotationYAxis)
            {
                randomMovement = Movement.RotationXAxis;
            }
        }
        else
        {
            if (randomMovement == Movement.RotationXAxis)
            {
                randomMovement = Movement.RotationYAxis;
            }
        }
    }
    
    private void MakeChangesOnGameMode()
    {
        if (GameManager.instance.gameMode == GameManager.GameMode.Chaos)
        {
            maxSpeed = 5f;
            minSpeed = 2f;
            
            if (spawner.xAxisMovement)
            {
                maxSpeed = 4.5f;
            }
            
            randomMovement = (Movement)Random.Range(0, 9);
            if (randomMovement == Movement.StopAndGo)
            {
                minSpeed = 2.5f;
            }
            else if(randomMovement == Movement.Bow ||
                    randomMovement == Movement.Fall)
            {
                if (spawner.xAxisMovement)
                {
                    maxSpeed = 3.5f;
                }
                else
                {
                    maxSpeed = 4f;
                }
            }
        }
        else
        {
            randomMovement = Movement.Straight;

            if (GameManager.instance.gameMode == GameManager.GameMode.Relax)
            {
                maxSpeed = 2;
            }
            else
            {
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
            }
        }
        
        if (isHeart)
        {
            randomMovement = Movement.RotationYAxis;
            
            if (spawner.xAxisMovement)
            {
                randomMovement = Movement.RotationXAxis;
            }
                        
            minSpeed = 3f;
            maxSpeed = 4.5f;
        }
        
        if (spawner.randomSquareSpeed)
        {
            speed = Random.Range(minSpeed, maxSpeed);
        }
    }

    private void MakeChangesOnMovement()
    {
        if (randomMovement == Movement.StopAndGo)
        {
            stopTime = Random.Range(0.5f, 1.5f);
            tempStopTime = stopTime;
        }
        else if (randomMovement == Movement.Obliquely)
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
                 randomMovement == Movement.ForwardAndBackward)
        {
            randomMagnitude = Random.Range(sinusMovementSpeedRange / 2, sinusMovementSpeedRange);
            randomHeight = Random.Range(sinusMovementSpeedRange / 2, sinusMovementSpeedRange);
        }
    }

    private void MakeChangesOnCollider()
    {
        if (speed > 1)
        {
            if (isFriend || isHeart)
            {
                collider.size = new Vector2(collider.size.x + 0.02f, collider.size.y + 0.02f);
                return;
            }
            
            collider.size = new Vector2(collider.size.x + speed / 10, collider.size.y + speed / 10);

        }
            
        if (randomMovement == Movement.Bow ||
            randomMovement == Movement.Fall ||
            randomMovement == Movement.Sinus ||
            randomMovement == Movement.ForwardAndBackward ||
            randomMovement == Movement.RotationXAxis ||
            randomMovement == Movement.RotationYAxis) 
        {
            if (CompareTag("Heart"))
            {
                return;
            }
            
            collider.size = new Vector2(collider.size.x + 0.3f, collider.size.y + 0.3f);
        }

    }
    
    #endregion

    #region DetectCollision
    private void DetectTouchWithSquare()
    {
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            foreach (Touch touch in Input.touches)
            {
                RaycastHit2D hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(touch.position));
                
                if(hit.collider != null)
                { 
                    if (hit.collider == collider) 
                    {
                        AudioManager.instance.Play("Explosion");
                        GenerateParticles();

                        if (isHeart)
                        {
                            GameManager.instance.UpdateHp(1);
                        }
                        else if (isFriend)
                        {
                            GameManager.instance.UpdateHp(-1);
                        }
                        else
                        {
                            AddPoint();
                        }
                        
                        DestroySquare(); 
                    }
                }
            }
        }
    }

    private void DetectMouseClickOnSquare()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
 
            if(hit.collider != null)
            { 
                if (hit.collider == collider) 
                {
                    AudioManager.instance.Play("Explosion");
                    GenerateParticles();

                    if (isHeart)
                    {
                        GameManager.instance.UpdateHp(1);
                        AddPoint();
                    }
                    else if (isFriend)
                    {
                        GameManager.instance.UpdateHp(-1);
                    }
                    else
                    {
                        AddPoint();
                    }
                    
                    DestroySquare(); 
                }
            }
        }
    }
    #endregion
    
    #region MovePattern
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
        move = new Vector3(math.sin(Time.time * randomMagnitude) * Time.deltaTime * randomHeight,
            math.sin(Time.time * randomMagnitude) * Time.deltaTime * randomHeight);
        
        transform.Translate(move);
    }

    private void MoveAndRotateInYAxis()
    {
        transform.Rotate(Vector3.down * rotationSpeed * Time.deltaTime);
    }

    private void MoveAndRotateInXAxis()
    {
        transform.Rotate(Vector3.left * rotationSpeed * Time.deltaTime);
    }
    #endregion
}