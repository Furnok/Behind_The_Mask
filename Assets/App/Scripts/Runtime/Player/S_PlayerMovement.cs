using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class S_PlayerMovement : MonoBehaviour
{
    //[Header("Settings")]

    [Header("References")]
    [SerializeField] Rigidbody _rigidbody;
    [SerializeField] Camera _camera;


    [Header("Inputs")]
    [SerializeField] RSE_OnMoveInput _onMoveInput;

    [Header("Outputs")]
    [SerializeField] SSO_PlayerSettings _playerSettings;
    [SerializeField] RSO_CameraRotation _cameraRotation;
    [SerializeField] RSO_PlayerIsMoving _playerIsMoving;
    [SerializeField] RSO_PlayerIsSprinting _playerIsSprinting;


    Vector2 _moveInput = Vector2.zero;

    bool _isMoving => _playerIsMoving.Value;
    bool _isSprinting => _playerIsSprinting.Value;

    Vector3 _desiredDirection = Vector3.zero;

    private void Awake()
    {
        _playerIsMoving.Value = false;
    }

    private void OnEnable()
    {
        _onMoveInput.action += OnInput;
    }

    void OnDisable()
    {
        _onMoveInput.action -= OnInput;
    }

    void OnInput(Vector2 moveInput)
    {
        _moveInput = moveInput;
    }

    private void FixedUpdate()
    {
        BuildDirection();
        Vector3 moveDirection = _desiredDirection.normalized;
        moveDirection.y = 0f;
        _playerIsMoving.Value = moveDirection.magnitude > 0;
        float currentSpeed = _isSprinting ? _playerSettings.Value.SprintSpeed : _playerSettings.Value.WalkSpeed;
        Vector3 velocity = moveDirection * currentSpeed;
        _rigidbody.linearVelocity = new Vector3(velocity.x, _rigidbody.linearVelocity.y, velocity.z);
    }

    void BuildDirection()
    {
        var camera = _camera;
        Vector3 camForward = camera.transform.forward;
        Vector3 camRight = camera.transform.right;

        camForward.y = 0f; camRight.y = 0f;
        camForward.Normalize(); camRight.Normalize();

        Vector3 dirWorld = (camRight * _moveInput.x + camForward * _moveInput.y);
        if (dirWorld.sqrMagnitude > 0.0001f) dirWorld.Normalize();

        _desiredDirection = dirWorld;
    }
}