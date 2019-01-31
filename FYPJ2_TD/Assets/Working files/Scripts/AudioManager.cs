using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioSource m_audioSfx;
    public AudioSource m_audioSource;
    public AudioMixer m_audioMixer;
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

    void Start()
    {
        m_audioSource.Play();
        m_audioMixer.SetFloat("masterVolume", PlayerPrefs.GetFloat("masterVolume", 0));
        m_audioMixer.SetFloat("musicVolume", PlayerPrefs.GetFloat("musicVolume", 0));
        m_audioMixer.SetFloat("sfxVolume", PlayerPrefs.GetFloat("sfxVolume", 0));
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

    public void OnMusic()
    {
        if (m_audioSource.isPlaying)
        {
            m_audioSource.Stop();
        }
    }

    public void OffMusic()
    {
        if (!m_audioSource.isPlaying)
        {
            m_audioSource.Play();
        }
    }
}
