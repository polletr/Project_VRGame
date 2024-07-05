using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{


    [Header("Mixer")]
    [SerializeField] private AudioMixer Mixer;

    [Header("Mixer Groups")]
    [SerializeField] private AudioMixerGroup MasterG;
    [SerializeField] private AudioMixerGroup BgMusicG;
    [SerializeField] private AudioMixerGroup SfxG;

    [Header("Game Event")]
    public GameEvent Event;

    private List<AudioSource> _allAudioSources = new();
    private List<AudioSource> _availableAudioSources = new();

    private void Awake()
    {
        foreach (AudioSource source in GetComponentsInChildren<AudioSource>())
        {
            _allAudioSources.Add(source);
            _availableAudioSources.Add(source);
        }
    }

    private void PlayBGMusic(AudioClip clip)
    {
        AudioSource audioSource = GetAvailableAudioSource();
        if (audioSource)
        {
            audioSource.outputAudioMixerGroup = BgMusicG;
            audioSource.loop = true;
            audioSource.clip = clip;
            audioSource.Play();
        }
        else
            Debug.LogWarning("No available audio sources");
    }

    private void PlayClip(AudioClip clip)
    {
        AudioSource audioSource = GetAvailableAudioSource();
        if (audioSource)
        {
            _availableAudioSources.Remove(audioSource);
            audioSource.outputAudioMixerGroup = SfxG;
            audioSource.PlayOneShot(clip);
            audioSource.outputAudioMixerGroup = MasterG;
            _availableAudioSources.Add(audioSource);
        }
        else
            Debug.LogWarning("No available audio sources");
    }

    private AudioSource GetAvailableAudioSource()
    {
        foreach (AudioSource audioSource in _availableAudioSources)
        {
            if (!audioSource.isPlaying)
            {
                _availableAudioSources.Remove(audioSource);
                return audioSource;
            }
        }
        return null;
    }

    private void StopAllAudio()
    {
        foreach (AudioSource audioSource in _allAudioSources)
        {
            audioSource.Stop();
        }
    }

    private void OnEnable()
    {
        Event.PlayBGMusic.AddListener(PlayBGMusic);
        Event.PlayClip.AddListener(PlayClip);
    
    }

    private void OnDisable()
    {
        Event.PlayBGMusic.RemoveListener(PlayBGMusic);
        Event.PlayClip.RemoveListener(PlayClip);
    }
}
