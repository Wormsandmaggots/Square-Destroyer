using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitInScreen : MonoBehaviour
{
    private float   designOrthographicSize;
    private float   designAspect;
    private float   designWidth;

    public  float   designAspectHeight;
    public  float   designAspectWidth;

    public void Awake()
    {
        designOrthographicSize = Camera.main.orthographicSize;
        designAspect = designAspectHeight / designAspectWidth;
        designWidth = designOrthographicSize * designAspect;

        Resize();
    }

    public void Resize()
    {       
        float wantedSize = designWidth / Camera.main.aspect;
        Camera.main.orthographicSize = Mathf.Max(wantedSize, designOrthographicSize);
    }
}
