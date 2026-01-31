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

    private void FixedUpdate()
    {
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.y += _lookInput.x;
        transform.rotation = Quaternion.Euler(rotation);
    }
}