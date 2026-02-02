using UnityEngine;

public class S_CameraMoveFeel : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float _idleAmplitude = 0.015f;
    [SerializeField] float _idleFrequency = 1.2f;

    [SerializeField] float _walkAmplitude = 0.03f;
    [SerializeField] float _walkFrequency = 1.8f;

    [SerializeField] float _sprintAmplitude = 0.06f;
    [SerializeField] float _sprintFrequency = 2.6f;

    [SerializeField] float _returnSpeed = 10f;

    [Header("References")]
    [SerializeField] Transform _cameraRig;

    //[Header("Inputs")]

    [Header("Outputs")]
    [SerializeField] RSO_PlayerIsMoving _playerIsMoving;
    [SerializeField] RSO_PlayerIsSprinting _playerIsSprinting;

    Vector3 _initialLocalPos;
    float _t;

    private void Awake()
    {
        if (_cameraRig == null) _cameraRig = transform;
        _initialLocalPos = _cameraRig.localPosition;
    }

    private void Update()
    {
        bool moving = _playerIsMoving.Value;
        bool sprint = _playerIsSprinting.Value;

        float amp, freq;

        if (!moving)
        {
            amp = _idleAmplitude;
            freq = _idleFrequency;
        }
        else if (sprint)
        {
            amp = _sprintAmplitude;
            freq = _sprintFrequency;
        }
        else
        {
            amp = _walkAmplitude;
            freq = _walkFrequency;
        }

        _t += Time.deltaTime * freq;

        // Respiration / bob: vertical + léger side sway
        float y = Mathf.Sin(_t) * amp;
        float x = Mathf.Sin(_t * 0.5f) * (amp * 0.4f);

        Vector3 target = _initialLocalPos + new Vector3(x, y, 0f);

        // retour smooth (important pour éviter jitter)
        _cameraRig.localPosition = Vector3.Lerp(_cameraRig.localPosition, target, _returnSpeed * Time.deltaTime);
    }
}