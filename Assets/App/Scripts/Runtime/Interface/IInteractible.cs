using UnityEngine;

public interface IInteractable
{
    int Priority { get; }
    Transform Transform { get; }
    bool IsInteractable { get; }

    void Interact();
}
