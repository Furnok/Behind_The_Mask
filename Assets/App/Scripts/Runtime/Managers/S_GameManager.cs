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

    //[Header("References")]

    //[Header("Inputs")]

    [TabGroup("Outputs")]
    [SerializeField] private RSE_OnMainMenu rseOnMainMenu;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_OnMenu rseOnMenu;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_OnGame rseOnGame;

    [TabGroup("Outputs")]
    [SerializeField] private RSE_PlayAudio rsePlayAudio;

    [TabGroup("Outputs")]
    [SerializeField] private RSO_SettingsSaved rsoSettingsSaved;

    private void OnDisable()
    {
        rsoSettingsSaved.Value = new();
    }

    private void Start()
    {
        if (isMainMenu)
        {
            rseOnMainMenu.Call();
            rsePlayAudio.Call(audioMainMenu);
        }
        else
        {
            rsePlayAudio.Call(audioGame);

            if (isMenu)
            {
                rseOnMenu.Call();
            }
            else
            {
                rseOnGame.Call();
            }
        }
    }
}