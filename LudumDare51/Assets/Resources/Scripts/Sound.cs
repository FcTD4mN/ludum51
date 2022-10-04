using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
*   Apply this class for better
*   control on the sound properties
*/
public class Sound
{
    public string name;

    public AudioClip clip;
    public AudioSource source;

    [Range(0f, 1f)]
    public float volume;

    [Range(.1f, 3f)]
    public float pitch;
    [Range(-1f, 1f)]
    public float pane;
    public bool loop;
    public bool playOnAwake;

    // Ensures music isn’t played multiple times
    public bool mPlaying;

    public Sound(string name, float volume, float pitch, float pane, bool loop, bool playOnAwake, AudioClip clip)
    {
        this.name = name;
        this.volume = volume;
        this.pitch = pitch;
        this.loop = loop;
        this.playOnAwake = playOnAwake;
        this.pane = pane;
        this.clip = clip;
        this.mPlaying = false;
    }
}