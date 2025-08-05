using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    [Range(.1f, 3f)] public float pitchMin;
    [Range(.1f, 3f)] public float pitchMax;

    public bool loop;

    /*[Range(0f,1f)] public float volume;
    

    

    [HideInInspector]
    public AudioSource source;*/
}
