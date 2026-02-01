using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class S_UISettings : MonoBehaviour
{
    [TabGroup("Settings")]
    [Title("Audio")]
    [SerializeField] private S_ClassAudio audioClick;

    [TabGroup("Settings")]
    [SerializeField] private S_ClassAudio audioWindow;

    [TabGroup("References")]
    [Title("Selectables")]
    [SerializeField] private Selectable dropDownResolutions;

    [TabGroup("References")]
    [Title("Script")]
    [SerializeField] private S_Settings settings;

    [TabGroup("Inputs")]
    [SerializeField] private RSE_OnEscapeInput rseOnEscapeInput;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_OnCloseWindow rseOnCloseWindow;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_PlayAudio rsePlayAudio;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_OnShowMouseCursor rseOnShowMouseCursor;

    [TabGroup("Outputs")]
    [SerializeField] private RSO_Navigation rsoNavigation;

    [TabGroup("Outputs")]
    [SerializeField] private RSO_CurrentWindows rsoCurrentWindows;

    [TabGroup("Outputs")]
    [SerializeField] private SSO_DisplayWindowTime ssoDisplayWindowTime;

    private bool isClosing = false;

    private void OnEnable()
    {
        rseOnEscapeInput.action += CloseEscape;

        if (Gamepad.current == null) rseOnShowMouseCursor.Call();

        isClosing = false;

        rsePlayAudio.Call(audioWindow);
    }

    private void OnDisable()
    {
        rseOnEscapeInput.action -= CloseEscape;

        isClosing = false;
    }

    public void OnDropdownClicked(Selectable uiElement)
    {
        if (uiElement.interactable)
        {
            GameObject blocker = GameObject.Find("Blocker");
            if (blocker != null)
            {
                Button button = blocker.GetComponent<Button>();
                if (button != null) button.onClick.AddListener(CloseDropDown);
            }
        }
    }

    private void CloseDropDown()
    {
        rsePlayAudio.Call(audioClick);
    }

    private void CloseEscape()
    {
        if (dropDownResolutions?.GetComponent<TMP_Dropdown>()?.IsExpanded == true)
        {
            rsePlayAudio.Call(audioClick);

            return;
        }

        if (!isClosing)
        {
            if (rsoCurrentWindows.Value[^1] == gameObject) Close();
        }
    }

    public void Close()
    {
        if (!isClosing)
        {
            isClosing = true;
            settings.Close();

            rsePlayAudio.Call(audioWindow);

            rseOnCloseWindow.Call(gameObject);

            if (rsoNavigation.Value.selectablePressOldWindow == null) rsoNavigation.Value.selectableFocus = null;
            else
            {
                rsoNavigation.Value.selectablePressOldWindow?.Select();
                rsoNavigation.Value.selectablePressOldWindow = null;
            }
        }
    }
}