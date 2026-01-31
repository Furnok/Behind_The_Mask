using UnityEngine;

public class S_DoorInteractible : MonoBehaviour, IInteractable
{
    [Header("Settings")]
    [SerializeField] int _priority = 0;
    [SerializeField] bool _isInteractable = true;
    [SerializeField] Mask _requiredMask;

    [Header("References")]
    [SerializeField] GameObject _doorVisuals;

    //[Header("Inputs")]

    [Header("Outputs")]
    [SerializeField] RSO_PlayerCurrentMaskEquipped _playerCurrentMaskEquipped;

    public int Priority => _priority;
    public Transform Transform => transform;
    public bool IsInteractable => _isInteractable;

    public void Interact()
    {
        if(_playerCurrentMaskEquipped.Value == _requiredMask)
        {
            // Logic to open the door and animation
            Debug.Log("Door opened with mask: " + _requiredMask.ToString());
            _doorVisuals.SetActive(false); // Simulate door opening by disabling visuals for now
        }
    }
}