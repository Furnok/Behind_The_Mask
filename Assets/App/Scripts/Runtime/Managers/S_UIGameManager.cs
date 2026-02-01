using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class S_UIGameManager : MonoBehaviour
{
    [TabGroup("References")]
    [Title("Game Window")]
    [SerializeField] private GameObject gameWindow;

    [TabGroup("References")]
    [SerializeField] private CanvasGroup gameCanvasGroup;

    [TabGroup("References")]
    [Title("Book Window")]
    [SerializeField] private GameObject bookWindow;

    [TabGroup("References")]
    [Title("Main Menu Window")]
    [SerializeField] private GameObject menuWindow;

    [TabGroup("References")]
    [SerializeField] private CanvasGroup menuCanvasGroup;

    [TabGroup("References")]
    [Title("Game Over Window")]
    [SerializeField] private GameObject goWindow;

    [TabGroup("References")]
    [SerializeField] private CanvasGroup goCanvasGroup;

    [TabGroup("References")]
    [Title("Win Window")]
    [SerializeField] private GameObject winWindow;

    [TabGroup("References")]
    [SerializeField] private CanvasGroup winCanvasGroup;

    [TabGroup("References")]
    [Title("Fade Window")]
    [SerializeField] private GameObject fadeWindow;

    [TabGroup("References")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_OnMenu rseOnMenu;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_OnGame rseOnGame;

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

    [TabGroup("Inputs")]
    [SerializeField] private RSE_OnGameOver rseOnGameOver;

    [TabGroup("Inputs")]
    [SerializeField] private RSE_OnGameWin rseOnGameWin;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_OnEscapeInput rseOnEscapeInput;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_OnGamePause rseOnGamePause;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_OnBookInput rseOnBookInput;

    [TabGroup("Outputs")]
    [SerializeField] private RSO_CurrentWindows rsoCurrentWindows;

    [TabGroup("Outputs")]
    [SerializeField] private SSO_FadeTime ssoFadeTime;

    [TabGroup("Outputs")]
    [SerializeField] private SSO_DisplayWindowTime ssoDisplayWindowTime;

    private Tween gameTween = null;
    private Tween fadeTween = null;
    private Tween endTween = null;

    private void OnEnable()
    {
        rseOnMenu.action += SetupMenu;
        rseOnGame.action += DisplayGame;
        rseOnOpenWindow.action += AlreadyOpen;
        rseOnCloseWindow.action += CloseWindow;
        rseOnCloseAllWindows.action += CloseAllWindows;
        rseOnFadeIn.action += FadeIn;
        rseOnFadeOut.action += FadeOut;
        rseOnEscapeInput.action += PauseGame;
        rseOnBookInput.action += DisplayBook;
        rseOnGameOver.action += GameOver;
        rseOnGameWin.action += GameWin;

        rsoCurrentWindows.Value = new();
    }

    private void OnDisable()
    {
        rseOnMenu.action -= SetupMenu;
        rseOnGame.action -= DisplayGame;
        rseOnOpenWindow.action -= AlreadyOpen;
        rseOnCloseWindow.action -= CloseWindow;
        rseOnCloseAllWindows.action -= CloseAllWindows;
        rseOnFadeIn.action -= FadeIn;
        rseOnFadeOut.action -= FadeOut;
        rseOnEscapeInput.action -= PauseGame;
        rseOnBookInput.action -= DisplayBook;
        rseOnGameOver.action -= GameOver;
        rseOnGameWin.action -= GameWin;

        fadeTween?.Kill();

        rsoCurrentWindows.Value = new();
    }

    private void PauseGame()
    {
        if (rsoCurrentWindows.Value.Count < 1 && !menuWindow.activeInHierarchy && !goWindow.activeInHierarchy && !winWindow.activeInHierarchy)
        {
            OpenWindow(menuWindow);

            rseOnGamePause.Call(true);
        }
    }

    private void SetupMenu()
    {
        fadeWindow.SetActive(true);

        StartCoroutine(S_Utils.DelayRealTime(0.2f, () =>
        {
            FadeIn();

            OpenWindow(menuWindow);

            rseOnGamePause.Call(true);
        }));
    }

    private void DisplayGame()
    {
        fadeWindow.SetActive(true);

        StartCoroutine(S_Utils.DelayRealTime(0.2f, () =>
        {
            FadeIn();

            gameTween?.Kill();

            gameTween = gameCanvasGroup.DOFade(1f, ssoDisplayWindowTime.Value.time).SetEase(Ease.Linear).SetUpdate(true).OnStart(() =>
            {
                gameWindow.SetActive(true);
            });
        }));
    }

    private void DisplayBook()
    {
        if (!menuWindow.activeInHierarchy && !bookWindow.activeInHierarchy)
        {
            OpenWindow(bookWindow);
        }
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

        cg.DOFade(1f, ssoDisplayWindowTime.Value.time).SetEase(Ease.Linear).SetUpdate(true).OnStart(() =>
        {
            window.SetActive(true);
        });

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

    private void GameOver()
    {
        endTween?.Kill();

        endTween = goCanvasGroup.DOFade(1f, ssoDisplayWindowTime.Value.time).SetEase(Ease.Linear).SetUpdate(true).OnStart(() =>
        {
            goWindow.SetActive(true);

            rseOnGamePause.Call(true);
        });
    }

    private void GameWin()
    {
        endTween?.Kill();

        endTween = winCanvasGroup.DOFade(1f, ssoDisplayWindowTime.Value.time).SetEase(Ease.Linear).SetUpdate(true).OnStart(() =>
        {
            winWindow.SetActive(true);

            rseOnGamePause.Call(true);
        });
    }
}