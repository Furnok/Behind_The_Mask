using UnityEngine;

public class S_KeyInteractible : MonoBehaviour, IInteractable
{
    [Header("Settings")]
    [SerializeField] int _priority = 0;
    [SerializeField] bool _isInteractable = true;

    [Header("References")]
    [SerializeField] GameObject _keyVisuals;

    //[Header("Inputs")]

    [Header("Outputs")]
    [SerializeField] RSO_HaveKey _haveKey;
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

    }
}