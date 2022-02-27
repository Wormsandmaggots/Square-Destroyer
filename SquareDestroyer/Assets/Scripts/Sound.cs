using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public bool isLooping;

    [Range(0,1)]
    public float volume;
    
    public AudioClip clip;
    [HideInInspector]
    public AudioSource source;

}
