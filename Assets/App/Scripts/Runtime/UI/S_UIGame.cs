using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class S_UIGame : MonoBehaviour
{
    //[Header("Settings")]

    [TabGroup("References")]
    [Title("Sliders")]
    [SerializeField] private Slider sliderStamina;

    [TabGroup("Inputs")]
    [SerializeField] private RSO_CurrentStamina rsoSetStaminaSliderValue;

    [TabGroup("Outputs")]
    [SerializeField] private SSO_PlayerSettings ssoPlayerSettings;

    [TabGroup("Outputs")]
    [SerializeField] private SSO_SliderTime ssoSliderTime;

    private Tween staminaTween = null;

    private void Awake()
    {
        sliderStamina.maxValue = ssoPlayerSettings.Value.StaminaMax;
    }

    private void OnEnable()
    {
        rsoSetStaminaSliderValue.onValueChanged += SetStaminaSliderValue;
    }

    private void OnDisable()
    {
        rsoSetStaminaSliderValue.onValueChanged -= SetStaminaSliderValue;
    }

    private void SetStaminaSliderValue(float value)
    {
        staminaTween?.Kill();

        staminaTween = sliderStamina.DOValue(value, ssoSliderTime.Value.time).SetEase(Ease.OutCubic);
    }
}