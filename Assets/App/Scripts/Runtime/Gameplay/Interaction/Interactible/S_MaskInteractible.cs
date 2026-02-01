using UnityEngine;

public class S_MaskInteractible : MonoBehaviour, IInteractable
{
    [Header("Settings")]
    [SerializeField] int _priority = 0;
    [SerializeField] int index = 0;
    [SerializeField] bool _isInteractable = true;
    [SerializeField] Mask _maskType;
    [SerializeField] SphereCollider colliderMask;

    [Header("References")]
    [SerializeField] GameObject _maskVisuals;

    //[Header("Inputs")]

    [Header("Outputs")]
    [SerializeField] RSE_OnPickUpMask _onPickUpMask;
    [SerializeField] private RSE_OnUIInterract rseOnUIInterract;
    [SerializeField] private RSE_OnUIError rseOnUIError;

    public int Priority => _priority;
    public Transform Transform => transform;
    public bool IsInteractable => _isInteractable;

    public void Interact()
    {
        Debug.Log("Picked up mask: " + _maskType.ToString());
        if (_isInteractable == true)
        {
            _onPickUpMask.Call(_maskType);
            _isInteractable = false;
            _maskVisuals.SetActive(false);
            colliderMask.enabled = false;
            rseOnUIInterract.Call(false);
            rseOnUIError.Call("You picked up the Mask " + index.ToString() + "!", Color.green);
        }
    }
}