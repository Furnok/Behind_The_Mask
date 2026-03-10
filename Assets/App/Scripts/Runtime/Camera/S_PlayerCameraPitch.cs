using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class S_PlayerCameraPitch : MonoBehaviour
{
    //[Header("Settings")]

    //[Header("References")]

    [Header("Inputs")]
    [SerializeField] RSE_OnLookInput _onLookInput;

    [Header("Outputs")]
    [SerializeField] SSO_PlayerSettings _playerSettings;
    [SerializeField] RSO_GameInPause _gameInPause;

    Vector2 _lookInput = Vector2.zero;
    float _currentPitch = 0f;

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
        if (_gameInPause.Value)
        {
            _lookInput = Vector2.zero;
            return;
        }

        _currentPitch -= _lookInput.y * _playerSettings.Value.Sensitivity;
        _currentPitch = Mathf.Clamp(_currentPitch, _playerSettings.Value.MinPitchAngle, _playerSettings.Value.MaxPitchAngle);

        transform.localRotation = Quaternion.Euler(_currentPitch, 0f, 0f);
    }
}