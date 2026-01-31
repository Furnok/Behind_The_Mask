using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UI;

public class S_UIGame : MonoBehaviour
{
    [TabGroup("Settings")]
    [Title("Colors")]
    [SerializeField] private Color32 colorFocus;

    [TabGroup("Settings")]
    [SerializeField] private Color32 colorUnFocus;

    [TabGroup("References")]
    [Title("Sliders")]
    [SerializeField] private Slider sliderStamina;

    [TabGroup("References")]
    [Title("Book")]
    [SerializeField] private Image imageBook;

    [TabGroup("References")]
    [Title("Interact")]
    [SerializeField] private GameObject objInteract;

    [TabGroup("References")]
    [SerializeField] private CanvasGroup canvasGroupInteract;

    [TabGroup("References")]
    [Title("Inventory")]
    [SerializeField] private List<Image> imageInventorySlot;

    [TabGroup("References")]
    [SerializeField] private List<Image> imageInventory;

    [TabGroup("References")]
    [Title("Masks")]
    [SerializeField] private List<Sprite> spriteMasks;

    [TabGroup("References")]
    [SerializeField] private List<Sprite> spriteNoMasks;

    [TabGroup("Inputs")]
    [SerializeField] private RSE_OnUpdateUIInventory rseOnUpdateUIInventory;

    [TabGroup("Inputs")]
    [SerializeField] private RSE_OnEquippedMaskUI rseOnEquippedMaskUI;

    [TabGroup("Inputs")]
    [SerializeField] private RSE_OnUIInterract rseOnUIInterract;

    [TabGroup("Inputs")]
    [SerializeField] private RSO_CurrentStamina rsoSetStaminaSliderValue;

    [TabGroup("Outputs")]
    [SerializeField] private RSO_PlayerCurrentMaskUnlocked rsoPlayerCurrentMaskUnlocked;

    [TabGroup("Outputs")]
    [SerializeField] private SSO_PlayerSettings ssoPlayerSettings;

    [TabGroup("Outputs")]
    [SerializeField] private SSO_SliderTime ssoSliderTime;

    [TabGroup("Outputs")]
    [SerializeField] private SSO_DisplayWindowTime ssoDisplayWindowTime;

    private Tween interactTween = null;
    private Tween staminaTween = null;

    private int maskIndex = -1;

    private void Awake()
    {
        sliderStamina.maxValue = ssoPlayerSettings.Value.StaminaMax;
    }

    private void OnEnable()
    {
        rseOnUpdateUIInventory.action += UpdateInventory;
        rsoSetStaminaSliderValue.onValueChanged += SetStaminaSliderValue;
        rseOnEquippedMaskUI.action += UpdateFocus;
        rseOnUIInterract.action += DisplayInteract;
    }

    private void OnDisable()
    {
        rseOnUpdateUIInventory.action -= UpdateInventory;
        rsoSetStaminaSliderValue.onValueChanged -= SetStaminaSliderValue;
        rseOnEquippedMaskUI.action -= UpdateFocus;
        rseOnUIInterract.action -= DisplayInteract;
    }

    private void SetStaminaSliderValue(float value)
    {
        staminaTween?.Kill();

        staminaTween = sliderStamina.DOValue(value, ssoSliderTime.Value.time).SetEase(Ease.OutCubic);
    }

    private void UpdateInventory()
    {
        var values = rsoPlayerCurrentMaskUnlocked.Value.Values.ToList();

        for (int i = 0; i < imageInventory.Count; i++)
        {
            if (values[i])
            {
                imageInventory[i].sprite = spriteMasks[i];
            }
            else
            {
                imageInventory[i].sprite = spriteNoMasks[i];
            }
        }
    }

    private void UpdateFocus(int index)
    {
        for (int i = 0; i < imageInventorySlot.Count; i++)
        {
            imageInventorySlot[i].color = colorUnFocus;
        }

        if (maskIndex != index)
        {
            imageInventorySlot[index].color = colorFocus;
            maskIndex = index;
        }
        else
        {
            maskIndex = -1;
        }
    }

    private void DisplayInteract(bool value)
    {
        interactTween?.Kill();

        if (value)
        {
            interactTween = canvasGroupInteract.DOFade(1f, ssoDisplayWindowTime.Value.time).SetEase(Ease.Linear).SetUpdate(true).OnStart(() =>
            {
                objInteract.SetActive(true);
            });
        }
        else
        {
            interactTween =  canvasGroupInteract.DOFade(0f, ssoDisplayWindowTime.Value.time).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() =>
            {
                objInteract.SetActive(false);
            });
        }
    }
}