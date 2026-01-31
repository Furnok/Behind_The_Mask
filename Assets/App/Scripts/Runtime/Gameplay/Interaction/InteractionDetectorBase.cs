using System.Collections.Generic;
using UnityEngine;

public abstract class InteractionDetectorBase : MonoBehaviour
{
    protected readonly List<IInteractable> _interactablesInRange = new();

    protected IInteractable _currentTarget;

    protected void RecalculateTarget(Vector3 originPosition)
    {
        _currentTarget = null;
        float bestScore = float.NegativeInfinity;

        foreach (var i in _interactablesInRange)
        {
            if (i == null) continue;
            if (!i.IsInteractable) continue;

            float distance = Vector3.Distance(originPosition, i.Transform.position);
            float score = i.Priority * 1000f - distance;

            if (score > bestScore)
            {
                bestScore = score;
                _currentTarget = i;
            }
        }
    }

    protected void TryAddInteractableFromCollider(Component col)
    {
        if (col == null) return;

        IInteractable interactable = col.GetComponent<IInteractable>()
                                ?? col.GetComponentInParent<IInteractable>();

        if (interactable == null) return;

        if (!_interactablesInRange.Contains(interactable))
        {
            _interactablesInRange.Add(interactable);
            RecalculateTarget(transform.position);
        }
    }

    protected void TryRemoveInteractableFromCollider(Component col)
    {
        if (col == null) return;

        IInteractable interactable = col.GetComponent<IInteractable>()
                                ?? col.GetComponentInParent<IInteractable>();

        if (interactable == null) return;

        if (_interactablesInRange.Remove(interactable))
        {
            RecalculateTarget(transform.position);
        }
    }

    protected void InteractCurrent()
    {
        _currentTarget?.Interact();
    }
}