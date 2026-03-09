using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class S_UIMainMenu : MonoBehaviour
{
    [TabGroup("References")]
    [Title("Levels")]
    [SerializeField] private S_SceneReference levelName;

    [TabGroup("References")]
    [Title("Settings Window")]
    [SerializeField] private GameObject settingsWindow;

    [TabGroup("References")]
    [Title("Credits Window")]
    [SerializeField] private GameObject creditsWindow;

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
    [SerializeField] private RSE_OnNeedCursor rseOnNeedCursor;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_OnLoadScene rseOnLoadScene;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_StopAllAudio rseStopAllAudio;

    [TabGroup("Outputs")]
    [SerializeField] private RSO_Navigation rsoNavigation;

    [TabGroup("Outputs")]
    [SerializeField] private SSO_FadeTime ssoFadeTime;

    private bool isTransit = false;

    private void OnEnable()
    {
        if (Gamepad.current == null) rseOnShowMouseCursor.Call();

        isTransit = false;
    }

    public void StartGame()
    {
        if (!isTransit)
        {
            isTransit = true;

            rseOnFadeOut.Call();

            rseStopAllAudio.Call();

            StartCoroutine(S_Utils.DelayRealTime(ssoFadeTime.Value.time, () =>
            {
                rseOnCloseAllWindows.Call();
                rsoNavigation.Value.selectableFocus = null;

                rseOnHideMouseCursor.Call();
                rseOnNeedCursor.Call(false);

                StartCoroutine(S_Utils.DelayRealTime(0.8f, () => rseOnLoadScene.Call(levelName.Name)));
            }));
        }
    }

    public void Settings()
    {
        if (!isTransit)
        {
            rseOnCloseAllWindows.Call();
            rsoNavigation.Value.selectableFocus = null;
            rseOnOpenWindow.Call(settingsWindow);
        }
    }

    public void Credits()
    {
        if (!isTransit)
        {
            rseOnCloseAllWindows.Call();
            rsoNavigation.Value.selectableFocus = null;
            rseOnOpenWindow.Call(creditsWindow);
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