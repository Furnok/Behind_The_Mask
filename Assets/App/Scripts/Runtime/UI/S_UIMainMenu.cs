using Sirenix.OdinInspector;
using UnityEngine;

public class S_UIMainMenu : MonoBehaviour
{
    [TabGroup("References")]
    [Title("Levels")]
    [SerializeField] private S_SceneReference levelName;

    //[Header("References")]

    //[Header("Inputs")]

    [TabGroup("Outputs")]
    [SerializeField] private RSE_OnFadeOut rseOnFadeOut;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_OnOpenWindow rseOnOpenWindow;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_OnCloseAllWindows rseOnCloseAllWindows;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_OnQuitGame rseOnQuitGame;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_OnHideMouseCursor rseOnHideMouseCursor;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_OnLoadScene rseOnLoadScene;

    [TabGroup("Outputs")]
    [SerializeField] private RSO_Navigation rsoNavigation;

    [TabGroup("Outputs")]
    [SerializeField] private SSO_FadeTime ssoFadeTime;

    public void StartGame()
    {
        rseOnFadeOut.Call();

        StartCoroutine(S_Utils.DelayRealTime(ssoFadeTime.Value.time, () =>
        {
            rseOnCloseAllWindows.Call();
            rsoNavigation.Value.selectableFocus = null;

            //rseOnGamePause.Call(false);
            rseOnHideMouseCursor.Call();

            rseOnLoadScene.Call(levelName.Name);
        }));
    }

    public void Settings()
    {

    }

    public void Credits()
    {

    }

    public void QuitGame()
    {
        
    }
}