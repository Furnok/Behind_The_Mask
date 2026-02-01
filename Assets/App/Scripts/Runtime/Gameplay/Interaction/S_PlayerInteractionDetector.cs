using UnityEngine;

public class S_PlayerInteractionDetector : InteractionDetectorBase
{
    [Header("Settings")]
    [SerializeField] float _coneAngle = 60f;

    [Header("References")]
    [SerializeField] Transform _origin;

    [Header("Inputs")]
    [SerializeField] RSE_OnInteractInput _onInteractInput;

    [Header("Outputs")]
    [SerializeField] private RSE_OnUIInterract rseOnUIInterract;

    //[Header("Outputs")]

    bool _uiShown = false;

    private void OnEnable()
    {
        if (_onInteractInput != null)
            _onInteractInput.action += OnInteract;
    }

    private void OnDisable()
    {
        if (_onInteractInput != null)
            _onInteractInput.action -= OnInteract;
    }

    void OnInteract()
    {
        var o = _origin != null ? _origin : transform;
        RecalculateTarget(o.position, o.forward, _coneAngle);

        InteractCurrent();
    }

    private void OnTriggerEnter(Collider other)
    {
        TryAddInteractableFromCollider(other);

        rseOnUIInterract.Call(true);
    }

    private void OnTriggerExit(Collider other)
    {
        TryRemoveInteractableFromCollider(other);

        rseOnUIInterract.Call(false);
    }

    private void Update()
    {
        RefreshUI();
    }

    void RefreshUI()
    {
        var o = _origin != null ? _origin : transform;
        RecalculateTarget(o.position, o.forward, _coneAngle);

        bool shouldShow = _currentTarget != null;

        if (shouldShow == _uiShown) return;
        _uiShown = shouldShow;

        rseOnUIInterract.Call(shouldShow);
    }
}