using Sirenix.OdinInspector;
using UnityEngine;

public class S_GameManager : MonoBehaviour
{
    [TabGroup("Settings")]
    [Title("Mode")]
    [SerializeField] private bool isMainMenu;

    [TabGroup("Settings")]
    [SerializeField] private bool isMenu;

    [TabGroup("Settings")]
    [Title("Audio")]
    [SerializeField] private S_ClassAudio audioMainMenu;

    [TabGroup("Settings")]
    [SerializeField] private S_ClassAudio audioGame;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_OnMainMenu rseOnMainMenu;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_OnMenu rseOnMenu;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_OnGame rseOnGame;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_PlayAudio rsePlayAudio;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_OnShowMouseCursor rseOnShowMouseCursor;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_OnHideMouseCursor rseOnHideMouseCursor;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_OnNeedCursor rseOnNeedCursor;

    [TabGroup("Outputs")]
    [SerializeField] private RSO_SettingsSaved rsoSettingsSaved;

    private void Awake()
    {
        Application.targetFrameRate = 120;
    }

    private void OnDisable()
    {
        rsoSettingsSaved.Value = new();
    }

    private void Start()
    {
        if (isMainMenu)
        {
            rseOnShowMouseCursor.Call();
            rseOnNeedCursor.Call(true);
            rseOnMainMenu.Call();
            rsePlayAudio.Call(audioMainMenu);
        }
        else
        {
            rsePlayAudio.Call(audioGame);

            if (isMenu)
            {
                rseOnShowMouseCursor.Call();
                rseOnNeedCursor.Call(true);
                rseOnMenu.Call();
            }
            else
            {
                rseOnHideMouseCursor.Call();
                rseOnNeedCursor.Call(false);
                rseOnGame.Call();
            }
        }
    }
}