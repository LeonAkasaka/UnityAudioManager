using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioInstance : MonoBehaviour
{
    /// <summary>
    /// Gets a playing AudioClip.
    /// </summary>
    public AudioClip AudioClip { get; private set; }

    /// <summary>
    /// True if playing audio, otherwise false.
    /// </summary>
    public bool IsPlaying { get; private set; }

    /// <summary>
    /// Gets a audio category.
    /// </summary>
    public string Category { get; private set; }

    private float _terminationSec = 0;
    private bool _stopped = false;

    /// <summary>
    /// Play audio.
    /// </summary>
    /// <param name="clip"></param>
    public void Play(AudioClip clip) { Play(clip, null); }

    /// <summary>
    /// Stop current playing audio.
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="category"></param>
    public void Play(AudioClip clip, string category)
    {
        if (IsPlaying) { throw new System.InvalidOperationException(); }

        AudioClip = clip;
        Category = category;
        StartCoroutine(IterateAudioPlaying());
    }

    /// <summary>
    /// Stop current audio.
    /// </summary>
    public void Stop() { Stop(0); }

    /// <summary>
    /// Stop current playing audio.
    /// </summary>
    /// <param name="sec">Fade time.</param>
    public void Stop(float sec)
    {
        _terminationSec = sec;
        _stopped = true;
    }

    private IEnumerator IterateAudioPlaying()
    {
        IsPlaying = true;
        gameObject.name = AudioClip.name;
        audio.clip = AudioClip;
        audio.Play();

        while (!_stopped && audio.isPlaying)
        {
            yield return null;
        }

        yield return StartCoroutine(IterateTermination());

        gameObject.name = "None";
        audio.Stop();
        IsPlaying = false;
    }

    private IEnumerator IterateTermination()
    {
        if (_terminationSec == 0)
        {
            yield break;
        }

        var t = 0F;
        var v = audio.volume;
        while (t < _terminationSec)
        {
            var p = 1 - _terminationSec / (_terminationSec - t);
            audio.volume = v * p;
            t += Time.deltaTime;
            yield return null;
        }
    }
}