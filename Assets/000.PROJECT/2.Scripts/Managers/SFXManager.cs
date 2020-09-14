using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    private new AudioSource audio;

    void Awake()
    {
        audio = GetComponent<AudioSource>();
    }

    public void SetSFXSound(float volume, bool isMute)
    {
        audio.volume = volume;
        audio.mute = isMute;
    }

    public void SoundSfx(AudioClip clip, Vector3 pos)
    {
        if (audio.mute)
            return;

        StartCoroutine(PlaySfx(clip, pos));
    }

    public void SoundSfx(AudioClip clip)
    {
        if (audio.mute)
            return;

        StartCoroutine(PlaySfx(clip));
    }

    private IEnumerator PlaySfx(AudioClip clip)
    {
        GameObject _obj = new GameObject("sfx");

        AudioSource _audioSource = _obj.AddComponent<AudioSource>();

        _audioSource.clip = clip;
        _audioSource.volume = audio.volume;
        _audioSource.Play();
        Destroy(_obj, clip.length + 0.2f);

        yield return null;
    }

    private IEnumerator PlaySfx(AudioClip clip, Vector3 pos)
    {
        GameObject _obj = new GameObject("sfx");

        _obj.transform.position = pos;
        AudioSource _audioSource = _obj.AddComponent<AudioSource>();

        _audioSource.clip = clip;
        _audioSource.volume = audio.volume;
        _audioSource.spatialBlend = 1f;
        _audioSource.minDistance = 5f;
        _audioSource.maxDistance = 15f;
        _audioSource.Play();
        Destroy(_obj, clip.length + 0.2f);

        yield return null;
    }
}
