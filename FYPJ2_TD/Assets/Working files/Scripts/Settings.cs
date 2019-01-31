using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    public GameObject m_audioManager;
    public AudioSource m_audioSource;
    public AudioMixer m_audioMixer;
    public GameObject pauseMenuUI;
    public Slider masterSlider, musicSlider, sfxSlider;
    private bool isPaused = false;

    // Use this for initialization
    void Awake()
    {
        m_audioManager = GameObject.Find("AudioManager");
        m_audioSource = m_audioManager.GetComponent<AudioSource>();

        if (PlayerPrefs.HasKey("masterVolume"))
            masterSlider.value = PlayerPrefs.GetFloat("masterVolume");

        if (PlayerPrefs.HasKey("musicVolume"))
            musicSlider.value = PlayerPrefs.GetFloat("musicVolume");

        if (PlayerPrefs.HasKey("sfxVolume"))
            sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
    }

    public void Pause()
    {
        isPaused = !isPaused;
        if (isPaused)
            ActivePauseMenu();
        else
        {
            DeactivePauseMenu();
        }
    }

    public void ActivePauseMenu()
    {
        Time.timeScale = 0;
        pauseMenuUI.SetActive(true);
    }

    public void DeactivePauseMenu()
    {
        isPaused = false;
        Time.timeScale = 1;
        pauseMenuUI.SetActive(false);
    }

    public void OnButton()
    {
        AudioManager.instance.OnMusic();
    }
    public void OffButton()
    {
        AudioManager.instance.OffMusic();
    }

    public void SetMasterVolume(float volume)
    {
        m_audioMixer.SetFloat("masterVolume", volume);
        PlayerPrefs.SetFloat("masterVolume", volume);
        PlayerPrefs.Save();
    }

    public void SetMusicVolume(float volume)
    {
        m_audioMixer.SetFloat("musicVolume", volume);
        PlayerPrefs.SetFloat("musicVolume", volume);
        PlayerPrefs.Save();
    }

    public void SetSfxVolume(float volume)
    {
        m_audioMixer.SetFloat("sfxVolume", volume);
        PlayerPrefs.SetFloat("sfxVolume", volume);
        PlayerPrefs.Save();
    }

    public void QuitButton()
    {
        SceneManager.LoadScene("LoginScene");
    }

}
