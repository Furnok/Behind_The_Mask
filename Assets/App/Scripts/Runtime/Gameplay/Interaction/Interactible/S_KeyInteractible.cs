using System;
using UnityEngine;

public class S_KeyInteractible : MonoBehaviour, IInteractable
{
    [Header("Settings")]
    [SerializeField] int _priority = 0;
    [SerializeField] bool _isInteractable = true;
    [SerializeField] private S_ClassAudio audioPickUp;

    [Header("References")]
    [SerializeField] GameObject _keyVisuals;
    [SerializeField] SphereCollider colliderkey;

    //[Header("Inputs")]

    [Header("Outputs")]
    [SerializeField] RSO_HaveKey _haveKey;
    [SerializeField] private RSE_OnUIInterract rseOnUIInterract;
    [SerializeField] private RSE_OnUIError rseOnUIError;
    [SerializeField] private RSE_PlayAudio rsePlayAudio;

    public int Priority => _priority;
    public Transform Transform => transform;
    public bool IsInteractable => _isInteractable;

    void Awake()
    {
        _haveKey.Value = false;
    }

    public void Interact()
    {
        if (!_isInteractable)
            return;

        _haveKey.Value = true;
        _isInteractable = false;
        _keyVisuals.SetActive(false);
        colliderkey.enabled = false;

        rseOnUIInterract.Call(false);
        rseOnUIError.Call("You picked up the Key!", Color.green);
        rsePlayAudio.Call(audioPickUp);
    }
}