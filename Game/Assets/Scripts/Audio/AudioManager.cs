using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    [SerializeField]
    Sound[] sounds;
    [SerializeField]
    AudioClip[] backgroundMusic;
    [SerializeField]
    AudioSource music;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        foreach (Sound s in sounds)
        {
            s.Source = gameObject.AddComponent<AudioSource>();
            s.Source.clip = s.clips[0];
            s.Source.volume = s.volume;
            s.Source.pitch = s.pitch;
            s.Source.loop = s.loop;
        }
    }

    public void Update()
    {
        PlayMusic();
    }

    public void Play(string sound, bool isOneShot = false)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        if (s.Source.isPlaying == false && isOneShot == false)
        {
            ShuffleAudioSettings(ref s);
            s.Source.Play();
        }

        if (isOneShot)
        {
            ShuffleAudioSettings(ref s);
            s.Source.PlayOneShot(s.Source.clip);
        }
    }

    private void ShuffleAudioSettings(ref Sound s)
    {
        s.Source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
        s.Source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));
        s.Source.clip = s.clips[UnityEngine.Random.Range(0, s.clips.Length)];
    }

    public void Stop(string sound, float delayTimeSeconds = 0)
    {
        if (delayTimeSeconds > 0)
        {
            StartCoroutine(StopInSeconds(sound, delayTimeSeconds));
            return;
        }

        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.Source.Stop();
    }

    IEnumerator StopInSeconds(string sound, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Stop(sound);
    }

    private void PlayMusic()
    {
        if (music.isPlaying == false)
        {
            music.clip = backgroundMusic[UnityEngine.Random.Range(0, backgroundMusic.Length)];
            music.Play();
        }
    }

}
