using UnityEngine;

public class S_PlayerInteractionDetector : InteractionDetectorBase
{
    [Header("Settings")]
    [SerializeField] float _coneAngle = 60f;
    [SerializeField] float _pitchAngle = 50f;

    [Header("References")]
    [SerializeField] Transform _origin;

    [Header("Inputs")]
    [SerializeField] RSE_OnInteractInput _onInteractInput;
    [SerializeField] RSE_OnUIInterract _onUIInterract;

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
        RecalculateTarget(o.position, o.forward, _coneAngle, _pitchAngle);

        InteractCurrent();
    }

    private void OnTriggerEnter(Collider other)
    {
        TryAddInteractableFromCollider(other);
    }

    private void OnTriggerExit(Collider other)
    {
        TryRemoveInteractableFromCollider(other);
    }

    private void Update()
    {
        RefreshUI();
    }

    void RefreshUI()
    {
        var o = _origin != null ? _origin : transform;
        RecalculateTarget(o.position, o.forward, _coneAngle, _pitchAngle);

        bool shouldShow = _currentTarget != null;

        if (shouldShow == _uiShown) return;
        _uiShown = shouldShow;

        _onUIInterract.Call(shouldShow);
    }
}