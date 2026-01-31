using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class S_UIMainMenuManager : MonoBehaviour
{
    [TabGroup("References")]
    [Title("Default Window")]
    [SerializeField] private GameObject defaultWindow;

    [TabGroup("References")]
    [Title("Main Menu Window")]
    [SerializeField] private GameObject mainMenuWindow;

    [TabGroup("References")]
    [SerializeField] private CanvasGroup mainMenuCanvasGroup;

    [TabGroup("References")]
    [Title("Fade Window")]
    [SerializeField] private GameObject fadeWindow;

    [TabGroup("References")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;

    [TabGroup("Inputs")]
    [SerializeField] private RSE_OnMainMenu rseOnMainMenu;

    [TabGroup("Inputs")]
    [SerializeField] private RSE_OnOpenWindow rseOnOpenWindow;

    [TabGroup("Inputs")]
    [SerializeField] private RSE_OnCloseWindow rseOnCloseWindow;

    [TabGroup("Inputs")]
    [SerializeField] private RSE_OnCloseAllWindows rseOnCloseAllWindows;

    [TabGroup("Inputs")]
    [SerializeField] private RSE_OnFadeIn rseOnFadeIn;

    [TabGroup("Inputs")]
    [SerializeField] private RSE_OnFadeOut rseOnFadeOut;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_OnShowMouseCursor rseOnShowMouseCursor;

    [TabGroup("Outputs")]
    [SerializeField] private RSO_CurrentWindows rsoCurrentWindows;

    [TabGroup("Outputs")]
    [SerializeField] private SSO_FadeTime ssoFadeTime;

    [TabGroup("Outputs")]
    [SerializeField] private SSO_DisplayWindowTime ssoDisplayWindowTime;

    private Tween defaultTween = null;
    private Tween fadeTween = null;

    private void OnEnable()
    {
        rseOnMainMenu.action += Setup;
        rseOnOpenWindow.action += AlreadyOpen;
        rseOnCloseWindow.action += CloseWindow;
        rseOnCloseAllWindows.action += CloseAllWindows;
        rseOnFadeIn.action += FadeIn;
        rseOnFadeOut.action += FadeOut;
    }

    private void OnDisable()
    {
        rseOnMainMenu.action -= Setup;
        rseOnOpenWindow.action -= AlreadyOpen;
        rseOnCloseWindow.action -= CloseWindow;
        rseOnCloseAllWindows.action -= CloseAllWindows;
        rseOnFadeIn.action -= FadeIn;
        rseOnFadeOut.action -= FadeOut;
    }

    private void Setup()
    {
        fadeWindow.SetActive(true);

        StartCoroutine(S_Utils.DelayRealTime(0.2f, () =>
        {
            FadeIn();

            StartCoroutine(S_Utils.DelayRealTime(ssoFadeTime.Value.time, () =>
            {
                if (Gamepad.current == null) rseOnShowMouseCursor.Call();

                defaultTween?.Kill();

                defaultTween = mainMenuCanvasGroup.DOFade(1f, ssoDisplayWindowTime.Value.time).SetEase(Ease.Linear).SetUpdate(true).OnStart(() =>
                {
                    defaultWindow.SetActive(true);
                });
            }));
        }));

    }

    private void AlreadyOpen(GameObject window)
    {
        if (window != null)
        {
            if (!window.activeInHierarchy) OpenWindow(window);
            else CloseWindow(window);
        }
    }

    private void OpenWindow(GameObject window)
    {
        CanvasGroup cg = window.GetComponent<CanvasGroup>();
        cg.DOKill();

        window.SetActive(true);

        cg.DOFade(1f, ssoDisplayWindowTime.Value.time).SetEase(Ease.Linear).SetUpdate(true);

        rsoCurrentWindows.Value.Add(window);
    }

    private void CloseWindow(GameObject window)
    {
        if (window != null && window.activeInHierarchy)
        {
            CanvasGroup cg = window.GetComponent<CanvasGroup>();
            cg.DOKill();

            cg.DOFade(0f, ssoDisplayWindowTime.Value.time).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() =>
            {
                window.SetActive(false);
            });

            rsoCurrentWindows.Value.Remove(window);
        }
    }

    private void CloseAllWindows()
    {
        foreach (var window in rsoCurrentWindows.Value)
        {
            CanvasGroup cg = window.GetComponent<CanvasGroup>();
            cg.DOKill();

            cg.DOFade(0f, ssoDisplayWindowTime.Value.time).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() =>
            {
                window.SetActive(false);
            });
        }

        rsoCurrentWindows.Value.Clear();
    }

    private void FadeIn()
    {
        fadeTween?.Kill();

        fadeTween = fadeCanvasGroup.DOFade(0f, ssoFadeTime.Value.time).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() =>
        {
            fadeWindow.SetActive(false);
        });
    }

    private void FadeOut()
    {
        fadeTween?.Kill();

        fadeTween = fadeCanvasGroup.DOFade(1f, ssoFadeTime.Value.time).SetEase(Ease.Linear).SetUpdate(true).OnStart(() =>
        {
            fadeWindow.SetActive(true);
        });
    }
}