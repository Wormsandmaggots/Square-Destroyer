using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public GameObject squarePrefab;
    
    // in which direction square should move
    public Direction direction;
    public enum Direction
    {
        Left = -1,
        Right = 1,
        Up = 2,
        Down = -2
    }

    public float spawningCooldown;
    private float tempCooldown;
    public float positionRange;

    public bool randomSquareSpeed;

    // if spawned squares move over X Axis
    public bool xAxisMovement = false;
    
    void Start()
    {
        if (GameManager.instance.gameMode == GameManager.GameMode.Chaos)
            spawningCooldown = 3;
        tempCooldown = spawningCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.start)
        {
            if (tempCooldown <= 0)
            {
                CreateSquare(); 
                tempCooldown = spawningCooldown;
            }
            
            tempCooldown -= Time.deltaTime;
        }
    }

    private void CreateSquare()
    {
        GameObject square = Instantiate(squarePrefab);

        square.transform.parent = gameObject.transform;

        if (xAxisMovement)
        {
            square.transform.position = new Vector3(gameObject.transform.position.x, Random.Range(-positionRange, positionRange) + gameObject.transform.position.y);
        }
        else
        {
            square.transform.position = new Vector3(Random.Range(-positionRange, positionRange) + gameObject.transform.position.x, gameObject.transform.position.y);
        }
    }
}
