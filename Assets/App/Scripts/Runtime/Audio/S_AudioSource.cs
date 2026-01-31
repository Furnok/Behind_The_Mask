using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

public class S_AudioSource : MonoBehaviour
{
    [TabGroup("References")]
    [Title("Audio")]
    [SerializeField] private AudioSource audioSource;

    private S_AudioManager currentAudioManager = null;

    private void OnEnable()
    {
        StartCoroutine(S_Utils.DelayFrame(() => StartCoroutine(AutoDestroyAfterAudio())));
    }

    public void Setup(S_AudioManager audioManager)
    {
        currentAudioManager = audioManager;
    }

    private IEnumerator AutoDestroyAfterAudio()
    {
        yield return new WaitForSeconds(audioSource.clip.length);

        currentAudioManager.UpdateList(audioSource);

        Destroy(gameObject);
    }
}