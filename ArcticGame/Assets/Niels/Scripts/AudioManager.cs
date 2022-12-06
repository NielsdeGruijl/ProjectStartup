using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager manager;

    [SerializeField] List<Sound> sounds = new List<Sound>();

    public float SFXVolume = 1;
    public float MusicVolume = 1;

    private void Awake()
    {
        if (manager != null && manager != this)
            Destroy(gameObject);
        else
            manager = this;

        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.playOnAwake = false;
        }

        AdjustSoundValues();
    }

    private void Update()
    {
        AdjustSoundValues();

        foreach (Sound sound in sounds)
        {
            sound.source.volume = sound.volume;
        }
    }

    public void PlayAudio(string clipName)
    {
        foreach (Sound sound in sounds)
        {
            if (sound.name == clipName)
            {
                if (!sound.loop)
                {
                    sound.source.loop = false;
                    sound.source.PlayOneShot(sound.clip);
                }
                else if (sound.loop)
                {
                    sound.source.loop = true;
                    sound.source.clip = sound.clip;
                    sound.source.Play();
                }
            }
        }
    }

    private void AdjustSoundValues()
    {
        foreach (Sound sound in sounds)
        {
            if (sound.type == Sound.SoundType.SFX)
                sound.volume *= SFXVolume;

            if (sound.type == Sound.SoundType.Music)
                sound.volume *= MusicVolume;
        }
    }
}
