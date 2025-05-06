using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    private AudioSource audioSource;

    // Clips para cada escena
    public AudioClip menuMusic;
    public AudioClip level1Music;
    public AudioClip level2Music;
    public AudioClip levelSelectMusic;

    void Awake()
    {
        // Singleton para que no se destruya entre escenas
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource = GetComponent<AudioSource>();
            audioSource.loop = true;

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeMusic(AudioClip newClip)
    {
        if (audioSource.clip == newClip || newClip == null)
            return;

        audioSource.clip = newClip;
        audioSource.Play();
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }

    // Cuando se carga una escena
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "MainMenu":
                ChangeMusic(menuMusic);
                break;
            case "LevelSelect":
                ChangeMusic(levelSelectMusic);
                break;
            case "Level1Test":
                ChangeMusic(level1Music);
                break;
            case "Level2Test":
                ChangeMusic(level2Music);
                break;
            case "Level3Test":
                ChangeMusic(level2Music);
                break;
            case "Level4Test":
                ChangeMusic(level2Music);
                break;

            default:
                break;
        }
    }
}

