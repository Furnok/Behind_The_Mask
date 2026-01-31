using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
public class S_ClassAudio
{
    [Title("Audio")]
    public AudioClip clip = null;

    public AudioMixerGroup mixerGroup = null;

    [Title("Parameters")]
    public bool fade = false;

    public bool loop = false;
}