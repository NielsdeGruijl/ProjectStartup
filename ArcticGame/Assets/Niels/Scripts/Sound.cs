using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound : MonoBehaviour
{
    public string name;

    public AudioClip clip;

    public float volume;

    public bool loop;

    public SoundType type;

    [HideInInspector] public AudioSource source;

    public enum SoundType
    {
        SFX,
        Music
    }
}
