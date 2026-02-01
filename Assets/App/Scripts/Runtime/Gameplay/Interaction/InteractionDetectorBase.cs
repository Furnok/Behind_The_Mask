using System.Collections.Generic;
using UnityEngine;

public abstract class InteractionDetectorBase : MonoBehaviour
{
    protected readonly List<IInteractable> _interactablesInRange = new();

    protected IInteractable _currentTarget;

    protected bool IsInConeYawPitch(
    Vector3 originPos,
    Vector3 forward,
    Vector3 targetPos,
    float yawAngle,
    float pitchAngle)
    {
        Vector3 toTarget = (targetPos - originPos);
        if (toTarget.sqrMagnitude < 0.0001f) return true;

        Vector3 fwdXZ = new Vector3(forward.x, 0f, forward.z);
        Vector3 toXZ = new Vector3(toTarget.x, 0f, toTarget.z);

        if (fwdXZ.sqrMagnitude < 0.0001f || toXZ.sqrMagnitude < 0.0001f)
            return true;

        float yaw = Vector3.Angle(fwdXZ.normalized, toXZ.normalized);
        if (yaw > yawAngle * 0.5f) return false;

        float horizDist = toXZ.magnitude;
        float pitch = Mathf.Atan2(toTarget.y, horizDist) * Mathf.Rad2Deg;

        float forwardPitch = Mathf.Atan2(forward.y, fwdXZ.magnitude) * Mathf.Rad2Deg;

        float deltaPitch = Mathf.Abs(pitch - forwardPitch);
        return deltaPitch <= pitchAngle * 0.5f;
    }

    protected void RecalculateTarget(Vector3 originPosition, Vector3 originForward, float coneAngle, float pitchAngle)
    {
        _currentTarget = null;
        float bestScore = float.NegativeInfinity;

        foreach (var i in _interactablesInRange)
        {
            if (i == null) continue;
            if (!i.IsInteractable) continue;

            Vector3 targetPos = i.Transform.position;

            var col = i.Transform.GetComponent<Collider>();
            if (col != null)
                targetPos = col.bounds.center;

            if (!IsInConeYawPitch(originPosition, originForward, targetPos, coneAngle, pitchAngle))
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