using System.Collections.Generic;
using UnityEngine;

public abstract class InteractionDetectorBase : MonoBehaviour
{
    protected readonly List<IInteractable> _interactablesInRange = new();

    protected IInteractable _currentTarget;

    protected bool IsInCone(Vector3 originPos, Vector3 forward, Vector3 targetPos, float maxAngle)
    {
        Vector3 toTarget = targetPos - originPos;
        toTarget.y = 0f;
        if (toTarget.sqrMagnitude < 0.0001f) return true;

        float angle = Vector3.Angle(forward, toTarget.normalized);
        return angle <= maxAngle * 0.5f;
    }

    protected void RecalculateTarget(Vector3 originPosition, Vector3 originForward, float coneAngle)
    {
        _currentTarget = null;
        float bestScore = float.NegativeInfinity;

        foreach (var i in _interactablesInRange)
        {
            if (i == null) continue;
            if (!i.IsInteractable) continue;

            Vector3 targetPos = i.Transform.position;

            if (!IsInCone(originPosition, originForward, targetPos, coneAngle))
                continue;

            float distance = Vector3.Distance(originPosition, targetPos);

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
            _interactablesInRange.Add(interactable);
    }

    protected void TryRemoveInteractableFromCollider(Component col)
    {
        if (col == null) return;

        IInteractable interactable = col.GetComponent<IInteractable>()
                                ?? col.GetComponentInParent<IInteractable>();

        if (interactable == null) return;

        _interactablesInRange.Remove(interactable);
    }

    protected void InteractCurrent()
    {
        _currentTarget?.Interact();
    }
}