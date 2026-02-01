using TMPro;
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
    [SerializeField] Animator _animator;
    [SerializeField] Transform _headTransform;
    [SerializeField] TextMeshPro _stateText;

    [Header("Outputs")]
    [SerializeField] RSO_PlayerCurrentMaskEquipped _currentMask;
    [SerializeField] RSE_OnPlayerGettingCatch _onPlayerGettingCatch;
    [SerializeField] RSE_PlayAudio _onPlayAudio;

    [Header("Settings")]
    [SerializeField] float _chaseDelay = 1.0f;
    [SerializeField] float _lostTargetTime = 1.5f;
    [SerializeField] Mask _enemyMask;
    [SerializeField] float _distanceToPlayerForStoppingChase = 3f;
    [SerializeField] private S_ClassAudio audioSee;
    [SerializeField] private S_ClassAudio audioCaught;


    EState _state = EState.Walk;
    float _delayTimer = 0f;
    float _lostTimer = 0f;

    readonly int IsChasingHash = Animator.StringToHash("IsChasing");
    readonly int IsWalkingHash = Animator.StringToHash("IsWalking");
    readonly int IsIdleHash = Animator.StringToHash("IsIdle");
    readonly int IsObservingHash = Animator.StringToHash("IsObserving");
    readonly int IsCatchingHash = Animator.StringToHash("IsCatching");

    private void OnEnable()
    {
        _stateText.text = "";

        _state = EState.Walk;
        _delayTimer = 0f;
        _lostTimer = 0f;
        if (_patrol != null) _patrol.SetClosestAsCurrent(transform.position);
        if (_motor != null && _patrol != null && _patrol.HasPoints)
        {
            _motor.MoveTo(_patrol.GetCurrentPoint());
            PlayAnimation(IsWalkingHash);
        }
        else
        {
            PlayAnimation(IsIdleHash);
        }
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

                PlayAnimation(IsWalkingHash);
                _stateText.text = "";

                if (_motor.HasReachedDestination(0.35f))
                {
                    Vector3 next = _patrol.GetNextDestinationPointOnly();
                    _motor.MoveTo(next);
                }
                break;

            case EState.Observe:
                _motor.Stop();
                _motor.LookAt(_perception.PlayerPosition);

                PlayAnimation(IsObservingHash);
                _stateText.text = "<color=Yellow>??</color>";


                _delayTimer = 0f;
                break;

            case EState.Chase:
                _motor.SetSpeedChase();
                _onPlayAudio.Call(audioSee);

                _delayTimer += Time.deltaTime;
                if (_delayTimer >= _chaseDelay)
                {
                    _motor.MoveTo(_perception.PlayerPosition);
                    _motor.LookAt(_perception.PlayerPosition);


                    if (Vector3.Distance(transform.position, _perception.PlayerPosition) <= _distanceToPlayerForStoppingChase)
                    {
                        _motor.Stop();
                        _onPlayerGettingCatch.Call(_headTransform);

                        _animator.SetBool(IsCatchingHash, true);
                        _stateText.text = "";

                        Debug.Log("Player Caught by Enemy");
                        _onPlayAudio.Call(audioCaught);
                    }
                    else
                    {
                        _stateText.text = "<color=Red>!</color>";
                        PlayAnimation(IsChasingHash);
                    }
                }
                else
                    _motor.LookAt(_perception.PlayerPosition);
                break;

            case EState.RunAway:

                PlayAnimation(IsChasingHash);
                _stateText.text = "<color=Yellow>!!</color>";

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

    void PlayAnimation(int hashAnimation)
    {
        StopAnimation();

        _animator.SetBool(hashAnimation, true);
    }

    void StopAnimation()
    {
        _animator.SetBool(IsChasingHash, false);
        _animator.SetBool(IsWalkingHash, false);
        _animator.SetBool(IsIdleHash, false);
        _animator.SetBool(IsObservingHash, false);
        _animator.SetBool(IsCatchingHash, false);
    }
}