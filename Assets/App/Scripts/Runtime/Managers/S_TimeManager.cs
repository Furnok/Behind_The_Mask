using Sirenix.OdinInspector;
using UnityEngine;

public class S_TimeManager : MonoBehaviour
{
    [TabGroup("Inputs")]
    [SerializeField] private RSE_OnGamePause rseOnGamePause;

    [TabGroup("Outputs")]
    [SerializeField] private RSO_GameInPause rsoGameInPause;

    private float _baseFixedDelta = 0;
    private float _gameTimeScale = 1f;

    private void Awake()
    {
        _baseFixedDelta = Time.fixedDeltaTime;
        _gameTimeScale = 1f;

        _gameTimeScale = 1f;
        PauseValueChange(false);
    }

    private void OnEnable()
    {
        rseOnGamePause.action += PauseValueChange;
    }

    private void OnDisable()
    {
        rseOnGamePause.action -= PauseValueChange;

        _gameTimeScale = 1f;
        rsoGameInPause.Value = false;
        ApplyGameplayTimeScale();
    }

    private void PauseValueChange(bool newPauseState)
    {
        rsoGameInPause.Value = newPauseState;
        ApplyGameplayTimeScale();
    }

    private void ApplyGameplayTimeScale()
    {
        float effective = rsoGameInPause.Value ? 0f : _gameTimeScale;
        Time.timeScale = effective;
        Time.fixedDeltaTime = _baseFixedDelta * Mathf.Max(effective, 0.01f);
    }
}