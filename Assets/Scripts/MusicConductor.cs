using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicConductor : MonoBehaviour
{
    public float thresholdDB = -3f;
    [SerializeField]
    private float cooldownDuration = 1f; // Cooldown duration in seconds
    private float lastSpawnTimestamp = -Mathf.Infinity; 

    private float timer;

    [SerializeField]
    private GameEvent gameEvent;

    [SerializeField] private AudioClip songClip;
    [SerializeField] private AudioClip beatClip;

    [SerializeField]
    private AudioSource musicSource;
    [SerializeField]
    private AudioSource beatTarget;

    private void Start()
    {
        musicSource.clip = songClip;
        beatTarget.clip = beatClip;
        StartCoroutine(SetReadyAfterDelay(3f));
    }

    void Update()
    {
        if (beatTarget.isPlaying)
        {
            // Calculate playback time
            float[] spectrumData = new float[1024]; // Example buffer size
            beatTarget.GetSpectrumData(spectrumData, 0, FFTWindow.Rectangular); // Get audio spectrum data

            float maxAmplitude = 0f;

            // Calculate maximum amplitude in current spectrum data
            for (int i = 0; i < spectrumData.Length; i++)
            {
                if (spectrumData[i] > maxAmplitude)
                {
                    maxAmplitude = spectrumData[i];
                }
            }

            // Convert amplitude to dB
            float amplitudeDB = 20 * Mathf.Log10(maxAmplitude);

            // Check if amplitude exceeds threshold
            if (amplitudeDB > thresholdDB && Time.time >= lastSpawnTimestamp + cooldownDuration)
            {
                Debug.Log("Amplitude in dB: " + amplitudeDB); // Debug log to check the amplitude value
                gameEvent.OnSpawn.Invoke();
                lastSpawnTimestamp = Time.time; // Update the last spawn timestamp
            }
        }
    }


    IEnumerator SetReadyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        beatTarget.Play();
        musicSource.Play();
        Debug.Log("Ready is now true");
    }

}