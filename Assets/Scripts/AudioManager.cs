using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    AudioSource[] audioSourceArray;

    void Awake()
    {
        audioSourceArray = this.gameObject.GetComponentsInChildren<AudioSource>();
    }

    public void Play(AudioClip clip, float volume, Vector3 pos)
    {
        foreach(var audioSource in audioSourceArray)
        {
            if (audioSource.isPlaying)
            {
                continue;
            }
            audioSource.volume = volume;
            audioSource.transform.localPosition = pos;
            audioSource.PlayOneShot(clip);
            break;
        }
    }
}
