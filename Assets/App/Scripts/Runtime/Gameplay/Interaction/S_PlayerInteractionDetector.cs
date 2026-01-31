using UnityEngine;

public class S_PlayerInteractionDetector : InteractionDetectorBase
{
    //[Header("Settings")]

    //[Header("References")]

    [Header("Inputs")]
    [SerializeField] RSE_OnInteractInput _onInteractInput;

    //[Header("Outputs")]

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
        RecalculateTarget(transform.position);
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
        RecalculateTarget(transform.position);
    }
}