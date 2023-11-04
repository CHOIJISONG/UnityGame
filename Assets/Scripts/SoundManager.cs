using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public AudioSource bgSound;
    public AudioClip[] bglist;
    public static SoundManager instance;

    public AudioSource musicSource, sfxSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        for (int i = 0; i < bglist.Length; i++)
        {
            if (arg0.name == bglist[i].name)
                BgSoundPlay(bglist[i]);
        }
    }

    public void BgSoundPlay(AudioClip clip)
    {
        bgSound.clip = clip;
        bgSound.loop = true;
        bgSound.volume = 0.1f;
        bgSound.Play();
    }

    public void PlaySFX(string sfxName, AudioClip clip)
    {
        GameObject go = new GameObject(sfxName + "Sound");
        AudioSource audiosource = go.AddComponent<AudioSource>();
        audiosource.clip = clip;
        audiosource.Play();

        Destroy(go, clip.length);
    }

    public void ToggleMusic()
    {
        bgSound.mute = !bgSound.mute;
    }

    public void ToggleSFX()
    {
        sfxSource.mute = !sfxSource.mute;
    }

    public void MusicVolume(float volume)
    {
        bgSound.volume = volume;
    }

    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}