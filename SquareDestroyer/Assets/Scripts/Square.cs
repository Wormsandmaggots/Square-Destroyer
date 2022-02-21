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

    private BoxCollider2D collider;

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
        ParticleSystem ps = Instantiate(particlesPrefab, transform.position,new Quaternion(0,0,180,0));
        ps.startColor = GetComponent<SpriteRenderer>().color;
        
        GameManager.instance.UpdatePoints((int)speed);
        
        Destroy(gameObject);
    }
}
