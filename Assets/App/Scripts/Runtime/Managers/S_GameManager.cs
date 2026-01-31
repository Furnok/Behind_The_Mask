using Sirenix.OdinInspector;
using UnityEngine;

public class S_GameManager : MonoBehaviour
{
    [TabGroup("Settings")]
    [Title("Mode")]
    [SerializeField] private bool isMainMenu;

    //[Header("References")]

    //[Header("Inputs")]

    [TabGroup("Outputs")]
    [SerializeField] private RSE_OnMainMenu rseOnMainMenu;

    private void Start()
    {
        if (isMainMenu) rseOnMainMenu.Call();
    }
}