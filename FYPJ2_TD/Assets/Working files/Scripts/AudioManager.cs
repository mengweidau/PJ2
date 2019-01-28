using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource m_audioSfx;
    public AudioSource m_audioSource;
    public AudioClip Shoot;
    public static AudioManager instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void PlayAudio(AudioClip clip)
    {
        m_audioSfx.clip = clip;
        m_audioSfx.Play();
    }

    public void PlayShoot()
    {
        m_audioSfx.clip = Shoot;
        m_audioSfx.Play();
    }
}
