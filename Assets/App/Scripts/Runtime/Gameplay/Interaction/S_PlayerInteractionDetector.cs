using UnityEngine;

public class S_PlayerInteractionDetector : InteractionDetectorBase
{
    //[Header("Settings")]

    //[Header("References")]

    [Header("Inputs")]
    [SerializeField] RSE_OnInteractInput _onInteractInput;

    [Header("Outputs")]
    [SerializeField] private RSE_OnUIInterract rseOnUIInterract;

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

        rseOnUIInterract.Call(true);
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("t");

        TryRemoveInteractableFromCollider(other);

        rseOnUIInterract.Call(false);
    }

    private void Update()
    {
        RecalculateTarget(transform.position);
    }
}