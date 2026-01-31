using UnityEngine;

public class S_PlayerStamina : MonoBehaviour
{
    //[Header("Settings")]

    //[Header("References")]

    //[Header("Inputs")]

    [Header("Outputs")]
    [SerializeField] RSO_CurrentStamina _currentStamina;
    [SerializeField] SSO_PlayerSettings _playerSettings;


    private void Awake()
    {
        _currentStamina.Value = _playerSettings.Value.StaminaMax;
    }
}