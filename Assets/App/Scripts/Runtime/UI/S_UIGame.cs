using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class S_UIGame : MonoBehaviour
{
    [TabGroup("Settings")]
    [Title("Audio")]
    [SerializeField] private S_ClassAudio audioDisplay;

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
    [Title("Error")]
    [SerializeField] private GameObject objError;

    [TabGroup("References")]
    [SerializeField] private CanvasGroup canvasGroupError;

    [TabGroup("References")]
    [SerializeField] private TextMeshProUGUI textError;

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
    [SerializeField] private RSE_OnUIError rseOnUIError;

    [TabGroup("Inputs")]
    [SerializeField] private RSO_CurrentStamina rsoSetStaminaSliderValue;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_PlayAudio rsePlayAudio;

    [TabGroup("Outputs")]
    [SerializeField] private RSO_PlayerCurrentMaskUnlocked rsoPlayerCurrentMaskUnlocked;

    [TabGroup("Outputs")]
    [SerializeField] private SSO_PlayerSettings ssoPlayerSettings;

    [TabGroup("Outputs")]
    [SerializeField] private SSO_SliderTime ssoSliderTime;

    [TabGroup("Outputs")]
    [SerializeField] private SSO_DisplayWindowTime ssoDisplayWindowTime;

    private Tween interactTween = null;
    private Tween errorTween = null;
    private Tween staminaTween = null;

    private Coroutine errorCoroutine = null;

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
        rseOnUIError.action += DisplayError;
    }

    private void OnDisable()
    {
        rseOnUpdateUIInventory.action -= UpdateInventory;
        rsoSetStaminaSliderValue.onValueChanged -= SetStaminaSliderValue;
        rseOnEquippedMaskUI.action -= UpdateFocus;
        rseOnUIInterract.action -= DisplayInteract;
        rseOnUIError.action -= DisplayError;
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

    private void DisplayError(string text, Color color)
    {
        if (errorCoroutine != null)
        {
            StopCoroutine(errorCoroutine);
            errorCoroutine = null;

            objError.SetActive(false);
            textError.text = "";
            textError.color = Color.black;
            canvasGroupError.alpha = 0f;
        }

        errorTween?.Kill();

        errorTween = canvasGroupError.DOFade(1f, ssoDisplayWindowTime.Value.time).SetEase(Ease.Linear).SetUpdate(true).OnStart(() =>
        {
            objError.SetActive(true);
            textError.text = text;
            textError.color = color;

            rsePlayAudio.Call(audioDisplay);

        }).OnComplete(() =>
        {
            errorCoroutine = StartCoroutine(DisplayTime());
        });
    }

    private IEnumerator DisplayTime()
    {
        yield return new WaitForSeconds(3f);

        errorTween = canvasGroupError.DOFade(0f, ssoDisplayWindowTime.Value.time).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() =>
        {
            objError.SetActive(false);
            textError.text = "";
        });
    }
}