using UnityEngine;

public class S_WinObjectInteractible : MonoBehaviour,  IInteractable
{
    [Header("Settings")]
    [SerializeField] int _priority = 0;
    [SerializeField] bool _isInteractable = true;

    //[Header("References")]

    //[Header("Inputs")]

    [Header("Outputs")]
    [SerializeField] private RSE_OnUIInterract rseOnUIInterract;
    [SerializeField] RSE_OnGameWin rseOnGameWin;

    public int Priority => _priority;
    public Transform Transform => transform;
    public bool IsInteractable => _isInteractable;

    public void Interact()
    {
        if (!_isInteractable)
            return;

        _isInteractable = false;

        rseOnUIInterract.Call(false);
        rseOnGameWin.Call();
        Debug.Log("Game Won!");
    }
}