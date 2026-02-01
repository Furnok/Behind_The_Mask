using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class S_UIBook : MonoBehaviour
{
    [TabGroup("Settings")]
    [Title("Audio")]
    [SerializeField] private S_ClassAudio audioWindow;

    [TabGroup("Inputs")]
    [SerializeField] private RSE_OnEscapeInput rseOnEscapeInput;

    [TabGroup("Inputs")]
    [SerializeField] private RSE_OnBookInput rseOnBookInput;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_OnCloseWindow rseOnCloseWindow;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_OnShowMouseCursor rseOnShowMouseCursor;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_OnHideMouseCursor rseOnHideMouseCursor;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_PlayAudio rsePlayAudio;

    [TabGroup("Outputs")]
    [SerializeField] private RSO_Navigation rsoNavigation;

    [TabGroup("Outputs")]
    [SerializeField] private RSO_CurrentWindows rsoCurrentWindows;

    private bool isClosing = false;

    private void OnEnable()
    {
        rseOnEscapeInput.action += CloseEscape;
        rseOnBookInput.action += CloseEscape;

        if (Gamepad.current == null) rseOnShowMouseCursor.Call();

        isClosing = false;

        rsePlayAudio.Call(audioWindow);
    }

    private void OnDisable()
    {
        rseOnEscapeInput.action -= CloseEscape;
        rseOnBookInput.action -= CloseEscape;

        rseOnHideMouseCursor.Call();

        isClosing = false;
    }

    private void CloseEscape()
    {
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
            rseOnCloseWindow.Call(gameObject);

            rsePlayAudio.Call(audioWindow);

            rsoNavigation.Value.selectableFocus = null;
        }
    }
}