using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class S_AudioManager : MonoBehaviour
{
    [TabGroup("Settings")]
    [Title("Parameters")]
    [SuffixLabel("s", Overlay = true)]
    [SerializeField] private float timeFadeInAudio;

    [TabGroup("Settings")]
    [SuffixLabel("s", Overlay = true)]
    [SerializeField] private float timeFadeOutAudio;

    [TabGroup("Settings")]
    [SerializeField, Range(0f, 1f)] private float volumeMax;

    [TabGroup("References")]
    [Title("Prefab")]
    [SerializeField] private GameObject audioSourcePrefab;

    [TabGroup("Inputs")]
    [SerializeField] private RSE_PlayAudio rsePlayAudio;

    [TabGroup("Inputs")]
    [SerializeField] private RSE_StopAllAudio rseStopAllAudio;

    [TabGroup("Inputs")]
    [SerializeField] private RSE_OnGamePause rseGamePause;

    private List<AudioSource> activeAudioSources = new();

    private void OnEnable()
    {
        rsePlayAudio.action += PlayAudio;
        rseStopAllAudio.action += StopAllAudios;
        rseGamePause.action += PauseAudios;
    }

    private void OnDisable()
    {
        rsePlayAudio.action -= PlayAudio;
        rseStopAllAudio.action -= StopAllAudios;
        rseGamePause.action -= PauseAudios;
    }

    private void PlayAudio(S_ClassAudio classAudio)
    {
        if (classAudio.clip == null) return;

        GameObject audioObj = Instantiate(audioSourcePrefab, classAudio.position, classAudio.rotation, gameObject.transform);
        audioObj.transform.SetParent(gameObject.transform);
        audioObj.name = $"Audio_{classAudio.clip.name}";

        AudioSource audioSource = audioObj.GetComponent<AudioSource>();
        audioSource.clip = classAudio.clip;
        audioSource.outputAudioMixerGroup = classAudio.mixerGroup;
        audioSource.loop = classAudio.loop;
        audioSource.spatialBlend = classAudio.is3D ? 1f : 0f;
        audioSource.minDistance = classAudio.minDistance;
        audioSource.maxDistance = classAudio.maxDistance;

        S_AudioSource script = audioObj.GetComponent<S_AudioSource>();
        script.Setup(this, classAudio.canPause, classAudio.loop);

        if (classAudio.fade)
        {
            FadeInAudio(audioSource);
        }
        else
        {
            audioSource.volume = volumeMax;

            audioSource.Play();
        }

        activeAudioSources.Add(audioSource);
    }

    private void FadeInAudio(AudioSource audioSource)
    {
        audioSource.volume = 0;

        audioSource.Play();

        audioSource.DOFade(volumeMax, timeFadeInAudio).SetEase(Ease.Linear).SetLink(audioSource.gameObject);
    }

    private void FadeOutAudio(AudioSource audioSource)
    {
        audioSource.DOFade(0, timeFadeOutAudio).SetEase(Ease.Linear).SetLink(audioSource.gameObject).OnComplete(() => StopAudio(audioSource));
    }

    private void StopAudio(AudioSource audioSource)
    {
        audioSource.Stop();

        UpdateList(audioSource);
    }

    public void UpdateList(AudioSource audioSource)
    {
        activeAudioSources.Remove(audioSource);
    }

    private void StopAllAudios()
    {
        foreach (AudioSource audioSource in activeAudioSources)
        {
            if (audioSource != null)
            {
                FadeOutAudio(audioSource);
            }
        }
    }

    private void PauseAudios(bool value)
    {
        foreach (AudioSource audioSource in activeAudioSources)
        {
            if (audioSource != null)
            {
                if (audioSource.GetComponent<S_AudioSource>().canPause)
                {
                    if (value) audioSource.Pause();
                    else audioSource.UnPause();
                }
            }
        }
    }
}