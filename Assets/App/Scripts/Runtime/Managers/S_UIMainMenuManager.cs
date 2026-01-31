using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

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

    [TabGroup("Outputs")]
    [SerializeField] private RSE_OnShowMouseCursor rseOnShowMouseCursor;

    [TabGroup("Outputs")]
    [SerializeField] private SSO_FadeTime ssoFadeTime;

    [TabGroup("Outputs")]
    [SerializeField] private SSO_DisplayWindowTime ssoDisplayWindowTime;

    private Tween defaultTween = null;
    private Tween fadeTween = null;

    private void OnEnable()
    {
        rseOnMainMenu.action += Setup;
    }

    private void OnDisable()
    {
        rseOnMainMenu.action -= Setup;
    }

    private void Setup()
    {
        fadeWindow.SetActive(true);

        StartCoroutine(S_Utils.DelayRealTime(0.2f, () =>
        {
            FadeIn();

            StartCoroutine(S_Utils.DelayRealTime(ssoFadeTime.Value.time, () =>
            {
                rseOnShowMouseCursor.Call();

                defaultTween?.Kill();

                defaultTween = mainMenuCanvasGroup.DOFade(1f, ssoDisplayWindowTime.Value.time).SetEase(Ease.Linear).SetUpdate(true).OnStart(() =>
                {
                    defaultWindow.SetActive(true);
                });
            }));
        }));

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