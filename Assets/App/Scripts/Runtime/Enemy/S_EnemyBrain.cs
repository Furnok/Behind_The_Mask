using UnityEngine;
using UnityEngine.AI;

public class S_EnemyBrain : MonoBehaviour
{
    public enum EState { Walk, Observe, Chase, RunAway }

    [Header("Referencess")]
    [SerializeField] S_EnemyPerception _perception;
    [SerializeField] S_EnemyMotor _motor;
    [SerializeField] S_EnemyPatrol _patrol;
    [SerializeField] NavMeshAgent _navMeshAgent;

    [Header("Outputs")]
    [SerializeField] RSO_PlayerCurrentMaskEquipped _currentMask;
    [SerializeField] RSE_OnPlayerGettingCatch _onPlayerGettingCatch;

    [Header("Settings")]
    [SerializeField] float _chaseDelay = 1.0f;
    [SerializeField] float _lostTargetTime = 1.5f;
    [SerializeField] Mask _enemyMask;
    [SerializeField] float _distanceToPlayerForStoppingChase = 3f;

    EState _state = EState.Walk;
    float _delayTimer = 0f;
    float _lostTimer = 0f;

    private void OnEnable()
    {
        _state = EState.Walk;
        _delayTimer = 0f;
        _lostTimer = 0f;
        if (_patrol != null) _patrol.SetClosestAsCurrent(transform.position);
        if (_motor != null && _patrol != null && _patrol.HasPoints) _motor.MoveTo(_patrol.GetCurrentPoint());
    }

    private void Update()
    {
        bool inObserve = _perception.PlayerInObserveRadius;
        bool inCone = _perception.PlayerInCone;

        bool isEngaged = _state == EState.Observe || _state == EState.Chase || _state == EState.RunAway;

        if (isEngaged && !inObserve)
        {
            _lostTimer += Time.deltaTime;
            if (_lostTimer >= _lostTargetTime)
                SwitchToWalk();
        }
        else
        {
            _lostTimer = 0f;
        }

        if (inCone && _state != EState.Chase)
        {
            EnemyMaskSolver(inCone);
        }
        else if (_state == EState.Chase)
        {
            EnemyMaskSolver(inCone);
        }
        else if (inObserve)
        {
            EnemyMaskSolverInObserve(inObserve);
        }

        TickState();

        Debug.Log($"Enemy State: {_state}");

    }

    void EnemyMaskSolver(bool inCone)
    {
        switch (_currentMask.Value)
        {
            case Mask.None:
                if (inCone)
                    SwitchState(EState.Chase);
                else
                    SwitchState(EState.Observe);
                break;
            case Mask.GreenMask:
                switch (_enemyMask)
                {
                    case Mask.None:
                        break;
                    case Mask.GreenMask:
                        SwitchState(EState.RunAway);
                        break;
                    case Mask.RedMask:
                        if (inCone)
                            SwitchState(EState.Chase);
                        else
                            SwitchState(EState.Observe);
                        break;
                    case Mask.BlueMask:
                        SwitchState(EState.Observe);
                        break;
                }
                break;
            case Mask.RedMask:
                switch (_enemyMask)
                {
                    case Mask.None:
                        break;
                    case Mask.GreenMask:
                        SwitchState(EState.Observe);
                        break;
                    case Mask.RedMask:
                        SwitchState(EState.RunAway);
                        break;
                    case Mask.BlueMask:
                        if (inCone)
                            SwitchState(EState.Chase);
                        else
                            SwitchState(EState.Observe);
                        break;
                }
                break;
            case Mask.BlueMask:
                switch (_enemyMask)
                {
                    case Mask.None:
                        break;
                    case Mask.GreenMask:
                        if (inCone)
                            SwitchState(EState.Chase);
                        else
                            SwitchState(EState.Observe);
                        break;
                    case Mask.RedMask:
                        SwitchState(EState.Observe);
                        break;
                    case Mask.BlueMask:
                        SwitchState(EState.RunAway);
                        break;
                }
                break;
        }
    }

    void EnemyMaskSolverInObserve(bool inObserve)
    {
        switch (_currentMask.Value)
        {
            case Mask.None:
                
            case Mask.GreenMask:
                switch (_enemyMask)
                {
                    case Mask.None:
                        break;
                    case Mask.GreenMask:
                        break;
                    case Mask.RedMask:
                    case Mask.BlueMask:
                        SwitchState(EState.Observe);
                        break;
                }
                break;
            case Mask.RedMask:
                switch (_enemyMask)
                {
                    case Mask.None:
                        break;
                    case Mask.GreenMask:
                        SwitchState(EState.Observe);
                        break;
                    case Mask.RedMask:
                        break;
                    case Mask.BlueMask:
                        break;
                }
                break;
            case Mask.BlueMask:
                switch (_enemyMask)
                {
                    case Mask.None:
                        break;
                    case Mask.GreenMask:
                        break;
                    case Mask.RedMask:
                        SwitchState(EState.Observe);
                        break;
                    case Mask.BlueMask:
                        break;
                }
                break;
        }
    }

    void TickState()
    {
        switch (_state)
        {
            case EState.Walk:
                _motor.SetSpeedWalk();

                if (_motor.HasReachedDestination(0.35f))
                {
                    Vector3 next = _patrol.GetNextDestinationPointOnly();
                    _motor.MoveTo(next);
                }
                break;

            case EState.Observe:
                _motor.Stop();
                _motor.LookAt(_perception.PlayerPosition);
                _delayTimer = 0f;
                break;

            case EState.Chase:
                _motor.SetSpeedChase();

                _delayTimer += Time.deltaTime;
                if (_delayTimer >= _chaseDelay)
                {
                    _motor.MoveTo(_perception.PlayerPosition);
                    _motor.LookAt(_perception.PlayerPosition);

                    if(Vector3.Distance(transform.position, _perception.PlayerPosition) <= _distanceToPlayerForStoppingChase)
                    {
                        _motor.Stop();
                        _onPlayerGettingCatch.Call(transform);
                        Debug.Log("Player Caught by Enemy");
                    }
                }
                else
                    _motor.LookAt(_perception.PlayerPosition);
                break;

            case EState.RunAway:
                _motor.SetSpeedRunAway();
                _motor.FleeFrom(_perception.PlayerPosition);
                _delayTimer = 0f;
                break;
        }
    }

    void SwitchToWalk()
    {
        if (_state == EState.Walk) return;

        SwitchState(EState.Walk);
        _patrol.SetClosestAsCurrent(transform.position);
        _motor.MoveTo(_patrol.GetCurrentPoint());
    }

    void SwitchState(EState newState)
    {
        if (_state == newState) return;

        _state = newState;
        _delayTimer = 0f;

        if (_state == EState.Walk)
            _patrol.ResetIfNeeded();
    }
}