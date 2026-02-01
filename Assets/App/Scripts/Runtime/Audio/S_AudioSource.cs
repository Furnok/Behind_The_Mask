using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

public class S_AudioSource : MonoBehaviour
{
    [TabGroup("References")]
    [Title("Audio")]
    [SerializeField] private AudioSource audioSource;

    [HideInInspector] public bool canPause = true;
    [HideInInspector] public bool islooping = false;

    private S_AudioManager currentAudioManager = null;

    public void Setup(S_AudioManager audioManager, bool value, bool value2)
    {
        currentAudioManager = audioManager;
        canPause = value;
        islooping = value2;

        if (!islooping)
        {
            StartCoroutine(S_Utils.DelayFrame(() => StartCoroutine(AutoDestroyAfterAudio())));
        }
    }

    private IEnumerator AutoDestroyAfterAudio()
    {
        if (canPause) yield return new WaitForSeconds(audioSource.clip.length);
        else yield return new WaitForSecondsRealtime(audioSource.clip.length);

        currentAudioManager.UpdateList(audioSource);

        Destroy(gameObject);
    }
}