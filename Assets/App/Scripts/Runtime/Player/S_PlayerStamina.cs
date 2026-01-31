using UnityEngine;

public class S_PlayerStamina : MonoBehaviour
{
    //[Header("Settings")]

    //[Header("References")]

    [Header("Inputs")]
    [SerializeField] RSE_OnSprintInput _onSprintInput;
    [SerializeField] RSE_OnSprintCancelInput _onSprintCancelInput;

    [Header("Outputs")]
    [SerializeField] RSO_CurrentStamina _currentStamina;
    [SerializeField] SSO_PlayerSettings _playerSettings;
    [SerializeField] RSO_PlayerIsSprinting _playerIsSprinting;
    [SerializeField] RSO_PlayerIsMoving _playerIsMoving;

    bool _canSprint => _playerIsSprinting.Value;
    float _recoveryTimer = 0f;
    bool _wantsSprint = false;

    private void Awake()
    {
        _currentStamina.Value = _playerSettings.Value.StaminaMax;
        _playerIsSprinting.Value = false;
        _recoveryTimer = 0f;
        _wantsSprint = false;
    }

    void OnEnable()
    {
        _onSprintInput.action += OnSprintInput;
        _onSprintCancelInput.action += OnSprintCancelInput;
    }

    void OnDisable()
    {
        _onSprintInput.action -= OnSprintInput;
        _onSprintCancelInput.action -= OnSprintCancelInput;
    }

    private void Update()
    {
        var settings = _playerSettings.Value;

        float stamina = _currentStamina.Value;
        stamina = Mathf.Clamp(stamina, 0f, settings.StaminaMax);

        bool canSprintNow = _wantsSprint && stamina > 0.001f && _playerIsMoving.Value == true;

        _playerIsSprinting.Value = canSprintNow;

        if (canSprintNow)
        {
            stamina -= settings.StaminaDrainRate * Time.deltaTime;
            if (stamina <= 0f)
            {
                stamina = 0f;

                _playerIsSprinting.Value = false;

                _recoveryTimer = settings.StaminaRecoveryDelay;
            }
            else
            {
                _recoveryTimer = settings.StaminaRecoveryDelay;
            }
        }
        else
        {
            _wantsSprint = false;

            if (_recoveryTimer > 0f)
            {
                _recoveryTimer -= Time.deltaTime;
            }
            else
            {
                stamina += settings.StaminaRecoveryRate * Time.deltaTime;
                if (stamina > settings.StaminaMax) stamina = settings.StaminaMax;
            }
        }

        _currentStamina.Value = stamina;
    }

    void OnSprintInput()
    {
        _wantsSprint = true;
    }

    void OnSprintCancelInput()
    {
        _wantsSprint = false;
    }
}