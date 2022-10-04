using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    private List<Sound> sounds;

    // Load all user prefs.
    public void Initialize()
    {
        // Load();
        sounds = new List<Sound>();
        LoadAllSounds();
    }


    private void LoadAllSounds()
    {
        // Retrieve all audio file from 'Resources/Sounds' folder.
        Object[] audios = Resources.LoadAll("Sounds", typeof(AudioClip));

        foreach (Object a in audios)
        {
            float vol = 0.7f;
            if (a.name == "laser")
                vol = 0.1f;

            Sound s = new Sound(a.name, vol, 1f, 0f, false, false, a as AudioClip);
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.panStereo = s.pane;
            s.source.loop = s.loop;
            s.source.playOnAwake = s.playOnAwake;
            sounds.Add(s);
        }
    }

    /*
    *   Handle Playing Sounds
    *   Based on Actions
    */

    public void Play(string name)
    {
        Sound sound = sounds.Find(s => s.name.Contains(name));

        if (sound != null)
            sound.source.Play();
        else
            Debug.Log("FOR WHATEVER REASON, FILE DOESNT EXISTS");
    }

    public void StartSound(string name, bool loop)
    {
        Sound sound = sounds.Find(s => s.name.Contains(name));

        if (sound != null)
        {
            // if (sound.mPlaying)
            // StopSound(name);

            sound.source.Play();
            sound.source.loop = loop;
            sound.mPlaying = true;
        }
    }

    public void StopSound(string name)
    {
        Sound sound = sounds.Find(s => s.name.Contains(name));

        if (sound != null && sound.mPlaying)
        {
            sound.source.Stop();
            sound.mPlaying = false;
        }
    }
}
