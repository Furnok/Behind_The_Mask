using UnityEngine;

public class S_DoorInteractible : MonoBehaviour, IInteractable
{
    [Header("Settings")]
    [SerializeField] int _priority = 0;
    [SerializeField] bool _isInteractable = true;

    //[Header("References")]

    //[Header("Inputs")]

    //[Header("Outputs")]

    public int Priority => _priority;
    public Transform Transform => transform;
    public bool IsInteractable => _isInteractable;

    public void Interact()
    {

    }
}