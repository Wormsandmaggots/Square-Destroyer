using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateParticles : MonoBehaviour
{
    private ParticleSystem ps;
    public float destroyCooldown;    
    void Start()
    {
        gameObject.transform.parent = GameObject.Find("ParticlesManager").transform;
        
        ps = GetComponent<ParticleSystem>();
        
        ps.Play();
        Destroy(gameObject,destroyCooldown);
    }
}
