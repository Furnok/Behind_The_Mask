using UnityEngine;
using UnityEngine.AI;

public class S_EnemyMotor : MonoBehaviour
{
    [SerializeField] NavMeshAgent _agent;

    [Header("Speeds")]
    [SerializeField] float _walkSpeed = 1.2f;
    [SerializeField] float _chaseSpeed = 3.5f;
    [SerializeField] float _runAwaySpeed = 3.0f;
    [SerializeField] float _rotationSpeed = 12f;

    bool _hasActiveDestination = false;
    Vector3 _currentDestination;

    public void SetSpeedWalk() => _agent.speed = _walkSpeed;
    public void SetSpeedChase() => _agent.speed = _chaseSpeed;
    public void SetSpeedRunAway() => _agent.speed = _runAwaySpeed;

    private void Update()
    {
        //Debug.Log(_currentDestination);

    }
    public void MoveTo(Vector3 pos)
    {
        if (!_agent.enabled) return;

        if (NavMesh.SamplePosition(pos, out NavMeshHit hit, 1.0f, _agent.areaMask))
        {
            _agent.isStopped = false;
            _currentDestination = hit.position;
            _hasActiveDestination = true;
            _agent.SetDestination(_currentDestination);
        }
        else
        {
            Debug.LogWarning("Destination not on NavMesh");
        }
    }

    public void Stop()
    {
        if (!_agent.enabled) return;
        _agent.isStopped = true;
    }

    public void LookAt(Vector3 worldPos)
    {
        Vector3 dir = worldPos - transform.position;
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.0001f) return;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), _rotationSpeed * Time.deltaTime);
    }

    public void FleeFrom(Vector3 playerPos)
    {
        Vector3 away = (transform.position - playerPos);
        away.y = 0f;

        if (away.sqrMagnitude < 0.0001f)
            away = transform.forward;

        away.Normalize();

        float[] distances = { 6f, 4f, 2.5f, 1.5f };

        float[] angles = { 0f, 25f, -25f, 50f, -50f, 90f, -90f, 135f, -135f, 180f };

        Vector3 origin = transform.position;

        foreach (float d in distances)
        {
            foreach (float a in angles)
            {
                Vector3 dir = Quaternion.Euler(0f, a, 0f) * away;
                Vector3 candidate = origin + dir * d;

                if (TryMoveToOnNavMesh(candidate, 2.0f))
                    return;
            }
        }

        Stop();
    }

    bool TryMoveToOnNavMesh(Vector3 pos, float sampleRadius)
    {
        if (!_agent.enabled) return false;

        if (NavMesh.SamplePosition(pos, out NavMeshHit hit, sampleRadius, _agent.areaMask))
        {
            _agent.isStopped = false;
            _currentDestination = hit.position;
            _hasActiveDestination = true;
            _agent.SetDestination(_currentDestination);
            return true;
        }

        return false;
    }

    public bool HasReachedDestination(float arrivedDistance = 0.25f)
    {
        if (!_agent.enabled) return true;
        if (!_hasActiveDestination) return false;
        if (_agent.pathPending) return false;

        if (_agent.pathStatus == NavMeshPathStatus.PathInvalid) return false;

        float dist = _agent.hasPath
            ? _agent.remainingDistance
            : Vector3.Distance(transform.position, _currentDestination);

        if (dist > arrivedDistance) return false;

        if (_agent.velocity.sqrMagnitude > 0.02f) return false;

        _hasActiveDestination = false;
        return true;
    }
}