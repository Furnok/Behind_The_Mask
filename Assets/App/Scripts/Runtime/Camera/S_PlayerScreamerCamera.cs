using DG.Tweening;
using System.Collections;
using UnityEngine;

public class S_PlayerScreamerCamera : MonoBehaviour
{
    //[Header("Settings")]

    //[Header("References")]

    [Header("Inputs")]
    [SerializeField] RSE_OnPlayerGettingCatch _onPlayerGettingCatch;

    [Header("Outputs")]
    [SerializeField] RSE_OnGameOver _onGameOver;

    [Header("References")]
    [SerializeField] Camera _cam;
    [SerializeField] Transform _cameraPivot;

    [Header("Settings")]
    [SerializeField] float _duration = 0.35f;
    [SerializeField] float _fovOnScream = 35f;
    [SerializeField] float _shakeDuration = 0.25f;
    [SerializeField] float _shakeStrength = 0.25f;
    [SerializeField] float _delayToCallGameOVer = 3f;

    float _initialFov;
    Tween _seq;
    Coroutine _gameOverCoroutine;

    private void Awake()
    {
        _initialFov = _cam.fieldOfView;
    }

    void OnEnable()
    {
        _onPlayerGettingCatch.action += PlayScreamer;
    }

    void OnDisable()
    {
        _onPlayerGettingCatch.action -= PlayScreamer;
    }

    public void PlayScreamer(Transform enemyFace)
    {
        if (enemyFace == null) return;

        if (_gameOverCoroutine == null)
            _gameOverCoroutine = StartCoroutine(CallGameOverCoroutine());

        _seq?.Kill();

        Vector3 dir = (enemyFace.position - _cameraPivot.position).normalized;
        Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up);

        Sequence s = DOTween.Sequence();

        s.Join(_cameraPivot.DORotateQuaternion(targetRot, _duration).SetEase(Ease.OutCubic));

        s.Join(DOTween.To(() => _cam.fieldOfView, v => _cam.fieldOfView = v, _fovOnScream, _duration)
            .SetEase(Ease.OutCubic));

        s.Join(_cameraPivot.DOShakePosition(_shakeDuration, _shakeStrength).SetEase(Ease.OutQuad));

        _seq = s;
    }

    IEnumerator CallGameOverCoroutine()
    {
        yield return new WaitForSeconds(_delayToCallGameOVer);
        _onGameOver.Call();
        Debug.Log("Game Over called from S_PlayerScreamerCamera");
    }
}