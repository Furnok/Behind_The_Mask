using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class S_UIWin : MonoBehaviour
{
    [TabGroup("Settings")]
    [Title("Audio")]
    [SerializeField] private S_ClassAudio audioWindow;

    [TabGroup("References")]
    [Title("Levels")]
    [SerializeField] private S_SceneReference levelName;

    [TabGroup("References")]
    [SerializeField] private S_SceneReference levelName2;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_OnShowMouseCursor rseOnShowMouseCursor;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_OnHideMouseCursor rseOnHideMouseCursor;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_PlayAudio rsePlayAudio;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_OnFadeOut rseOnFadeOut;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_StopAllAudio rseStopAllAudio;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_OnLoadScene rseOnLoadScene;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_OnGamePause rseOnGamePause;

    [TabGroup("Outputs")]
    [SerializeField] private RSO_Navigation rsoNavigation;

    [TabGroup("Outputs")]
    [SerializeField] private SSO_FadeTime ssoFadeTime;

    private bool isTransit = false;

    private void OnEnable()
    {
        isTransit = false;

        if (Gamepad.current == null) rseOnShowMouseCursor.Call();

        rsePlayAudio.Call(audioWindow);
    }

    public void Restart()
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

                StartCoroutine(S_Utils.DelayRealTime(0.8f, () =>
                {
                    rseOnLoadScene.Call(levelName.Name);
                    rseOnGamePause.Call(false);
                }));
            }));
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

                StartCoroutine(S_Utils.DelayRealTime(0.8f, () =>
                {
                    rseOnLoadScene.Call(levelName2.Name);
                    rseOnGamePause.Call(false);
                }));
            }));
        }
    }
}