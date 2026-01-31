using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class S_UIMenu : MonoBehaviour
{
    [TabGroup("Settings")]
    [Title("Audio")]
    [SerializeField] private S_ClassAudio audioWindow;

    [TabGroup("References")]
    [Title("Levels")]
    [SerializeField] private S_SceneReference levelName;

    [TabGroup("References")]
    [Title("Settings Window")]
    [SerializeField] private GameObject settingsWindow;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_OnFadeOut rseOnFadeOut;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_OnOpenWindow rseOnOpenWindow;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_OnCloseAllWindows rseOnCloseAllWindows;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_OnQuitGame rseOnQuitGame;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_OnShowMouseCursor rseOnShowMouseCursor;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_OnHideMouseCursor rseOnHideMouseCursor;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_OnLoadScene rseOnLoadScene;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_StopAllAudio rseStopAllAudio;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_OnEscapeInput rseOnEscapeInput;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_PlayAudio rsePlayAudio;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_OnResetFocus rseOnResetFocus;

    [TabGroup("Outputs")]
    [SerializeField] private RSO_CurrentWindows rsoCurrentWindows;

    [TabGroup("Outputs")]
    [SerializeField] private RSO_Navigation rsoNavigation;

    [TabGroup("Outputs")]
    [SerializeField] private SSO_FadeTime ssoFadeTime;

    private bool isTransit = false;

    private void OnEnable()
    {
        rseOnEscapeInput.action += CloseEscape;

        if (Gamepad.current == null) rseOnShowMouseCursor.Call();

        isTransit = false;

        rsePlayAudio.Call(audioWindow);
    }

    private void OnDisable()
    {
        rseOnEscapeInput.action -= CloseEscape;
    }

    private void CloseEscape()
    {
        if (rsoCurrentWindows.Value.Count > 0 && rsoCurrentWindows.Value[^1] == gameObject) ResumeGame();
    }

    public void ResumeGame()
    {
        if (!isTransit)
        {
            rseOnHideMouseCursor.Call();

            rseOnCloseAllWindows.Call();
            rsoNavigation.Value.selectableDefault = null;
            rseOnResetFocus.Call();
        }
    }

    public void Settings()
    {
        if (!isTransit)
        {
            rsoNavigation.Value.selectableFocus = null;
            rseOnOpenWindow.Call(settingsWindow);
        }
    }

    public void MainMenu()
    {
        if (!isTransit)
        {
            isTransit = true;

            rseOnFadeOut.Call();

            rseStopAllAudio.Call();

            StartCoroutine(S_Utils.DelayRealTime(ssoFadeTime.Value.time, () =>
            {
                rsoNavigation.Value.selectableFocus = null;

                rseOnHideMouseCursor.Call();

                StartCoroutine(S_Utils.DelayRealTime(0.8f, () => rseOnLoadScene.Call(levelName.Name)));
            }));
        }
    }

    public void QuitGame()
    {
        if (!isTransit)
        {
            isTransit = true;

            rseOnFadeOut.Call();

            StartCoroutine(S_Utils.DelayRealTime(ssoFadeTime.Value.time, () =>
            {
                rseOnQuitGame.Call();
                rsoNavigation.Value.selectableFocus = null;
            }));
        }
    }
}