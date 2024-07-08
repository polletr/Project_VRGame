using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    [Header("Mixer")]
    [SerializeField] private AudioMixer mixer;

    [Header("Mixer Groups")]
    [SerializeField] private AudioMixerGroup masterGroup;
    [SerializeField] private AudioMixerGroup bgMusicGroup;
    [SerializeField] private AudioMixerGroup sfxGroup;

    [Header("Game Event")]
    public GameEvent gameEvent;

    private List<AudioSource> _audioSources = new();

    private void Awake()
    {
        foreach (AudioSource source in GetComponentsInChildren<AudioSource>())
        {
            _audioSources.Add(source);
        }
    }

    private void PlayAudio(AudioClip clip, bool isBgMusic)
    {
        AudioSource audioSource = GetAvailableAudioSource();

        if (isBgMusic) StopAllAudio();

        if (audioSource)
        {
            audioSource.outputAudioMixerGroup = isBgMusic ? bgMusicGroup : sfxGroup;
            audioSource.loop = isBgMusic;
            audioSource.clip = clip;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("No available audio sources");
        }
    }

    private AudioSource GetAvailableAudioSource()
    {
        foreach (AudioSource audioSource in _audioSources)
        {
            if (!audioSource.isPlaying)
            {
                return audioSource;
            }
        }
        return null;
    }

    public void StopAllAudio()
    {
        foreach (AudioSource audioSource in _audioSources)
        {
            audioSource.Stop();
        }
    }

    private void OnEnable()
    {
        gameEvent.PlayBGMusic.AddListener((clip) => PlayAudio(clip, true));
        gameEvent.PlayClip.AddListener((clip) => PlayAudio(clip, false));
    }

    private void OnDisable()
    {
        gameEvent.PlayBGMusic.RemoveListener((clip) => PlayAudio(clip, true));
        gameEvent.PlayClip.RemoveListener((clip) => PlayAudio(clip, false));
    }
}
