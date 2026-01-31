using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class S_UIDevice : MonoBehaviour
{
    [TabGroup("References")]
    [Title("Images")]
    [SerializeField] private List<Image> images;

    [TabGroup("References")]
    [Title("Text")]
    [SerializeField] private List<TextMeshProUGUI> texts;

    [TabGroup("References")]
    [Title("Keyboard & Mouse")]
    [SerializeField] private List<Sprite> imagesKeyboardMouse;

    [TabGroup("References")]
    [SerializeField] private List<string> textKeyboardMouse;

    [TabGroup("References")]
    [Title("PlayStation")]
    [SerializeField] private List<Sprite> imagesPlayStation;

    [TabGroup("References")]
    [Title("Xbox")]
    [SerializeField] private List<Sprite> imagesXbox;

    [TabGroup("Inputs")]
    [SerializeField] private RSE_OnUpdateDevice rseOnUpdateDevice;

    [TabGroup("Outputs")]
    [SerializeField] private RSO_Device rsoDevice;

    private void OnEnable()
    {
        rseOnUpdateDevice.action += UpdateInputsUI;

        UpdateInputsUI();
    }

    private void OnDisable()
    {
        rseOnUpdateDevice.action -= UpdateInputsUI;
    }

    private void UpdateInputsUI()
    {
        if (rsoDevice.Value == S_EnumDevice.KeyboardMouse)
        {
            for (int i = 0; i < images.Count; i++)
            {
                images[i].sprite = imagesKeyboardMouse[i];
                texts[i].text = textKeyboardMouse[i];
            }
        }
        else if (rsoDevice.Value == S_EnumDevice.PlaystationController)
        {
            for (int i = 0; i < images.Count; i++)
            {
                images[i].sprite = imagesPlayStation[i];
                texts[i].text = "";
            }
        }
        else if (rsoDevice.Value == S_EnumDevice.XboxController)
        {
            for (int i = 0; i < images.Count; i++)
            {
                images[i].sprite = imagesXbox[i];
                texts[i].text = "";
            }
        }
    }
}