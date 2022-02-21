using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyIfDuplicate : MonoBehaviour
{

    private static GameObject thisObject;

    void Awake() 
    {
        if(thisObject != null)
        {
            Destroy(gameObject);
            return;
        }
        
        thisObject = gameObject;
    }
    
}
