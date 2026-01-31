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

    public bool canPause = true;

    [Title("Position")]
    public Vector3 position = Vector3.zero;

    public Quaternion rotation = Quaternion.identity;

    [Title("3D")]
    public bool is3D = false;

    [ShowIf("Is3D")]
    [OnValueChanged(nameof(ClampDistanceMin))]
    public float minDistance = 0;

    [ShowIf("Is3D")]
    [OnValueChanged(nameof(ClampDistanceMax))]
    public float maxDistance = 500f;

    private bool Is3D => is3D == true;

    private void ClampDistanceMin()
    {
        minDistance = Math.Max(0f, minDistance);
    }

    private void ClampDistanceMax()
    {
        maxDistance = Math.Max(0f, maxDistance);
    }
}