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
    public float destroyCooldown;

    void Start()
    {
        spawner = GetComponentInParent<Spawner>();
        GetComponent<SpriteRenderer>().color = new Color(Random.value, Random.value, Random.value);
    }

    // Update is called once per frame
    void Update()
    {
        MoveSquare();

        destroyCooldown -= Time.deltaTime;

        if (destroyCooldown <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void MoveSquare()
    {
        Vector3 move = spawner.xAxisMovement ? Vector3.right : Vector3.up;

        move.x = move.x * (float)spawner.direction * speed * Time.deltaTime;
        move.y = move.y * (float)spawner.direction / 2f * speed * Time.deltaTime;
        
        transform.Translate(move);
    }

    private void OnMouseDown()
    {
        Instantiate(particlesPrefab, transform.position,quaternion.identity);
        GameManager.instance.UpdatePoints((int)speed);
        Destroy(gameObject);
    }
}
