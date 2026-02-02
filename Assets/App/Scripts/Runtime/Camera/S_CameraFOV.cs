using UnityEngine;

public class S_CameraFOV : MonoBehaviour
{
    //[Header("Settings")]

    [Header("References")]
    [SerializeField] Camera _camera;
    [SerializeField] Transform masks;

    [Header("Inputs")]
    [SerializeField] RSO_PlayerIsSprinting _playerIsSprinting;

    [Header("Outputs")]
    [SerializeField] SSO_PlayerSettings _playerSettings;

    float _targetFOV;
    float _fovVelocity;

    float _baseTransformMasks;
    float _targetTransformMasks;
    float _transformVelocity;


    private void Awake()
    {
        float initialFOV = _playerSettings.Value.NormalFOV;
        _camera.fieldOfView = initialFOV;
    }

    private void OnEnable()
    {
        _playerIsSprinting.onValueChanged += OnSprintStateChanged;

        _baseTransformMasks = masks.localPosition.z;
    }

    void OnDisable()
    {
        _playerIsSprinting.onValueChanged -= OnSprintStateChanged;
    }

    void OnSprintStateChanged(bool isSprinting)
    {
        _targetFOV = isSprinting ? _playerSettings.Value.SprintFOV : _playerSettings.Value.NormalFOV;
        _targetTransformMasks = isSprinting ? -0.15f : _baseTransformMasks;
    }

    private void Update()
    {
        float current = _camera.fieldOfView;
        float next = Mathf.SmoothDamp(current, _targetFOV, ref _fovVelocity, _playerSettings.Value.SmoothTimeFOV);
        _camera.fieldOfView = next;

        float currentpos = masks.localPosition.z;
        float nextPos = Mathf.SmoothDamp(currentpos, _targetTransformMasks, ref _transformVelocity, _playerSettings.Value.SmoothTimeFOV);
        masks.localPosition = new Vector3(masks.localPosition.x, masks.localPosition.y, nextPos);
    }
}