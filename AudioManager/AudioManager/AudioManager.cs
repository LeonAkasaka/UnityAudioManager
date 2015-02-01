using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Audio Manager.
/// </summary>
public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _audioInstanceSource = null;
    private GameObject Prefab { get { return _audioInstanceSource; } }

    public IEnumerable<AudioInstance> MusicInstances { get { return _musicInstances; } }
    private List<AudioInstance> _musicInstances = new List<AudioInstance>();

    private void Start()
    {
        StartCoroutine(IterateAudioManager());
    }

    private IEnumerator IterateAudioManager()
    {
        while (true)
        {
            var garbage = MusicInstances.Where(x => !x.IsPlaying).ToArray();
            foreach (var i in garbage)
            {
                _musicInstances.Remove(i);
                GameObject.Destroy(i.gameObject);
            }

            yield return new WaitForSeconds(1);
        }
    }

    /// <summary>
    /// Play AudioClip as a BGM.
    /// </summary>
    /// <param name="bgm">AudioClip.</param>
    /// <returns>AudioInstance.</returns>
    public AudioInstance PlayBgw(AudioClip bgm)
    {
        const string BGM = "BGM";

        var current = _musicInstances.FirstOrDefault(x => x.AudioClip == bgm && x.Category == BGM);
        if (current != null)
        {
            return current;
        }

        var ai = Play(bgm, BGM);
        _musicInstances.Add(ai);
        return ai;
    }

    /// <summary>
    /// Play AudioClip as a sound effect.
    /// </summary>
    /// <param name="se">AudioClip.</param>
    /// <returns>AudioInstance.</returns>
    public AudioInstance PlayEffect(AudioClip se)
    {
        return Play(se, "SoundEffec");
    }

    private AudioInstance Play(AudioClip audioClip, string category)
    {
        var ai = GetAudioInstance();
        ai.Play(audioClip, category);
        return ai;
    }

    private AudioInstance GetAudioInstance()
    {
        var obj = GameObject.Instantiate(Prefab) as GameObject;
        obj.transform.parent = transform;
        var ai = obj.GetComponent<AudioInstance>();
        return ai;
    }
}
