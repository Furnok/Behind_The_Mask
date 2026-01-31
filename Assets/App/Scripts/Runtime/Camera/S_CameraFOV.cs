using UnityEngine;

public class S_CameraFOV : MonoBehaviour
{
    //[Header("Settings")]

    [Header("References")]
    [SerializeField] Camera _camera;

    [Header("Inputs")]
    [SerializeField] RSO_PlayerIsSprinting _playerIsSprinting;

    [Header("Outputs")]
    [SerializeField] SSO_PlayerSettings _playerSettings;

    float _targetFOV;
    float _fovVelocity;

    private void Awake()
    {
        float initialFOV = _playerSettings.Value.NormalFOV;
        _camera.fieldOfView = initialFOV;
    }

    private void OnEnable()
    {
        _playerIsSprinting.onValueChanged += OnSprintStateChanged;
    }

    void OnDisable()
    {
        _playerIsSprinting.onValueChanged -= OnSprintStateChanged;
    }

    void OnSprintStateChanged(bool isSprinting)
    {
        _targetFOV = isSprinting ? _playerSettings.Value.SprintFOV : _playerSettings.Value.NormalFOV;
    }

    private void Update()
    {
        float current = _camera.fieldOfView;
        float next = Mathf.SmoothDamp(current, _targetFOV, ref _fovVelocity, _playerSettings.Value.SmoothTimeFOV);
        _camera.fieldOfView = next;
    }
}