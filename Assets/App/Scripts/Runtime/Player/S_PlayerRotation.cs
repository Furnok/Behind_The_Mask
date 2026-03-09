using UnityEngine;

public class S_PlayerRotation : MonoBehaviour
{
    //[Header("Settings")]

    [Header("References")]
    [SerializeField] Camera _camera;

    [Header("Inputs")]
    [SerializeField] RSE_OnLookInput _onLookInput;

    [Header("Outputs")]
    [SerializeField] SSO_PlayerSettings _playerSettings;

    Vector2 _lookInput = Vector2.zero;
    private float _currentYaw;

    private void OnEnable()
    {
        _onLookInput.action += OnInput;
    }

    void OnDisable()
    {
        _onLookInput.action -= OnInput;
    }

    void OnInput(Vector2 lookInput)
    {
        _lookInput = lookInput;
    }

    private void Update()
    {
        _currentYaw += _lookInput.x * _playerSettings.Value.Sensitivity;

        transform.rotation = Quaternion.Euler(0f, _currentYaw, 0f);
    }
}