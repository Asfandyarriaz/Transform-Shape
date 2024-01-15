using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("------- Audio Source -------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    [Header("------- Audio Clip -------")]
    [SerializeField] public AudioClip background;
    [SerializeField] public AudioClip changeShape;
    [SerializeField] public AudioClip onButtonClick;
    [SerializeField] public AudioClip objectBreakSound;
    [SerializeField] public AudioClip lose;

    private void Awake()
    {
        Instance = this;
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
    }
    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnGameStateChanged;
    }

    //TODO: Get current level from save system
    void GameManagerOnGameStateChanged(GameManager.GameState state)
    {
        if (state == GameManager.GameState.Lose)
        {
            PlayMusic(lose);
        }
        if (state == GameManager.GameState.Win)
        {

        }
    }

    private void Start()
    {
        MuteMusic(!PlayerDataController.Instance.playerData.isMusicAllow);
        MuteSound(!PlayerDataController.Instance.playerData.isSoundAllow);

        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void MuteMusic(bool mute)
    {
        if (mute)
            musicSource.mute = true;
        else
            musicSource.mute = false;
    }
    public void MuteSound(bool mute)
    {
        if (mute)
            sfxSource.mute = true;
        else
            sfxSource.mute = false;
    }
}
