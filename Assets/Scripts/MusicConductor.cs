using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicConductor : MonoBehaviour
{
    private AudioSource audioSource;
    private float[] samples = new float[512];
    private float[] spectrum = new float[512];
    private float rmsValue;
    private float threshold = 0.02f; // Initial threshold
    public float thresholdMultiplier = 1.5f; // Multiplier to detect stronger beats

    public List<float> beatTimestamps = new List<float>();
    private int beatIndex = 0;

    private bool audioProcessed;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PreprocessAudio();
    }

    void Update()
    {
        if (audioProcessed)
            DetectBeat();
    }

    void PreprocessAudio()
    {
        int numSamples = audioSource.clip.samples;
        float[] clipSamples = new float[numSamples * audioSource.clip.channels];
        audioSource.clip.GetData(clipSamples, 0);

        for (int i = 0; i < numSamples; i += samples.Length)
        {
            int length = Mathf.Min(samples.Length, numSamples - i);
            System.Array.Copy(clipSamples, i, samples, 0, length);
            AnalyzeAudio();
            if (rmsValue > threshold)
            {
                beatTimestamps.Add(i / (float)audioSource.clip.frequency);
                threshold = rmsValue * thresholdMultiplier;
            }
            else
            {
                threshold *= 0.98f; // Slowly decrease the threshold over time
            }
        }

        audioProcessed = true;
        audioSource.Play();
    }

    void AnalyzeAudio()
    {
        int i;
        float sum = 0;
        for (i = 0; i < samples.Length; i++)
        {
            sum += samples[i] * samples[i];
        }
        rmsValue = Mathf.Sqrt(sum / samples.Length); // RMS = square root of average

        // Frequency analysis
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
        float maxV = 0;
        var maxN = 0;
        for (i = 0; i < spectrum.Length; i++)
        {
            if (!(spectrum[i] > maxV) || !(spectrum[i] > threshold))
                continue;

            maxV = spectrum[i];
            maxN = i; // maxN is the index of max
        }
        float pitchValue = maxN * 24000 / spectrum.Length; // convert index to frequency

        // Add pitch value influence to RMS
        rmsValue += pitchValue * 0.001f;
    }

    void DetectBeat()
    {
        if (beatIndex < beatTimestamps.Count && audioSource.time >= beatTimestamps[beatIndex])
        {
            Debug.Log("Spawn");
            beatIndex++;
        }
    }
}