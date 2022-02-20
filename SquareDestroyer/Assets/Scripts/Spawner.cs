using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public GameObject squarePrefab;
    
    public Direction direction;
    public enum Direction
    {
        Left = -1,
        Right = 1,
        Up = 1,
        Down = -1
    }

    public float spawningCooldown;
    private float tempCooldown;
    public float positionRange;

    public bool xAxisMovement;
    
    void Start()
    {
        tempCooldown = spawningCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if (tempCooldown <= 0)
        {
            CreateSquare();
            tempCooldown = spawningCooldown;
        }

        tempCooldown -= Time.deltaTime;
    }

    private void CreateSquare()
    {
        Instantiate(squarePrefab, xAxisMovement ? new Vector3(Random.Range(-positionRange, positionRange), 0) : new Vector3(0, Random.Range(-positionRange, positionRange)),new Quaternion());
    }
    
}
